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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Scumle.ViewModel.Shapes;
using Scumle.View.Preview;
using System.Windows.Controls.Primitives;
using System.Windows;
using Scumle.Helpers;

namespace Scumle.ViewModel
{
    class MainViewModel : ViewModelBase<Main>, INotifyPropertyChanged
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
        private string _memoryOfCopy;
        private double _loadingOffSet = 5;
        private static Color _selectedColor;
        private System.Drawing.Point OldMousePos;
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
        public ObservableCollection<ILine> Lines { get; } = new ObservableCollection<ILine>();
        public List<IShape> Selected { get; } = new List<IShape>();
        public ObservableCollection<ILine> CopiedLines { get; } = new ObservableCollection<ILine>();
        public ObservableCollection<IShape> CopiedShapes { get; } = new ObservableCollection<IShape>();
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
        public RelayCommand CopyCommand { get; private set; }
        public RelayCommand CutCommand { get; private set; }
        public RelayCommand PasteCommand { get; private set; }
        public RelayCommand DeleteSelectedShapesCommand { get; private set; }
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
        public ICommand MoveShapesCommand => new RelayCommand<DragDeltaEventArgs>(MoveShapes);
        public ICommand StartMoveShapesCommand => new RelayCommand<DragStartedEventArgs>(StartMoveShapes);
        public ICommand EndMoveShapesCommand => new RelayCommand<DragCompletedEventArgs>(EndMoveShapes);
        public ICommand WindowExitCommand => new RelayCommand<CancelEventArgs>(WindowExit);
        #endregion

        #region Moving
        private void MoveShapes(DragDeltaEventArgs e)
        {
            IShape shape = getShapeFromElement(e.Source);
            if (!shape.IsSelected)
            {
                SelectShape(shape, true);
            }

            foreach (IShape i in Selected)
            {
                i.ShapeMove(e.HorizontalChange, e.VerticalChange);
            }
        }

        private void StartMoveShapes(DragStartedEventArgs e)
        {
            IShape shape = getShapeFromElement(e.Source);
            OldMousePos = System.Windows.Forms.Cursor.Position;
        }

        private IShape getShapeFromElement(object obj)
        {
            FrameworkElement element = obj as FrameworkElement;
            return element.DataContext as IShape;
        }

        private void EndMoveShapes(DragCompletedEventArgs e)
        {
            System.Drawing.Point NewMousePos = System.Windows.Forms.Cursor.Position;
            if (NewMousePos.Equals(OldMousePos))
            {
                IShape shape = getShapeFromElement(e.Source);
                bool clearSelection = !Keyboard.IsKeyDown(Key.LeftShift);
                SelectShape(shape, clearSelection);
            }
            else
            {
                double offsetX = NewMousePos.X - OldMousePos.X;
                double offsetY = NewMousePos.Y - OldMousePos.Y;
                new ShapeMoveCommand(Selected, offsetX, offsetY).Add();
            }
        }

        #endregion

        #region Constructor
        public MainViewModel(Main scumle) : base(scumle)
        {
            SelectedColor = Color.FromRgb(205, 92, 92);



            Shapes = new ObservableCollection<IShape>();

            DeleteSelectedShapesCommand = new RelayCommand(DeleteSelectedShapes, HasSelectedShapes);
            CopyCommand = new RelayCommand(Copy, HasSelectedShapes);
            PasteCommand = new RelayCommand(Paste, HasCopiedShapes);
            CutCommand = new RelayCommand(Cut, HasSelectedShapes);
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

        #region copypaste

        private bool HasCopiedShapes()
        {
            return CopiedShapes.Any();
        }

        private void Copy()
        {
            CopiedShapes.Clear();
            CopiedLines.Clear();
            foreach (IShape i in Selected)
            {
                foreach (LineViewModel l in Lines)
                { 
                    // Copy the line if it is originating from a shape.
                    if (l.From.Shape.ID == i.ID)
                    {
                        foreach(IShape j in Selected)
                        {
                            if(l.To.Shape.ID == j.ID )
                            {
                                CopiedLines.Add(l);
                            }
                        }
                     
                    }
                    
                }
                             
                CopiedShapes.Add(i);
                
            }          
            _memoryOfCopy = GenericSerializer.SerializeToXMLInMemory(saving(CopiedShapes, CopiedLines));
            PasteCommand.RaiseCanExecuteChanged(); 
        }
        private void Cut()
        {
            Copy();
            DeleteSelectedShapes();
        }
        private void Paste()
        {
            DeselectAllShapes();
            List<ModelBase> copyMemoryShapes = Helpers.GenericSerializer.convertFromXMLInMemory(_memoryOfCopy);
            List<string> newIDList = new List<string>();
            foreach (var model in copyMemoryShapes)
            {

                if (model is Shape)
                {
                    string newID = CreateShapeID();

                    foreach (var line in copyMemoryShapes)
                    {
                        if (line is Line && (line as Line).StoreFromId == (model as Shape).ID)
                        {
                            (line as Line).StoreFromId = newID;

                        }
                        if (line is Line && (line as Line).StoreToId == (model as Shape).ID)
                        {
                            (line as Line).StoreToId = newID;

                        }


                    }

                       (model as Shape).ID = newID;
                    newIDList.Add(newID);
                }
            }
            if (copyMemoryShapes != null)
            { loading(copyMemoryShapes); }


                foreach (IShape shape in Shapes)
                {
                    if (newIDList.Contains(shape.ID)) { SelectShape(shape, false); }
                }



        }

        private void loading(List<ModelBase> loadedModelsList)
        {
            IList<ILine> linesAdd = new List<ILine>();
            IList<IShape> shapesAdd = new List<IShape>();
            foreach (var loadedModel in loadedModelsList)
            {

                if (loadedModel is UMLClass)
                {
                    var actualUMLClass = loadedModel as UMLClass;
                    var storedColor = Color.FromRgb(actualUMLClass.ColorR, actualUMLClass.ColorG, actualUMLClass.ColorB);

                    IShape actualViewModel = new UMLClassViewModel(new UMLClass(actualUMLClass.X + _loadingOffSet, actualUMLClass.Y + _loadingOffSet, actualUMLClass.Width, actualUMLClass.Height,
                                                                           actualUMLClass.Name, storedColor, actualUMLClass.ID, actualUMLClass.UMLFields, actualUMLClass.UMLMethods));

                    shapesAdd.Add(actualViewModel);
                }
                else if (loadedModel is BasicShape)
                {
                    var actualBasicShape = loadedModel as BasicShape;
                    var storedColor = Color.FromRgb(actualBasicShape.ColorR, actualBasicShape.ColorG, actualBasicShape.ColorB);


                    IShape actualViewModel = new BasicShapeViewModel(new BasicShape(actualBasicShape.Type, actualBasicShape.X + _loadingOffSet, actualBasicShape.Y + _loadingOffSet,
                                                                           actualBasicShape.Width, actualBasicShape.Height, storedColor, actualBasicShape.ID));
                    shapesAdd.Add(actualViewModel);

                }
                else if (loadedModel is Line)
                {

                    var actualLine = loadedModel as Line;
                    string fromID = actualLine.StoreFromId;
                    string toID = actualLine.StoreToId;
                    IPoint cpFrom = null;
                    IPoint cpTo = null;

                    foreach (var viewModel in shapesAdd)
                    {
                        var actualViewModel = viewModel as IShape;

                        if (actualViewModel.ID == fromID)
                        {

                            cpFrom = actualViewModel.ConnectionPoints.ElementAt(0);
                        }

                        if (actualViewModel.ID == toID)
                        {

                            cpTo = actualViewModel.ConnectionPoints.ElementAt(0);

                        }
                    }

                    if (cpFrom != null || cpTo != null) { linesAdd.Add(new LineViewModel(new Line(actualLine.Type, cpFrom, cpTo))); }

                }

            }

            new MultiUndoRedoCommand(new List<UndoRedoCommand>() {
                new ShapeAddCommand(Shapes, shapesAdd),
                new LineAddCommand(Lines, linesAdd)
            }).Execute();
        }

        public List<ModelBase> saving(ObservableCollection<IShape> shapesToSave, ObservableCollection<ILine> linesToSave)
        {
            // List containing all models to be saved.
            List<ModelBase> modelsToSave = new List<ModelBase>();

            foreach (var ViewModel in shapesToSave)
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

            foreach (var viewModel in linesToSave)
            {
                var actualModel = (viewModel as LineViewModel).Model as Line;
                modelsToSave.Add(actualModel);
            }
            return modelsToSave;

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
            Keyboard.ClearFocus();
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
                CopyCommand.RaiseCanExecuteChanged();
                CutCommand.RaiseCanExecuteChanged();
            }
            else
            {
                Selected.Add(shape);
                shape.IsSelected = true;
                DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
                CopyCommand.RaiseCanExecuteChanged();
                CutCommand.RaiseCanExecuteChanged();
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
            CopyCommand.RaiseCanExecuteChanged();
            CutCommand.RaiseCanExecuteChanged();
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

        #region SaveLoadWorkspace
        public void SaveWorkSpace()
        {
            if (_currentFilePath != null)
            {

                SaveThread saveThread = new SaveThread(Shapes, Lines, _currentFilePath);
                saveThread.Start();       
                UndoRedo.ChangeSinceSave=false;

                StatusText = "File: \"" + _currentFilePath + "\" Saved successfully";
            }
            else
            {
                SaveAsWorkSpace();
                StatusText = "File: \"" + _currentFilePath + "\" Saved successfully";
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
                SaveThread saveThread = new SaveThread(Shapes, Lines, _currentFilePath);
                saveThread.Start();
                UndoRedo.ChangeSinceSave = false;

            }

            StatusText = "File: \"" + _currentFilePath + "\" Saved successfully";

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
                    LoadThread loadThread = new LoadThread(Shapes, Lines, Path.GetFullPath(open.FileName));
                    loadThread.Start();
                    Shapes.Clear();
                    Lines.Clear();
                    UndoRedo.clear();
                    UndoRedo.ChangeSinceSave = false;
                    _currentFilePath = Path.GetFullPath(open.FileName);             
                }
            }
        }
    

        public string CreateShapeID()
        {
            return Guid.NewGuid().ToString();

        }

   
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
                MessageBoxResult messageBoxResult = MessageBox.Show("Changes to your file will be lost.\n Do you wish to proceed?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Question);

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
            if (DiscardChanges())
            {
                Application.Current.Shutdown();
            }
        }
        private void WindowExit(CancelEventArgs e)
        {
            if (DiscardChanges())
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
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

        public void ExportImage(Window window)
        {
            FrameworkElement diagramUsercontrol = window.FindName("Print") as FrameworkElement;
            Grid grid = diagramUsercontrol.FindName("Diagram") as Grid;
            Escape();
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".png";
            if (save.ShowDialog() == true)
            {
                Size size = new Size(grid.ActualWidth, grid.ActualHeight);
                RenderTargetBitmap img = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(new VisualBrush(grid), null, new Rect(new Point(), size));
                    context.Close();
                }
                img.Render(visual);

                FileStream fileStream = File.Create(save.FileName);

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
