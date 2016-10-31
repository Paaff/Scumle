using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Scumle.ViewModel
{

    class ShapeViewModel : ViewModelBase<Shape>
    {
        private bool _isSelected = false;

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

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        internal void Select()
        {
            throw new NotImplementedException();
        }
        #endregion

        internal void ShapeMove(double dX, double dY)
        {
            Model.MoveDelta(dX, dY);
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }

        internal void ShapeResize(double dX, double dY)
        {
            Model.Resize(dX, dY);
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
        }
    }
}
