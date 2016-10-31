using Scumle.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    class LineViewModel : ViewModelBase<Line>
    {
        private ConnectionPointViewModel _from;
        private ConnectionPointViewModel _to;

        public LineViewModel(ConnectionPointViewModel from, ConnectionPointViewModel to) : base(new Line(from.Model, to.Model))
        {
            _from = from;
            _to = to;
        }

        #region Properties
        public ConnectionPointViewModel From
        {
            get { return _from; }
            set
            {
                _from = value;
                Model.From = value.Model;
                OnPropertyChanged();
            }
        }

        public ConnectionPointViewModel To
        {
            get { return _to; }
            set
            {
                _to = value;
                Model.To = value.Model;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
