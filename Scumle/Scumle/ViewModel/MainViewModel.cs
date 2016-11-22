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

namespace Scumle.ViewModel
{
    class MainViewModel : ViewModelBase<Model.Scumle>, INotifyPropertyChanged
    {
        #region Fields
        private ETool _tool = ETool.Default;
        private int _num = 0;
        private double _zoom = 1.0;
        private ConnectionPointViewModel _connectionFrom = null;
        private ConnectionPointViewModel _connectionTo = null;
        private bool _isMouseDownOnGrid;
        private double _selectionX;
        private double _selectionY;
        private double _selectionWidth;
        private double _selectionHeight;
        public UndoRedoController UndoRedo = UndoRedoController.Instance;
        #endregion

        #region Properties
        public ETool Tool
        {
            get { return _tool; }
            set { _tool = value; OnPropertyChanged(); }
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

        public static Color SelectedColor { get; set; }
        public ObservableCollection<ShapeViewModel> Shapes { get; }
        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();
        public List<ShapeViewModel> Selected { get; } = new List<ShapeViewModel>();
        public ObservableCollection<LineViewModel> CopiedLines { get; } = new ObservableCollection<LineViewModel>();
        public List<ShapeViewModel> CopiedShapes { get; } = new List<ShapeViewModel>();
        public String Version { get; } = "Version 1.0.0";
        #endregion

        #region Commands
        public ICommand SetLineConnectionCommand => new RelayCommand(SetLineConnection);
        public ICommand ExportImageCommand => new RelayCommand<Canvas>(ExportImage);
        public ICommand ChangeZoomCommand => new RelayCommand<string>(ChangeZoom);
        public ICommand SetShapeSelectionCommand => new RelayCommand(SetShapeInsertion);
        public ICommand SaveWorkspaceCommand => new RelayCommand(SaveWorkspace);
        public ICommand OpenWorkspaceCommand => new RelayCommand(OpenWorkspace);
        public RelayCommand DeleteSelectedShapesCommand => new RelayCommand(DeleteSelectedShapes, HasSelectedShapes);
        public ICommand NewWorkspaceCommand => new RelayCommand(NewWorkspace);
        public ICommand UndoCommand => UndoRedoController.Instance.UndoCommand;
        public ICommand RedoCommand => UndoRedoController.Instance.RedoCommand;
        public ICommand AddShapeCommand { get; } // TODO: This is not implemented?
        public ICommand LineToConnectionCommand => new RelayCommand<MouseEventArgs>(LineToConnection);
        public ICommand MouseDownGridCommand => new RelayCommand<MouseButtonEventArgs>(GridMouseDown);
        public ICommand MouseMoveGridCommand => new RelayCommand<MouseEventArgs>(GridMouseMove);
        public ICommand MouseUpGridCommand => new RelayCommand<MouseButtonEventArgs>(GridMouseUp);
        public ICommand EscCommand => new RelayCommand(Escape);
        public ICommand SelectAllCommand => new RelayCommand(SelectAll);
        public ICommand ColorSelectedCommand => new RelayCommand(ColorSelected);
        #endregion

        #region Constructor
        public MainViewModel(Model.Scumle scumle) : base(scumle)
        {
            ShapeViewModel uml1 = new UMLClassViewModel(new UMLClass(400, 400, "My frist shape"));
            ShapeViewModel uml2 = new UMLClassViewModel(new UMLClass(50, 50, "My second shape"));
            Shapes = new ObservableCollection<ShapeViewModel>() { uml1, uml2 };

            ConnectionPointViewModel cp1 = uml1.ConnectionPoints.ElementAt(0);
            ConnectionPointViewModel cp2 = uml2.ConnectionPoints.ElementAt(1);

            Lines.Add(new LineViewModel(cp1, cp2));

            SelectedColor = Color.FromRgb(0, 153, 255);
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
            }
            else if (_connectionTo == null)
            {
                _connectionTo = point;
            }
            
            if (_connectionFrom != null && _connectionTo != null)
            {
                if (_connectionFrom != _connectionTo)
                {
                    new LineAddCommand(Lines, new LineViewModel(_connectionFrom, _connectionTo)).Execute();
                }

                _connectionFrom = null;
                _connectionTo = null;
                EndLineConnection();
            }
        }
        #endregion

        #region selection
        private void SelectAll()
        {
            foreach (ShapeViewModel i in Shapes)
            {
                SelectShape(i, false);
                
            }
        }
        private void Escape()
        {
            Tool = ETool.Default;
            DeselectAllShapes();
        }
        internal void SelectShape(ShapeViewModel shape, bool clearSelection)
        {
            if (clearSelection)
            {
                DeselectAllShapes();
                Selected.Add(shape);
                shape.IsSelected = true;
                DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
            } else {
                Selected.Add(shape);
                shape.IsSelected = true;
                DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
            }
        }

        internal void DeselectAllShapes()
        {
            foreach (ShapeViewModel shape in Selected)
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
                foreach (ShapeViewModel i in Shapes)
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
            Point p = e.MouseDevice.GetPosition(e.Source as IInputElement);
            ShapeViewModel shape = new UMLClassViewModel(new Eclipse(p.X, p.Y, "My shape " + _num++));
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

        public void DeleteShape(ShapeViewModel shape)
        {
            shape.IsSelected = false;
            Shapes.Remove(shape);
            RemoveLines(shape);
        }

        public void RemoveLines(ShapeViewModel shape)
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

        public void SaveWorkspace()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".scumle";
            save.Filter = "(.scumle)|*.scumle";
            if (save.ShowDialog() == true)
            {
                // List containing all models to be saved.
                List<List<ModelBase>> modelsToSave = new List<List<ModelBase>>();

                // Lists for each different type of 
                List<ModelBase> shapesToSave = new List<ModelBase>();
                List<ModelBase> linesToSave = new List<ModelBase>();

                foreach (var ViewModel in Shapes)
                {
                    shapesToSave.Add(ViewModel.Model);
                }

                foreach (var ViewModel in Lines)
                {
                    linesToSave.Add(ViewModel.Model);
                }

                modelsToSave.Add(shapesToSave);
                modelsToSave.Add(linesToSave);

                Helpers.GenericSerializer.convertToXML(modelsToSave, Path.GetFullPath(save.FileName));
            }


        }

        public void OpenWorkspace()
        {

            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".scumle";
            open.Filter = "(.scumle)|*.scumle";
            List<Shape> loadedModels = new List<Shape>();



            // Show open file dialog box
            Nullable<bool> result = open.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                loadedModels = Helpers.GenericSerializer.convertFromXML<List<Shape>>(Path.GetFullPath(open.FileName));
                Shapes.Clear();
                foreach (var loadedModel in loadedModels)
                {
                    if (loadedModel is UMLClass)
                    {
                        Shapes.Add(new UMLClassViewModel(loadedModel));
                    }
                    else
                    {
                        Shapes.Add(new ShapeViewModel(loadedModel));
                    }
                }

            }
        }

        //TODO: Implement adding a new "window pane" instead of just deleting the one we have.
        public void NewWorkspace()
        {
            Shapes.Clear();
        }

        #endregion

        #region methods
        private void SetLineConnection()
        {
            Tool = ETool.LineTool;
        }
        private void EndLineConnection()
        {
            Tool = ETool.Default;
        }
        public void ColorSelected()
        {
            new ShapeColorCommand(Selected, new SolidColorBrush(SelectedColor)).Execute();
        }

        //The majority of this image conversion code is from http://stackoverflow.com/questions/4560173/save-wpf-view-as-image-preferably-png
        public static void ExportImage(Canvas grid)
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

            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".png";
            var fileStream = File.Create(save.FileName);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            encoder.Save(fileStream);
    
            fileStream.Close();

        }

        public void ChangeZoom(string value)
        {
            Zoom = Double.Parse(value);
        }
        #endregion

    }

}
