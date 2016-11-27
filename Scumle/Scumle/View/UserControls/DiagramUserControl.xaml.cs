using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scumle.View.UserControls
{
    /// <summary>
    /// Interaction logic for DiagramUserControl.xaml
    /// </summary>
    public partial class DiagramUserControl : UserControl
    {
        public DiagramUserControl()
        {
            InitializeComponent();
        }

        private MainViewModel Main
        {
            get { return DataContext as MainViewModel; }
        }
        
        private void SelectShape(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            IShape shape = elem.DataContext as IShape;
            bool clearSelection = true;
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                clearSelection = false;
            }
            Main.SelectShape(shape, clearSelection);
        }
        private void Zoom(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                Main.ZoomEvent(e);
            }
        }
    }
}
