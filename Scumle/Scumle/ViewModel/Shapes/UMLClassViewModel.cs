using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scumle.Model;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Scumle.Model.Shapes;
using System.Windows;

namespace Scumle.ViewModel
{
    public class UMLClassViewModel : ShapeViewModel<UMLClass>
    {

        #region Fields
        public ObservableCollection<UMLItem> fields;
        public ObservableCollection<UMLItem> methods;
        #endregion

        #region Commands

        public ICommand removeFieldCommand => new RelayCommand<UMLItem>(removeField);
        public ICommand removeMethodCommand => new RelayCommand<UMLItem>(removeMethod);
        public ICommand addFieldCommand => new RelayCommand(addField);
        public ICommand addMethodCommand => new RelayCommand(addMethod);
 
        #endregion

        #region Constructor  
        public UMLClassViewModel(UMLClass uml) : base(uml)
        {
            Width = 300;
            Height = 150;
            ShapeColor = new SolidColorBrush(Color.FromRgb(232, 232, 232));

            fields = new ObservableCollection<UMLItem>(Model.umlFields);
            methods = new ObservableCollection<UMLItem>(Model.umlMethods);
   


        }
        #endregion
            
        #region Properties    

        public string Name
        {
            get { return Model.Name; }
            set { SetValue(value); }
        }
        
      
        public ObservableCollection<UMLItem> UMLFields
        {
            get { return fields; }
            set { }
        }

        public ObservableCollection<UMLItem> UMLMethods
        {
            get { return methods; }
            set { }
        }


        #endregion

        #region Command Methods
        private void removeField(UMLItem field)
        {
            if (field != null)
            {
                UMLFields.Remove(field);
                Model.umlFields.Remove(field);
            }
            
        }
        
        private void removeMethod(UMLItem method)
        {
            if (method != null)
            {
                UMLMethods.Remove(method);
                Model.umlMethods.Remove(method);
            }
        }

        private void addField()
        {
            UMLFields.Add(new UMLItem("New Field .."));
            Model.umlFields.Add(new UMLItem("New Field .."));
        }

        private void addMethod()
        {
            UMLMethods.Add(new UMLItem("New Method .."));
            Model.umlMethods.Add(new UMLItem("New Method .."));
        }

        #endregion



    }
}
