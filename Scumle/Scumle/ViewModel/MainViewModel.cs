using Scumle.Helpers;
using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Scumle.ViewModel
{
    class MainViewModel : ViewModelBase<Model.Scumle>
    {

        private int Num = 0;
        private double _zoom = 1.0;

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

        public ObservableCollection<ShapeViewModel> Selected { get; }

        public String Version { get; } = "Version 1.0.0";

        public MainViewModel(Model.Scumle scumle) : base(scumle)
        {
            Shapes = new ObservableCollection<ShapeViewModel>()
            {
                new UMLClassViewModel(new Shape(50, 50, "My frist shape")),
                new UMLClassViewModel(new Shape(100, 100, "My second shape"))
            };

            AddShapeCommand = new RelayCommand(AddShape);
            ChangeZoomCommand = new RelayCommand<string>(ChangeZoom);

        }

        public RelayCommand<string> ChangeZoomCommand { get; set; }

        public RelayCommand AddShapeCommand { get; private set; }

        public void AddShape()
        {
            Shapes.Add(new ShapeViewModel(new Shape(50, 50, "My shape " + Num++)));
        }

        public void ChangeZoom(string value)
        {   
            Zoom = Double.Parse(value);
        }

    }

}
