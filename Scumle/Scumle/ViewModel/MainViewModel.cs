using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Scumle.Model;
using Scumle.Model.Shapes;
using Scumle.UndeRedo;
using Scumle.UndeRedo.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Scumle.ViewModel.Shapes;
using Scumle.View.Preview;

namespace Scumle.ViewModel
{
    class MainViewModel : ViewModelBase<Model.Scumle>, INotifyPropertyChanged
    {
        #region Fields
        public static ETool _tool = ETool.Default;
        private double _zoom = 1.0;
        private readonly double _ZOOMFACTOR = 750;
        private ConnectionPointViewModel _connectionFrom = null;
        private ConnectionPointViewModel _connectionTo = null;
        private bool _isMouseDownOnGrid;
        private double _selectionX;
        private double _selectionY;
        private double _selectionWidth;
        private double _selectionHeight;
        private double _connectionX1 = 0;
        private double _connectionY1 = 0;
        private double _connectionX2 = 0;
        private double _connectionY2 = 0;
        private bool _isOneConnectedPoint = false;
        private string _currentFilePath = null;
        private string _statusText;
        private static Color _selectedColor;

        public UndoRedoController UndoRedo = UndoRedoController.Instance;
        #endregion

        #region Properties
        public int SelectedConnector { get; set; }
        public int SelectedFigure { get; set; }
        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged(); }
        }
        public ETool Tool
        {
            get { return _tool; }
            set { _tool = value; OnPropertyChanged(); }
        }
        public double ConnectionX1
        {
            get { return _connectionX1; }
            set { _connectionX1 = value; OnPropertyChanged(); }
        }
        public double ConnectionY1
        {
            get { return _connectionY1; }
            set { _connectionY1 = value; OnPropertyChanged(); }
        }
        public double ConnectionX2
        {
            get { return _connectionX2; }
            set { _connectionX2 = value; OnPropertyChanged(); }
        }
        public double ConnectionY2
        {
            get { return _connectionY2; }
            set { _connectionY2 = value; OnPropertyChanged(); }
        }
        public double SelectionX
        {
            get { return _selectionX; }
            set { _selectionX = value; OnPropertyChanged(); }
        }
        public double SelectionY
        {
            get { return _selectionY; }
            set { _selectionY = value; OnPropertyChanged(); }
        }
        public double SelectionWidth
        {
            get { return _selectionWidth; }
            set { _selectionWidth = value; OnPropertyChanged(); }
        }
        public double SelectionHeight
        {
            get { return _selectionHeight; }
            set { _selectionHeight = value; OnPropertyChanged(); }
        }
        private Point StartingPoint { get; set; }
        public double Zoom
        {
            get { return _zoom; }
            set { _zoom = value; OnPropertyChanged(); }
        }

        public static Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                foreach (var viewModel in ShapesPreview.List)
                {
                    var actualViewModel = viewModel as IShape;
                    actualViewModel.ShapeColor = new SolidColorBrush(value);
                }
            }
        }
        public ObservableCollection<IShape> Shapes { get; }
        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();
        public List<IShape> Selected { get; } = new List<IShape>();
        public ObservableCollection<LineViewModel> CopiedLines { get; } = new ObservableCollection<LineViewModel>();
        public List<IShape> CopiedShapes { get; } = new List<IShape>();
        public string Version { get; } = "Version 1.0.0";
        #endregion

        #region Commands
        public ICommand ZoomCommand => new RelayCommand<MouseWheelEventArgs>(ZoomEvent);
        public ICommand SetLineConnectionCommand => new RelayCommand(SetLineConnection);
        public ICommand ExportImageCommand => new RelayCommand<Window>(ExportImage);
        public ICommand ChangeZoomCommand => new RelayCommand<double>(ChangeZoom);
        public ICommand SetShapeSelectionCommand => new RelayCommand(SetShapeInsertion);
        public ICommand SaveAsWorkSpaceCommand => new RelayCommand(SaveAsWorkSpace);
        public ICommand SaveWorkSpaceCommand => new RelayCommand(SaveWorkSpace);
        public ICommand OpenWorkSpaceCommand => new RelayCommand(OpenWorkSpace);
        public ICommand CopyCommand => new RelayCommand(Copy);
        public ICommand CutCommand => new RelayCommand(Cut);
        public ICommand PasteCommand => new RelayCommand(Paste);
        public RelayCommand DeleteSelectedShapesCommand { get; set; }
        public ICommand NewWorkSpaceCommand => new RelayCommand(NewWorkSpace);
        public ICommand UndoCommand => UndoRedoController.Instance.UndoCommand;
        public ICommand RedoCommand => UndoRedoController.Instance.RedoCommand;
        public ICommand LineToConnectionCommand => new RelayCommand<MouseEventArgs>(LineToConnection);
        public ICommand MouseDownGridCommand => new RelayCommand<MouseButtonEventArgs>(GridMouseDown);
        public ICommand MouseMoveGridCommand => new RelayCommand<MouseEventArgs>(GridMouseMove);
        public ICommand MouseUpGridCommand => new RelayCommand<MouseButtonEventArgs>(GridMouseUp);
        public ICommand EscCommand => new RelayCommand(Escape);
        public ICommand SelectAllCommand => new RelayCommand(SelectAll);
        public ICommand ColorSelectedCommand => new RelayCommand(ColorSelected);
        public ICommand ExitCommand => new RelayCommand(Exit);

        #endregion

        #region Constructor
        public MainViewModel(Model.Scumle scumle) : base(scumle)
        {
            SelectedColor = Color.FromRgb(205, 92, 92);

            string UMLFields = "-First field : int\n-Second Field : String";
            string UMLMethods = "+First method()\n+Second method()";
            IShape uml1 = new UMLClassViewModel(new UMLClass(400, 400, 300, 200, "My Class 1", SelectedColor, CreateShapeID(), UMLFields, UMLMethods));
            IShape uml2 = new UMLClassViewModel(new UMLClass(50, 50, 300, 200, "My Class 2", SelectedColor, CreateShapeID(), UMLFields, UMLMethods));

            IShape shape1 = new BasicShapeViewModel(new BasicShape(EBasicShape.Ellipse, 400, 50, 50, 50, SelectedColor, CreateShapeID()));
            IShape shape2 = new BasicShapeViewModel(new BasicShape(EBasicShape.Rectangle, 50, 400, 50, 50, SelectedColor, CreateShapeID()));


            Shapes = new ObservableCollection<IShape>() { uml1, uml2, shape1, shape2 };





            IPoint cp1 = uml1.ConnectionPoints.ElementAt(0);
            IPoint cp2 = uml2.ConnectionPoints.ElementAt(3);

            Lines.Add(new LineViewModel(new Line(ELine.Inheritance, cp1, cp2)));



            DeleteSelectedShapesCommand = new RelayCommand(DeleteSelectedShapes, HasSelectedShapes);
        }
        #endregion

        #region Methods
        private void LineToConnection(MouseEventArgs e)
        {
            FrameworkElement source = e.Source as FrameworkElement;
            ConnectionPointViewModel point = source.DataContext as ConnectionPointViewModel;

            if (_connectionFrom == null)
            {
                _connectionFrom = point;
                _connectionFrom.ShapeColor = new SolidColorBrush(Color.FromRgb(51, 255, 51));
                _isOneConnectedPoint = true;
                ConnectionX1 = _connectionFrom.CenterX;
                ConnectionY1 = _connectionFrom.CenterY;

            }
            else if (_connectionTo == null)
            {
                _connectionTo = point;
            }

            if (_connectionFrom != null && _connectionTo != null)
            {
                if (_connectionFrom != _connectionTo)
                {
                    new LineAddCommand(Lines, new LineViewModel(new Line((ELine)SelectedConnector, _connectionFrom, _connectionTo))).Execute();
                }

                EndLineConnection();
            }
        }

        internal void ZoomEvent(MouseWheelEventArgs e)
        {
            double change = ((double)e.Delta) / _ZOOMFACTOR;
            Zoom *= (1.0 + change);
            e.Handled = true;
        }
        #endregion

        #region
        //Nemt at implementere når tingene bliver serializable https://www.codeproject.com/articles/23832/implementing-deep-cloning-via-serializing-objects
        private void Copy()
        {
            foreach (IShape i in Selected)
            {
                CopiedShapes.Add(i);
            }
        }
        private void Cut()
        {
            Copy();
            DeleteSelectedShapes();
        }
        private void Paste()
        {
            foreach (IShape i in CopiedShapes)
            {
                Shapes.Add(i);
            }
        }
        #endregion

        #region selection
        private void SelectAll()
        {
            foreach (IShape i in Shapes)
            {
                SelectShape(i, false);
            }
        }
        private void Escape()
        {
            Tool = ETool.Default;
            DeselectAllShapes();
            EndLineConnection();
        }
        internal void SelectShape(IShape shape, bool clearSelection)
        {
            if (shape == null) return;
            if (clearSelection)
            {
                DeselectAllShapes();
                Selected.Add(shape);
                shape.IsSelected = true;
                DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Selected.Add(shape);
                shape.IsSelected = true;
                DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
            }
        }

        internal void DeselectAllShapes()
        {
            foreach (IShape shape in Selected)
            {
                shape.IsSelected = false;
            }
            Selected.Clear();
            DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
        }
        public bool HasSelectedShapes()
        {
            return Selected.Count > 0;
        }
        #endregion

        #region GridMouseEventHandling
        public void GridMouseDown(MouseButtonEventArgs e)
        {
            if (Tool == ETool.Default)
            {
                DeselectAllShapes();
                _isMouseDownOnGrid = true;
                e.MouseDevice.Target.CaptureMouse();
                StartingPoint = e.MouseDevice.GetPosition(e.Source as IInputElement);
            }

            if (Tool == ETool.ShapeTool)
            {
                AddShape(e);
            }
        }
        public void GridMouseMove(MouseEventArgs e)
        {
            Point curPos = e.MouseDevice.GetPosition(e.Source as IInputElement);
            if (_isMouseDownOnGrid)
            {
                SelectionX = Math.Min(StartingPoint.X, curPos.X);
                SelectionY = Math.Min(StartingPoint.Y, curPos.Y);
                SelectionHeight = Math.Abs(curPos.Y - StartingPoint.Y);
                SelectionWidth = Math.Abs(curPos.X - StartingPoint.X);
            }
            else if (_isOneConnectedPoint)
            {
                ConnectionX2 = curPos.X;
                ConnectionY2 = curPos.Y;
            }

        }
        public void GridMouseUp(MouseButtonEventArgs e)
        {

            if (_isMouseDownOnGrid)
            {
                Point endingPoint = e.MouseDevice.GetPosition(e.Source as IInputElement);
                double minX = Math.Min(StartingPoint.X, endingPoint.X);
                double minY = Math.Min(StartingPoint.Y, endingPoint.Y);
                double maxX = Math.Max(StartingPoint.X, endingPoint.X);
                double maxY = Math.Max(StartingPoint.Y, endingPoint.Y);
                foreach (IShape i in Shapes)
                {
                    if (!(i.X > maxX || i.X + i.Width < minX || i.Y > maxY || i.Y + i.Height < minY))
                    {
                        SelectShape(i, false);
                    }
                }
                _isMouseDownOnGrid = false;
                e.MouseDevice.Target.ReleaseMouseCapture();
                SelectionX = 0;
                SelectionY = 0;
                SelectionHeight = 0;
                SelectionWidth = 0;
            }
        }

        #endregion

        #region addShape

        public void SetShapeInsertion()
        {
            Tool = ETool.ShapeTool;
        }

        public void AddShape(MouseButtonEventArgs e)
        {
            string UMLFields = "-First field : int\n-Second Field : String";
            string UMLMethods = "+First method()\n+Second method()";
            Point p = e.MouseDevice.GetPosition(e.Source as IInputElement);
            IShape shape = null;
            switch (SelectedFigure)
            {
                case 0:
                    shape = new BasicShapeViewModel(new BasicShape(EBasicShape.Ellipse, p.X, p.Y, 50, 50, SelectedColor, CreateShapeID()));
                    break;
                case 1:
                    shape = new UMLClassViewModel(new UMLClass(p.X, p.Y, 300, 200, "New Shape", SelectedColor, CreateShapeID(), UMLFields, UMLMethods));
                    break;
                case 2:
                    shape = new BasicShapeViewModel(new BasicShape(EBasicShape.Rectangle, p.X, p.Y, 50, 50, SelectedColor, CreateShapeID()));
                    break;
                default:
                    Console.WriteLine("Figure selection error");
                    break;
            }
            if (shape != null)
                new ShapeAddCommand(Shapes, shape).Execute();
            Tool = ETool.Default;
        }
        #endregion

        #region deletion
        public void DeleteSelectedShapes()
        {
            new ShapeRemoveCommand(Shapes, Lines, Selected).Execute();
            DeselectAllShapes();
        }

        public void DeleteShape(IShape shape)
        {
            shape.IsSelected = false;
            Shapes.Remove(shape);
            RemoveLines(shape);
        }

        public void RemoveLines(IShape shape)
        {
            foreach (LineViewModel line in Lines.ToList())
            {
                if (line.From.Shape == shape || line.To.Shape == shape)
                {
                    Lines.Remove(line);
                }
            }
        }
        #endregion

        #region WorkSpace
        public void SaveWorkSpace()
        {
            if (_currentFilePath != null)
            {
                Helpers.GenericSerializer.convertToXML(saving(), _currentFilePath);
                UndoRedo.ChangeSinceSave=false;
                StatusText = "File: \"" + _currentFilePath + "\" Saved successfully";
            }
            else
            {
                SaveAsWorkSpace();
            }
        }

        public void SaveAsWorkSpace()
        {
                SaveFileDialog save = new SaveFileDialog();
                save.DefaultExt = ".scumle";
                save.Filter = "(.scumle)|*.scumle";
                
                if (save.ShowDialog() == true)
                {
                    _currentFilePath = Path.GetFullPath(save.FileName);
                    Helpers.GenericSerializer.convertToXML(saving(), _currentFilePath);
                UndoRedo.ChangeSinceSave = false;
                }
            

        }

        public List<ModelBase> saving()
        {
            // List containing all models to be saved.
            List<ModelBase> modelsToSave = new List<ModelBase>();

            foreach (var ViewModel in Shapes)
            {
                if (ViewModel is UMLClassViewModel)
                {
                    var actualViewModel = ViewModel as UMLClassViewModel;
                    modelsToSave.Add(actualViewModel.Shape);
                }
                else if (ViewModel is BasicShapeViewModel)
                {
                    var actualViewModel = ViewModel as BasicShapeViewModel;
                    modelsToSave.Add(actualViewModel.Shape);
                }
            }

            foreach (var ViewModel in Lines)
            {
                var actualModel = ViewModel.Model as Line;             
                modelsToSave.Add(actualModel);
            }
            return modelsToSave;
         
        }

        public void OpenWorkSpace()
        {
            if (DiscardChanges())
            {
                OpenFileDialog open = new OpenFileDialog();
                open.DefaultExt = ".scumle";
                open.Filter = "(.scumle)|*.scumle";
                List<ModelBase> loadedModelsList = new List<ModelBase>();

                // Show open file dialog box
                bool? result = open.ShowDialog();

                // Process open file dialog box results
                if (result == true)
                {
                    loadedModelsList = Helpers.GenericSerializer.convertFromXML<List<ModelBase>>(Path.GetFullPath(open.FileName));
                    Shapes.Clear();
                    Lines.Clear();
                    UndoRedo.clear();
                    UndoRedo.ChangeSinceSave = false;
                    _currentFilePath = Path.GetFullPath(open.FileName);
                    foreach (var loadedModel in loadedModelsList)
                    {
                        if (loadedModel is UMLClass)
                        {                            
                            var actualUMLClass = loadedModel as UMLClass;
                            var storedColor = Color.FromRgb(actualUMLClass.ColorR, actualUMLClass.ColorG, actualUMLClass.ColorB);
                            IShape actualViewModel = new UMLClassViewModel(new UMLClass(actualUMLClass.X, actualUMLClass.Y, actualUMLClass.Width, actualUMLClass.Height,
                                                                                        actualUMLClass.Name, storedColor, actualUMLClass.ID, actualUMLClass.UMLFields, actualUMLClass.UMLMethods));
                    
                            Shapes.Add(actualViewModel);
                        }
                        else if (loadedModel is BasicShape)
                        {
                            var actualBasicShape = loadedModel as BasicShape;
                            var storedColor = Color.FromRgb(actualBasicShape.ColorR, actualBasicShape.ColorG, actualBasicShape.ColorB);

                            IShape actualViewModel = new BasicShapeViewModel(new BasicShape(actualBasicShape.Type, actualBasicShape.X, actualBasicShape.Y,
                                                                                            actualBasicShape.Width, actualBasicShape.Height, storedColor, actualBasicShape.ID));
                            Shapes.Add(actualViewModel);

                        }
                        else if (loadedModel is Line)
                        {
                            
                            var actualLine = loadedModel as Line;
                            var from = actualLine.storeFrom;
                            var to = actualLine.storeTo;
                            IPoint cpFrom = null;
                            IPoint cpTo = null;

                            foreach (var viewModel in Shapes)
                            {
                                var actualViewModel = viewModel as IShape;

                                if (actualViewModel.ID == from.storeShape.ID)
                                {
                                    if (from.Horizontal == HorizontalAlignment.Center && from.Vertical == VerticalAlignment.Top) { cpFrom = actualViewModel.ConnectionPoints.ElementAt(0); }
                                    else if (from.Horizontal == HorizontalAlignment.Left && from.Vertical == VerticalAlignment.Center) { cpFrom = actualViewModel.ConnectionPoints.ElementAt(1); }
                                    else if (from.Horizontal == HorizontalAlignment.Right && from.Vertical == VerticalAlignment.Center) { cpFrom = actualViewModel.ConnectionPoints.ElementAt(2); }
                                    else if (from.Horizontal == HorizontalAlignment.Center && from.Vertical == VerticalAlignment.Bottom) { cpFrom = actualViewModel.ConnectionPoints.ElementAt(3); }
                                }

                                if (actualViewModel.ID == to.storeShape.ID)
                                {
                                    if (to.Horizontal == HorizontalAlignment.Center && to.Vertical == VerticalAlignment.Top) { cpTo = actualViewModel.ConnectionPoints.ElementAt(0); }
                                    else if (to.Horizontal == HorizontalAlignment.Left && to.Vertical == VerticalAlignment.Center) { cpTo = actualViewModel.ConnectionPoints.ElementAt(1); }
                                    else if (to.Horizontal == HorizontalAlignment.Right && to.Vertical == VerticalAlignment.Center) { cpTo = actualViewModel.ConnectionPoints.ElementAt(2); }
                                    else if (to.Horizontal == HorizontalAlignment.Center && to.Vertical == VerticalAlignment.Bottom) { cpTo = actualViewModel.ConnectionPoints.ElementAt(3); }

                                }
                            }                                


                            Lines.Add(new LineViewModel(new Line(actualLine.Type, cpFrom, cpTo)));

                        }
                    }
                }
            }


         


        }



        public string CreateShapeID()
        {
            return Guid.NewGuid().ToString();

        }

        //TODO: Implement adding a new "window pane" instead of just deleting the one we have.
        public void NewWorkSpace()
        {

            if (DiscardChanges())
            {
                _currentFilePath = null;
                Shapes.Clear();
                Lines.Clear();
                UndoRedo.clear();
            }
        }
        private bool DiscardChanges()
        {

            if (UndoRedo.ChangeSinceSave)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Changes to your file will be lost.\n Do you wish to proceed?", "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (messageBoxResult == MessageBoxResult.No)
                {
                    return false;
                }
            }
            return true;

        }

        #endregion

        #region methods
        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void SetLineConnection()
        {
            Tool = ETool.LineTool;
        }
        private void EndLineConnection()
        {
            if (_connectionFrom != null)
            {
                _connectionFrom.ShapeColor = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
            if (_connectionTo != null)
            {
                _connectionTo.ShapeColor = new SolidColorBrush(Color.FromRgb(128, 128, 128));
            }
            _connectionFrom = null;
            _connectionTo = null;
            _isOneConnectedPoint = false;
            ConnectionX1 = 0;
            ConnectionX2 = 0;
            ConnectionY1 = 0;
            ConnectionY2 = 0;
            Tool = ETool.Default;
        }

        public void ColorSelected()
        {
            new ShapeColorCommand(Selected, new SolidColorBrush(SelectedColor)).Execute();
        }

        //The image conversion code is inspired by http://stackoverflow.com/questions/4560173/save-wpf-view-as-image-preferably-png
        public void ExportImage(Window grid)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".png";
            if (save.ShowDialog() == true)
            {
                Size size = new Size(grid.ActualWidth, grid.ActualHeight);
                RenderTargetBitmap img = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                DrawingVisual drawingvisual = new DrawingVisual();
                using (DrawingContext context = drawingvisual.RenderOpen())
                {
                    context.DrawRectangle(new VisualBrush(grid), null, new Rect(new Point(), size));
                    context.Close();
                }
                img.Render(drawingvisual);

                var fileStream = File.Create(save.FileName);

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(img));
                encoder.Save(fileStream);

                fileStream.Close();

            }
        }

        public void ChangeZoom(double value)
        {
            Zoom = value;
        }
        #endregion

    }

}
