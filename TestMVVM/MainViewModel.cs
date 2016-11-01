using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestMVVM.Model;
using TestMVVM.Model.Shapes;

namespace TestMVVM
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Shape> Shapes { get; set; }
        public RelayCommand MoveAllShapesCommand { get; }
        public RelayCommand AddShapeCommand { get; }

        public MainViewModel()
        {
            AddShapeCommand = new RelayCommand(AddShape);
            MoveAllShapesCommand = new RelayCommand(MoveAllShapes);
            Shapes = new ObservableCollection<Shape> { new Circle(50, 50), new Square(10, 10) };
        }

        private void MoveAllShapes()
        {
            // TODO: Theres is no OnPropertyChanged here!
            foreach (Shape shape in Shapes)
            {
                shape.X += 10;
                shape.Y += 10;
            }
        }

        private void AddShape()
        {
            Random random = new Random();
            Shapes.Add(new Circle(random.Next(0, 450), random.Next(0, 250)));
        }


    }
}
