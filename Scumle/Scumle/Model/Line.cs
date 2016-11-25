using System;
using System.Xml.Serialization;

namespace Scumle.Model
{
    [XmlInclude(typeof(ConnectionPoint))]
    public class Line : ModelBase, ILine
    {
        public Line(ELine type, IPoint from, IPoint to)
        {
            From = from;
            To = to;
            Type = type;
        }

        // For Serialization
        public Line() { }

        public IPoint From { get; set; }
        public IPoint To { get; set; }
        public ELine Type { get; set; }
        public double Angle
        {
            get { return calculateAngle(); }
        }

        private double calculateAngle()
        {
            double dX = To.CenterX - From.CenterX;
            double dY = To.CenterY - From.CenterY;

            return Math.Atan2(dY, dX) * 180.0 / Math.PI + 90;
        }
    }
}
