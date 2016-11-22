using System;
using System.Xml.Serialization;

namespace Scumle.Model
{
    [XmlInclude(typeof(ConnectionPoint))]
    public class Line : ModelBase
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
    }
}
