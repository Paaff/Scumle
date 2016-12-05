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
    public interface IPoint
    {
        IShape Shape { get; }
        double CenterX { get; }
        double CenterY { get; }
        HorizontalAlignment Horizontal { get; } // TODO: This should not be neccesary
        VerticalAlignment Vertical { get; } // TODO: This should not be neccesary
        string AttachedID { get; }
    }
}