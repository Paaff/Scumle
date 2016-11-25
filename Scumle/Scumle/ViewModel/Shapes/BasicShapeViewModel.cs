using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;
using Scumle.Model.Shapes;

namespace Scumle.ViewModel.Shapes
{
    public class BasicShapeViewModel : ShapeViewModel<BasicShape>
    {
        public BasicShapeViewModel(BasicShape shape) : base(shape)
        {
        }

        public EBasicShape Type
        {
            get { return Model.Type; }
            set { SetValue(value); }
        }
    }
}
