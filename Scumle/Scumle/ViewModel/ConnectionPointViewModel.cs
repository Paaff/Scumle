using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Scumle.ViewModel
{
    public class ConnectionPointViewModel : ViewModelBase<ConnectionPoint>
    {
        private ShapeViewModel _shape;
        private HorizontalAlignment _horizontal = HorizontalAlignment.Center;
        private VerticalAlignment _vertical = VerticalAlignment.Center;

        public ConnectionPointViewModel(ShapeViewModel shape) : base(new ConnectionPoint())
        {
            Shape = shape;
        }

        public ShapeViewModel Shape
        {
            get { return _shape; }
            set
            {
                _shape = value;
                OnPropertyChanged();
            }
        }

        public HorizontalAlignment Horizontal
        {
            get { return _horizontal; }
            set
            {
                _horizontal = value;
                OnPropertyChanged();
            }
        }

        public VerticalAlignment Vertical
        {
            get { return _vertical; }
            set
            {
                _vertical = value;
                OnPropertyChanged();
            }
        }
        

    }
}
