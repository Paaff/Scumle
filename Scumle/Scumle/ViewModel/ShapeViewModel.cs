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

        public string Name
        {
            get { return Model.Name; }
            set { SetValue(value); }
        }
        #endregion

        internal void MoveDelta(object sender, DragDeltaEventArgs e)
        {
            Model.MoveDelta(e.HorizontalChange, e.VerticalChange);
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
        }
    }
}
