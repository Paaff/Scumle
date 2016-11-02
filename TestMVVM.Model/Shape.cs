using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace TestMVVM.Model
{
    public abstract class Shape : ViewModelBase
    {
        private double _x;
        private double _y;

        public ICommand MoveShapeCommand { get; }

        public Shape(double x, double y)
        {
            X = x;
            Y = y;
            MoveShapeCommand = new RelayCommand<DragDeltaEventArgs>(MoveShape);

        }

        public void MoveShape(DragDeltaEventArgs e)
        {
            X += e.HorizontalChange;
            Y += e.VerticalChange;
        }

        public double X
        {
          get { return _x; }
          set { Set(ref _x, value); }
        }

        public double Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }
    }
}
