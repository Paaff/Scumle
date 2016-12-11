using Scumle.ViewModel;
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
    /// Interface for implementing any sort of point in the program.
    /// This is used for connection points for which the lines can bind to
    /// </summary>
    public interface IPoint
    {
        IShape Shape { get; }
        double CenterX { get; }
        double CenterY { get; }
        HorizontalAlignment Horizontal { get; }
        VerticalAlignment Vertical { get; }
        string AttachedID { get; }
    }
}