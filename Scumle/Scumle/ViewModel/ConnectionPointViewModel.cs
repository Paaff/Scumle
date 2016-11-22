using GalaSoft.MvvmLight.CommandWpf;
using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Scumle.ViewModel
{
    public class ConnectionPointViewModel : ViewModelBase<ConnectionPoint>
    {
        public ConnectionPointViewModel(ConnectionPoint model, ShapeViewModel shape) : base(model)
        {
            if (shape.Model != model.Shape)
            {
                throw new ArgumentException("The shapeviewmodel does not wrap the shape for the connection point");
            }
            Shape = shape;
        }

        public ShapeViewModel Shape { get; set; }

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
