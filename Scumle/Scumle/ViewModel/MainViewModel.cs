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
        private int Num = 0;
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

        public ObservableCollection<ShapeViewModel> Shapes { get; }

        public ObservableCollection<ShapeViewModel> Selected { get; } = new ObservableCollection<ShapeViewModel>();

        public String Version { get; } = "Version 1.0.0";

        internal void SelectShape(ShapeViewModel shape)
        {
            DeselectAllShapes();
            Selected.Add(shape);
            shape.IsSelected = true;
        }

        internal void DeselectAllShapes()
        {
            foreach (ShapeViewModel shape in Selected)
            {
                shape.IsSelected = false;
            }
            Selected.Clear();
        }
        #endregion

        #region Commands

        public RelayCommand<string> ChangeZoomCommand { get; set; }

        public RelayCommand AddShapeCommand { get; private set; }

        public RelayCommand SaveWorkspaceCommand { get; }

        public RelayCommand OpenWorkspaceCommand { get; }

        #endregion

        #region Constructor
        public MainViewModel(Model.Scumle scumle) : base(scumle)
        {
            Shapes = new ObservableCollection<ShapeViewModel>()
            {
                new UMLClassViewModel(new Shape(50, 50, "My frist shape")),
                new UMLClassViewModel(new Shape(100, 100, "My second shape"))
            };

            AddShapeCommand = new RelayCommand(AddShape);
            ChangeZoomCommand = new RelayCommand<string>(ChangeZoom);
            SaveWorkspaceCommand = new RelayCommand(SaveWorkspace);
            OpenWorkspaceCommand = new RelayCommand(OpenWorkspace);

          
          

        }
        #endregion
       
        #region Methods
               
        public void AddShape()
        {
            Shapes.Add(new ShapeViewModel(new Shape(50, 50, "My shape " + Num++)));
        }

        public void ChangeZoom(string value)
        {   
            Zoom = Double.Parse(value);
        }

        public void SaveWorkspace()
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".scumle";
            save.Filter = "(.scumle)|*.scumle";
            if (save.ShowDialog() == true)
                File.WriteAllText(save.FileName, "Testing");
               
        }

        public void OpenWorkspace()
        {

            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".scumle";
            open.Filter = "(.scumle)|*.scumle";
          
            // Testing:
            
            // Show open file dialog box
            Nullable<bool> result = open.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // 
                string filename = open.FileName;
            }
        }
        #endregion
    }

}
