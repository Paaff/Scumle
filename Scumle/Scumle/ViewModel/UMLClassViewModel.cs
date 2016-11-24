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

namespace Scumle.ViewModel
{
    public class UMLClassViewModel : ShapeViewModel
    {

        private ObservableCollection<string> _fields = new ObservableCollection<string>();
        private ObservableCollection<string> _methods = new ObservableCollection<string>();

        private ICommand addFieldCommand;
        private ICommand addMethodCommand;

        #region Constructor
        // This Constructor should be taking an UMLClass and not a Shape yes?
        public UMLClassViewModel(UMLClass shape) : base(shape)
        {
            Width = 300;
            Height = 150;
            ShapeColor = new SolidColorBrush(Color.FromRgb(232, 232, 232));

      

        }

        private void addField(string field)
        {
            _fields.Add(field);
        }
        #endregion



    }
}
