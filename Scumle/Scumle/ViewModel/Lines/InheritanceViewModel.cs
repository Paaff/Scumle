using Scumle.Model.Lines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel.Lines
{
    class InheritanceViewModel : LineViewModel
    {
        public InheritanceViewModel(ConnectionPointViewModel from, ConnectionPointViewModel to) 
            : base(new Inheritance(from.Model, to.Model), from, to) { }
    }
}
