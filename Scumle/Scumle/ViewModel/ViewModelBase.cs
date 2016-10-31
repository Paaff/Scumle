using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scumle.ViewModel
{
    [XmlInclude(typeof(UMLClassViewModel))]
    public class ViewModelBase<T> : INotifyPropertyChanged
    {
        [XmlIgnoreAttribute]
        public T Model { get; private set; }

        public ViewModelBase(T model)
        {
            Model = model;
        }

        // For XML serialization 
        public ViewModelBase() { }

        protected void SetValue<Value>(Value value, [CallerMemberName] string propertyName = null)
        {
            
            if(Model != null) { 
            var propertyInfo = Model.GetType().GetProperty(propertyName);
            var currentValue = propertyInfo.GetValue(Model);
            if (!Equals(currentValue, value))
            {
                propertyInfo.SetValue(Model, value);
                OnPropertyChanged(propertyName);
            }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
