using Scumle.Model;
using Scumle.ViewModel.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.Tools
{
    class BasicShapeTemplateSelector : DictionaryTemplateSelector<BasicShapeViewModel, EBasicShape>
    {
        public BasicShapeTemplateSelector() : base()
        {
            Templates = new Dictionary<EBasicShape, string>
            {
                { EBasicShape.Ellipse, "EllipseTemplate" },
                { EBasicShape.Rectangle, "RectangleTemplate" }
            };
        }

        public override EBasicShape templateProperty(BasicShapeViewModel Shape)
        {
            return Shape.Type;
        }
    }
}
