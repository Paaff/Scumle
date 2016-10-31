using Microsoft.Win32;
using Scumle.Helpers;
using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Scumle.ViewModel
{
    class MainViewModel : ViewModelBase<Model.Scumle>
    {
        #region Fields
        private int _num = 0;
        private double _zoom = 1.0;
        #endregion

        #region Properties
        public double Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ShapeViewModel> Shapes { get;}

        public ObservableCollection<ShapeViewModel> Selected { get; } = new ObservableCollection<ShapeViewModel>();

        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();

        public String Version { get; } = "Version 1.0.0";
        #endregion

        #region Commands

        public RelayCommand<string> ChangeZoomCommand { get; set; }

        public RelayCommand AddShapeCommand { get; private set; }

        public RelayCommand SaveWorkspaceCommand { get; }

        public RelayCommand OpenWorkspaceCommand { get; }

        public RelayCommand DeleteSelectedShapesCommand { get; }

        #endregion

        #region Constructor
        public MainViewModel(Model.Scumle scumle) : base(scumle)
        {
            ShapeViewModel uml1 = new UMLClassViewModel(new Shape(50, 50, "My frist shape"));
            ShapeViewModel uml2 = new UMLClassViewModel(new Shape(100, 100, "My second shape"));
            Shapes = new ObservableCollection<ShapeViewModel>() { uml1, uml2 };

            ConnectionPointViewModel cp1 = uml1.ConnectionPoints.ElementAt(0);
            ConnectionPointViewModel cp2 = uml2.ConnectionPoints.ElementAt(1);

            Lines.Add(new LineViewModel(cp1, cp2));


            AddShapeCommand = new RelayCommand(AddShape);
            ChangeZoomCommand = new RelayCommand<string>(ChangeZoom);
            SaveWorkspaceCommand = new RelayCommand(SaveWorkspace);
            OpenWorkspaceCommand = new RelayCommand(OpenWorkspace);
            DeleteSelectedShapesCommand = new RelayCommand(DeleteSelectedShapes, HasSelectedShapes);


        }
        #endregion

        #region Methods
        internal void SelectShape(ShapeViewModel shape)
        {
            DeselectAllShapes();
            Selected.Add(shape);
            shape.IsSelected = true;
            DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
        }

        internal void DeselectAllShapes()
        {
            foreach (ShapeViewModel shape in Selected)
            {
                shape.IsSelected = false;
            }
            Selected.Clear();
        }

        public void AddShape()
        {
            Shapes.Add(new ShapeViewModel(new Shape(50, 50, "My shape " + _num++)));
        }

        public void ChangeZoom(string value)
        {   
            Zoom = Double.Parse(value);
        }

        public void DeleteSelectedShapes()
        {
            foreach (ShapeViewModel shape in Selected)
            {
                Shapes.Remove(shape);
            }
            Selected.Clear();
            DeleteSelectedShapesCommand.RaiseCanExecuteChanged();
        }

        public bool HasSelectedShapes()
        {
            return Selected.Count > 0;
        }

        public void SaveWorkspace()
        {            
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".scumle";
            save.Filter = "(.scumle)|*.scumle";
            if (save.ShowDialog() == true)
                GenericSerializer.convertToXML<ObservableCollection<ShapeViewModel>>(Shapes, Path.GetFullPath(save.FileName));
               
        }

        public void OpenWorkspace()
        {

            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".scumle";
            open.Filter = "(.scumle)|*.scumle";
            ObservableCollection<ShapeViewModel> loadedShapes = new ObservableCollection<ShapeViewModel>();



            // Show open file dialog box
            Nullable<bool> result = open.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                loadedShapes = GenericSerializer.convertFromXML<ObservableCollection<ShapeViewModel>>(Path.GetFullPath(open.FileName));
                Shapes.Clear();
                foreach(var item in loadedShapes)
                {
                    Shapes.Add(item);
                }
               
            }
        }
        #endregion
    }

}
