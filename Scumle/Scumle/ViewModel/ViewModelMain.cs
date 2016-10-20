using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    class ViewModelMain : ViewModelBase<Model.Scumle>
    {
        public ObservableCollection<ShapeViewModel> Shapes { get; }

        public String Version { get; } = "Version 1.0.0";

        public ViewModelMain(Model.Scumle scumle) : base(scumle)
        {
            Shapes = new ObservableCollection<ShapeViewModel>()
            {
                new ShapeViewModel(new Shape(10, 10, "My frist shape")),
                new ShapeViewModel(new Shape(100, 100, "My second shape"))
            };
        }

    }

}
