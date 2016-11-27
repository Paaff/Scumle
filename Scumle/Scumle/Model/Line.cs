using Scumle.ViewModel;
using System;
using System.Xml.Serialization;

namespace Scumle.Model
{
    public class Line : ModelBase, ILine
    {   
 
        public Line(ELine type, IPoint from, IPoint to)
        {
            From = from;
            To = to;
            Type = type;

            // For Serialization
            storeTo = (to as ConnectionPointViewModel).Model as ConnectionPoint;
            storeFrom = (from as ConnectionPointViewModel).Model as ConnectionPoint;

                      
        }

        // For Serialization
        public Line() { }
        [XmlIgnore]
        public IPoint From { get; set; }
        [XmlIgnore]
        public IPoint To { get; set; }
        public ELine Type { get; set; }

        public ConnectionPoint storeTo { get; set; }
        public ConnectionPoint storeFrom { get; set; }

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
