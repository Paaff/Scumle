using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace Scumle.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            this.DataContext = new MainViewModel(new Model.Scumle());
            InitializeComponent();
        }

        private MainViewModel Main
        {
            get { return DataContext as MainViewModel; }
        }

        private void SelectShape(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement elem = sender as FrameworkElement;
            ShapeViewModel shape = elem.DataContext as ShapeViewModel;
            bool clearSelection = true;
            if (Keyboard.IsKeyDown(Key.LeftShift))
            {
                clearSelection = false;
            }
            Main.SelectShape(shape,clearSelection);
        }
    }

}
