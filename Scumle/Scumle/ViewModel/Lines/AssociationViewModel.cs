using Scumle.Model.Lines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel.Lines
{
    class AssociationViewModel : LineViewModel
    {
        public AssociationViewModel(ConnectionPointViewModel from, ConnectionPointViewModel to) 
            : base(new Association(from.Model, to.Model), from, to) { }
    }
}
