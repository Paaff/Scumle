using Scumle.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    class LineViewModel : ViewModelBase<Line>
    {
        private ConnectionPointViewModel _from;
        private ConnectionPointViewModel _to;

        public LineViewModel(Line line, ConnectionPointViewModel from, ConnectionPointViewModel to) : base(line)
        {
            _from = from;
            _to = to;

            // Subscribe to property chnaged event from connection points
            From.PropertyChanged += new PropertyChangedEventHandler(UpdateProperties);
            To.PropertyChanged += new PropertyChangedEventHandler(UpdateProperties);
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

        public double Angle
        {
            get { return Model.Angle; }
        }

        public void UpdateProperties(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Angle));
        }
        #endregion
    }
}
