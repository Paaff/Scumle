using System;

namespace Scumle.Model
{   
    public class Line
    {
        public Line(ConnectionPoint from, ConnectionPoint to)
        {
            From = from;
            To = to;
        }

        public ConnectionPoint From { get; set; }
        public ConnectionPoint To { get; set; }
    }
}
