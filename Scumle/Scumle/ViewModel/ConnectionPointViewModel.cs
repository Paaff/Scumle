using GalaSoft.MvvmLight.CommandWpf;
using Scumle.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Scumle.ViewModel
{
    public class ConnectionPointViewModel : ViewModelBase<IPoint>, IPoint
    {
        public ConnectionPointViewModel(IPoint model, IShape shape) : base(model)
        {
            /*
            if (shape.Shape != model.Shape)
            {
                throw new ArgumentException("The shapeviewmodel does not wrap the shape for the connection point");
            }
            */
            Shape = shape;
            ShapeColor = new SolidColorBrush(Color.FromRgb(47, 79, 79));
        }

        private Brush _shapeColor;
        public IShape Shape { get; set; }

        public Brush ShapeColor
        {
            get
            {
                return _shapeColor;
            }

            set
            {
                _shapeColor = value;
                OnPropertyChanged();
            }
        }

        public double CenterX
        {
            get { return Model.CenterX; }
        }

        public double CenterY
        {
            get { return Model.CenterY; }
        }

        public HorizontalAlignment Horizontal
        {
            get { return Model.Horizontal; }
        }

        public VerticalAlignment Vertical
        {
            get { return Model.Vertical; }
        }

        public void PropertyChange()
        {
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
        }

    }
}
