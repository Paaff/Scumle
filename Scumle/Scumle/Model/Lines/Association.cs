using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Model.Lines
{
    class Association : Line
    {
        public Association(ConnectionPoint from, ConnectionPoint to) : base(from, to) { }
    }
}
