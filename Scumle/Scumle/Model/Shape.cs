using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Scumle.Model
{
    class Shape : INotifyPropertyChanged
    {

        double _X;
        double _Y;

        public Shape(double X, double Y, String Name)
        {
            this.X = X;
            this.Y = Y;
            this.Width = 25;
            this.Height = 25;
            this.Name = Name;
        }

        public double X
        {
            get { return _X; }
            set
            {
                _X = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(X)));
            }
        }
        public double Y
        {
            get { return _Y; }
            set { _Y = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Y)));
            }
        }

        public double Width { get; set; }
        public double Height { get; set; }
        public String Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void MoveDelta(object sender, DragDeltaEventArgs e)
        {
            MoveDelta(e.HorizontalChange, e.VerticalChange);
        }

        public void MoveDelta(double X, double Y)
        {
            this.X += X;
            this.Y += Y;
        }

    }
}
