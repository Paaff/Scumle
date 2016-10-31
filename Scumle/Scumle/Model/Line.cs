using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model
{
    class Line
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
