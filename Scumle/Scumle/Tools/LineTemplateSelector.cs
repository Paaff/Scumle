using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Scumle.Tools
{
    public class LineTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (item is LineViewModel)
            {
                LineViewModel line = item as LineViewModel;

                switch (line.Type)
                {
                    case ELine.Association:
                        return element.FindResource("LineAssociation") as DataTemplate;
                    case ELine.Inheritance:
                        return element.FindResource("LineInheritance") as DataTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
