using System;
using System.Xml.Serialization;

namespace Scumle.Model
{
    [XmlInclude(typeof(ConnectionPoint))]
    public abstract class Line : ModelBase
    {
        public Line(ConnectionPoint from, ConnectionPoint to)
        {
            From = from;
            To = to;
        }

        // For Serialization
        public Line() { }

        public ConnectionPoint From { get; set; }
        public ConnectionPoint To { get; set; }
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
