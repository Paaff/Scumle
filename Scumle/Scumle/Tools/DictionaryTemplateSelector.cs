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
    public abstract class DictionaryTemplateSelector<TModel,TProperty> : DataTemplateSelector where TModel : class
    {
        /// <summary>
        /// The dictionary strictly below may be populated with types of TProperty and
        /// data template names. These will then be looked up dynamically at runtime.
        /// Make sure that each template names exists in LinesDictionary.xaml
        /// </summary>
        protected Dictionary<TProperty, String> Templates { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (item is TModel)
            {
                TModel line = item as TModel;
                string templateName;

                if (!Templates.TryGetValue(templateProperty(line), out templateName))
                {
                    return base.SelectTemplate(item, container);
                }

                return element.FindResource(templateName) as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        public abstract TProperty templateProperty(TModel Model);
        
    }
}
