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
        #region Constructor  
        public UMLClassViewModel(UMLClass uml) : base(uml)
        {
            Width = 300;
            Height = 150;
            ShapeColor = new SolidColorBrush(Color.FromRgb(205, 92, 92));
        }
        #endregion

        #region Properties

     

        public string Name
        {
            get { return Model.Name; }
            set { SetValue(value); }
        }
        public string UMLFields
        {
            get { return Model.UMLFields; }
            set { SetValue(value); }
        }
        public string UMLMethods
        {
            get { return Model.UMLMethods; }
            set { SetValue(value); }
        }



        #endregion

    }
}
