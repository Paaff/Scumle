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
    public class UMLClassViewModel : ShapeViewModel
    {

        public ObservableCollection<string> fields;
        public ObservableCollection<string> methods;

        public ICommand removeFieldCommand => new RelayCommand<string>(removeField);
        public ICommand removeMethodCommand => new RelayCommand<string>(removeMethod);
        public ICommand addFieldCommand => new RelayCommand(addField);
        public ICommand addMethodCommand => new RelayCommand(addMethod);

        #region Constructor
        // This Constructor should be taking an UMLClass and not a Shape yes?
        public UMLClassViewModel(UMLClass uml) : base(uml)
        {
            Width = 300;
            Height = 150;
            ShapeColor = new SolidColorBrush(Color.FromRgb(232, 232, 232));

            fields = new ObservableCollection<string>(uml.umlFields);
            methods = new ObservableCollection<string>(uml.umlMethods);


        }
        #endregion

        #region Properties

      
        public ObservableCollection<string> UMLFields
        {
            get { return fields; }
            set { }
        }

        public ObservableCollection<string> UMLMethods
        {
            get { return methods; }
            set { }
        }
        #endregion

        private void removeField(string field)
        {
            UMLFields.Remove(field);
           
        }
        
        private void removeMethod(string method)
        {
            UMLMethods.Remove(method);
            
        }

        private void addField()
        {
            UMLFields.Add("New Field .. ");
        }

        private void addMethod()
        {
            UMLMethods.Add("New Method .. ");
        }
      


    }
}
