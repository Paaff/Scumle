using Scumle.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scumle.ViewModel
{
    public class LineViewModel : ViewModelBase<ILine>, ILine
    {
        public LineViewModel(ILine line) : base(line)
        {

            // Subscribe to property chnaged event from connection points
            SubscribeToChange(From as INotifyPropertyChanged);
            SubscribeToChange(To as INotifyPropertyChanged);
        }

        private void SubscribeToChange(INotifyPropertyChanged i)
        {
            if (i != null)
            {
                i.PropertyChanged += new PropertyChangedEventHandler(UpdateProperties);
            }
        }

        #region Properties
        public IPoint From
        {
            get { return Model.From; }
            set { SetValue(value); }
        }

        public IPoint To
        {
            get { return Model.To; }
            set { SetValue(value); }
        }

        public ELine Type
        {
            get { return Model.Type; }
            set { SetValue(value); }
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
