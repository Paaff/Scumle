using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ShapeUserControl.xaml
    /// </summary>
    public partial class ShapeUserControl : Thumb
    {
        private bool IsResizing = false;
        public ShapeUserControl()
        {
            InitializeComponent();
        }

        private void ShapeMove(object sender, DragDeltaEventArgs e)
        {
            if (IsResizing) return;
            ShapeViewModel shape = DataContext as ShapeViewModel;
            shape.MoveDelta(sender, e);
        }

        private void ShapeResize(object sender, DragDeltaEventArgs e)
        {
            ShapeViewModel shape = DataContext as ShapeViewModel;
            shape.ShapeResize(sender, e);
        }

        private void StartResize(object sender, DragStartedEventArgs e)
        {
            IsResizing = true;
        }

        private void EndResize(object sender, DragCompletedEventArgs e)
        {
            IsResizing = false;
        }
    }
}
