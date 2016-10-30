using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Scumle.ViewModel
{

    class ShapeViewModel : ViewModelBase<Shape>
    {

        public ShapeViewModel(Shape shape) : base(shape)
        {
        }

        #region Properties
        public double X
        {
            get { return Model.X; }
            set { SetValue(value); }
        }

        public double Y
        {
            get { return Model.Y; }
            set { SetValue(value); }
        }

        public double Width
        {
            get { return Model.Width; }
            set { SetValue(value); }
        }

        public double Height
        {
            get { return Model.Height; }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return Model.Name; }
            set { SetValue(value); }
        }
        #endregion

        internal void MoveDelta(double dX, double dY)
        {
            Model.MoveDelta(dX, dY);
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        #region Resizing
        internal void ShapeResize(double dX, double dY)
        {
            Model.Resize(dX, dY);
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
        }

        internal void ShapeResizeHorizontal(double dX)
        {
            Model.Resize(dX, 0);
            OnPropertyChanged(nameof(Width));
        }

        internal void ShapeMoveHorizontal(double dX)
        {
            Model.MoveDelta(dX, 0);
            OnPropertyChanged(nameof(X));
        }

        internal void ShapeResizeVertical(double dY)
        {
            Model.Resize(0, dY);
            OnPropertyChanged(nameof(Height));
        }

        internal void ShapeMoveVertical(double dX)
        {
            Model.MoveDelta(0, dX);
            OnPropertyChanged(nameof(Y));
        }
        #endregion
    }
}
