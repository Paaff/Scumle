using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    public class UMLItemsViewModel : ViewModelBase<UMLItem>
    {
        public UMLItemsViewModel(UMLItem umlitem) : base(umlitem)
        {
                
        }

        public string ListValue
        {
            get { return Model.ListValue; }
            set { SetValue(value); }
        }
    }
}
