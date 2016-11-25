using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.View.Preview
{
    public class PreviewConnectionPoint : ConnectionPoint
    {
        public PreviewConnectionPoint(Shape shape, double X, double Y) : base(shape, System.Windows.HorizontalAlignment.Center, System.Windows.VerticalAlignment.Center)
        {
            CenterX = X;
            CenterY = Y;
        }

        public new double CenterX { get; set; }

        public new double CenterY { get; set; }
    }

}
