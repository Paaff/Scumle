using GalaSoft.MvvmLight.Command;
using Scumle.Model;
using Scumle.Model.Shapes;
using Scumle.UndeRedo.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Scumle.ViewModel
{

    public abstract class ShapeViewModel<T> : ViewModelBase<T>, IShapeViewModel where T : Shape
    {
        #region fields
        private bool _isSelected = false;
        private bool isResizing = false;
        private System.Windows.Point oldPos;
        private System.Windows.Point newPos;
        private System.Windows.Size oldSize;
        private System.Windows.Size newSize;
        private Brush _shapeColor;

        public ICommand ShapeMoveCommand => new RelayCommand<DragDeltaEventArgs>(ShapeMoveEvent);
        public ICommand MoveStartedCommand => new RelayCommand(MoveStartedEvent);
        public ICommand MoveCompletedCommand => new RelayCommand(MoveCompletedEvent);
        public ICommand ResizeStartedCommand => new RelayCommand(ResizeStartedEvent);
        public ICommand ResizeCompletedCommand => new RelayCommand<DragCompletedEventArgs>(ResizeCompletedEvent);
        #endregion

        public ShapeViewModel(T shape) : base(shape)
        {
            AddInitalConnectionPoints();
        }

        #region Private methods
        private void ShapeMoveEvent(DragDeltaEventArgs e)
        {
            if (isResizing || MainViewModel._tool == ETool.LineTool) return;
            ShapeMove(e.HorizontalChange, e.VerticalChange);
        } 

        private void MoveStartedEvent()
        {
            oldPos = new System.Windows.Point(X, Y);
        }

        private void MoveCompletedEvent()
        {
            newPos = new System.Windows.Point(X, Y);
            if (!oldPos.Equals(newPos))
            {
                new ShapeMoveCommand(this, oldPos, newPos).Execute();
            }
        }

        private void ResizeStartedEvent()
        {
            isResizing = true;
            oldPos = new System.Windows.Point(X, Y);
            oldSize = new System.Windows.Size(Width, Height);
        }

        private void ResizeCompletedEvent(DragCompletedEventArgs e)
        {
            e.Handled = true;
            isResizing = false;
            newPos = new System.Windows.Point(X, Y);
            newSize = new System.Windows.Size(Width, Height);
            if (!oldSize.Equals(newSize))
            {
                new ShapeResizeCommand(this, oldPos, newPos, oldSize, newSize).Execute();
            }
        }

        private void AddInitalConnectionPoints()
        {
            ConnectionPoints = new ObservableCollection<ConnectionPointViewModel>(
                Model.ConnectionPoints.Select(c => new ConnectionPointViewModel(c, this)));
        }
        #endregion

        #region Properties
        public ObservableCollection<ConnectionPointViewModel> ConnectionPoints { get; private set; }

        public double X
        {
            get { return Model.X; }
            set { SetValue(value); UpdateConnectionPoints(); }
        }

        public double Y
        {
            get { return Model.Y; }
            set { SetValue(value); UpdateConnectionPoints(); }
        }

        public double Width
        {
            get { return Model.Width; }
            set { SetValue(value); UpdateConnectionPoints(); }
        }

        public double Height
        {
            get { return Model.Height; }
            set { SetValue(value); UpdateConnectionPoints(); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public Brush ShapeColor
        {
            get { return _shapeColor; }
            set { _shapeColor = value; OnPropertyChanged(); }
        }

        public Shape Shape
        {
            get { return Model as Shape; }
        }
        #endregion

        internal void ShapeMove(double dX, double dY)
        {
            Model.MoveDelta(dX, dY);
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
            UpdateConnectionPoints();
        }

        internal void ShapeResize(double dX, double dY)
        {
            Model.Resize(dX, dY);
            OnPropertyChanged(nameof(Width));
            OnPropertyChanged(nameof(Height));
            UpdateConnectionPoints();
        }

        internal void UpdateConnectionPoints()
        {
            foreach (ConnectionPointViewModel c in ConnectionPoints)
            {
                c.PropertyChange();
            }
        }

    }
}
