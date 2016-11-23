using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;
using Scumle.Model.Lines;

namespace Scumle.ViewModel.Lines
{
    class RealisationViewModel : LineViewModel
    {
        public RealisationViewModel(ConnectionPointViewModel from, ConnectionPointViewModel to) 
            : base(new Realisation(from.Model, to.Model), from, to) { }
    }
}
