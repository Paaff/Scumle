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
    public class LineTemplateSelector : DictionaryTemplateSelector<LineViewModel, ELine>
    {
        /// <summary>
        /// The dictionary strictly below may be populated with line types and
        /// data template names. These will then be looked up dynamically at runtime.
        /// Make sure that each template names exists in LinesDictionary.xaml
        /// </summary>
        public LineTemplateSelector() : base()
        {
            Templates = new Dictionary<ELine, string>()
            {
                { ELine.Association, "LineAssociation" },
                { ELine.Inheritance, "LineInheritance" },
                { ELine.Relational, "LineRelational" }
            };
        }

        public override ELine templateProperty(LineViewModel Line)
        {
            return Line.Type;
        }
    }
}
