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
        /// <summary>
        /// The dictionary strictly below may be populated with line types and
        /// data template names. These will then be looked up dynamically at runtime.
        /// Make sure that each template names exists in LinesDictionary.xaml
        /// </summary>
        Dictionary<ELine, String> templates = new Dictionary<ELine, string>()
        {
            {ELine.Association, "LineAssociation"},
            {ELine.Inheritance, "LineInheritance"}
        };

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (item is LineViewModel)
            {
                LineViewModel line = item as LineViewModel;
                string templateName;

                if (!templates.TryGetValue(line.Type, out templateName)) {
                    return base.SelectTemplate(item, container);
                }

                return element.FindResource(templateName) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
