using Scumle.Model;
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

        private IShape Shape
        {
            get { return DataContext as IShape; }
        }

        private void ShapeMove(object sender, DragDeltaEventArgs e)
        {
            if (IsResizing) return;
            Shape.ShapeMove(e.HorizontalChange, e.VerticalChange);
        }

        #region Shape Resize Corners
        private void ShapeResizeSE(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(e.HorizontalChange, e.VerticalChange);
        }

        private void ShapeResizeSW(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(-e.HorizontalChange, e.VerticalChange);
            Shape.ShapeMove(e.HorizontalChange, 0);
        }

        private void ShapeResizeNE(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(e.HorizontalChange, -e.VerticalChange);
            Shape.ShapeMove(0, e.VerticalChange);
        }

        private void ShapeResizeNW(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(-e.HorizontalChange, -e.VerticalChange);
            Shape.ShapeMove(e.HorizontalChange, e.VerticalChange);
        }
        #endregion

        #region Shape Resize Sides
        private void ShapeResizeE(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(e.HorizontalChange, 0);
        }
        private void ShapeResizeS(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(0, e.VerticalChange);
        }

        private void ShapeResizeW(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(-e.HorizontalChange, 0);
            Shape.ShapeMove(e.HorizontalChange, 0);
        }

        private void ShapeResizeN(object sender, DragDeltaEventArgs e)
        {
            e.Handled = true;
            Shape.ShapeResize(0, -e.VerticalChange);
            Shape.ShapeMove(0, e.VerticalChange);
        }
        #endregion

        private void StartResize(object sender, DragStartedEventArgs e)
        {
            IsResizing = true;
        }

        private void EndResize(object sender, DragCompletedEventArgs e)
        {
            IsResizing = false;
        }

        private void CopyShape(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Copy shape!");
        }
    }
}
