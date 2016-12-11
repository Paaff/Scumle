using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Scumle.Model
{
    /// <summary>
    /// Interface for implementing any sort of line in the editor
    /// </summary>
    public interface ILine
    {
        IPoint From { get; set; }
        IPoint To { get; set; }
        ELine Type { get; set; }
        double Angle { get; }
        string StoreToId { get; set; }
        string StoreFromId { get; set; }
    }
}
