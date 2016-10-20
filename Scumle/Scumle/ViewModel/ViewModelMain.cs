using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    class ViewModelMain
    {
        public ObservableCollection<Shape> Shapes { get; }

        public String Version { get; }

        public ViewModelMain()
        {
            Version = "Version 1.0.0";

            Shapes = new ObservableCollection<Shape>()
            {
                new Shape(20, 20, "My first shape"),
                new Shape(100, 100, "My second shape")
            };
        }

    }

}
