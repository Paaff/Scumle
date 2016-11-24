using Scumle.ViewModel;
using Scumle.ViewModel.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Scumle.Tools
{
    class ShapeTemplateSelector : DataTemplateSelector
    {
        Dictionary<Type, DataTemplateSelector> TemplateSelectors = new Dictionary<Type, DataTemplateSelector>() {
            { typeof(BasicShapeViewModel), new BasicShapeTemplateSelector() }
        };

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is IShapeViewModel)
            {
                IShapeViewModel shape = item as IShapeViewModel;
                DataTemplateSelector templateSelector;

                if (!TemplateSelectors.TryGetValue(shape.GetType(), out templateSelector))
                {
                    return base.SelectTemplate(item, container);
                }

                return templateSelector.SelectTemplate(shape, container);
            }

            return base.SelectTemplate(item, container);
        }
    }
}
