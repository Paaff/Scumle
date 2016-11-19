using GalaSoft.MvvmLight.Command;
using Scumle.Model;
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

    public class ShapeViewModel : ViewModelBase<Shape>
    {
        private bool _isSelected = false;
        private bool isResizing = false;
        private Point oldPos;
        private Point newPos;
        private Size oldSize;
        private Size newSize;
        private Brush _shapeColor;
        private bool _ConnectionVisibilty = false;

        public ICommand ShapeMoveCommand => new RelayCommand<DragDeltaEventArgs>(ShapeMoveEvent);
        public ICommand MoveStartedCommand => new RelayCommand(MoveStartedEvent);
        public ICommand MoveCompletedCommand => new RelayCommand(MoveCompletedEvent);
        public ICommand ResizeStartedCommand => new RelayCommand(ResizeStartedEvent);
        public ICommand ResizeCompletedCommand => new RelayCommand<DragCompletedEventArgs>(ResizeCompletedEvent);

        public bool ConnectionVisibility
        {
            get
            {
                return _ConnectionVisibilty;
            }

            set
            {
                _ConnectionVisibilty = value;
                OnPropertyChanged();
            }
        }

        public ShapeViewModel(Shape shape) : base(shape)
        {
            AddInitalConnectionPoints();
        }
        
        private void ShapeMoveEvent(DragDeltaEventArgs e)
        {
            if (isResizing) return;
            ShapeMove(e.HorizontalChange, e.VerticalChange);
        } 

        private void MoveStartedEvent()
        {
            oldPos = new Point(X, Y);
        }

        private void MoveCompletedEvent()
        {
            newPos = new Point(X, Y);
            if (!oldPos.Equals(newPos))
            {
                new ShapeMoveCommand(this, oldPos, newPos).Execute();
            }
        }

        private void ResizeStartedEvent()
        {
            isResizing = true;
            oldPos = new Point(X, Y);
            oldSize = new Size(Width, Height);
        }

        private void ResizeCompletedEvent(DragCompletedEventArgs e)
        {
            e.Handled = true;
            isResizing = false;
            newPos = new Point(X, Y);
            newSize = new Size(Width, Height);
            if (!oldSize.Equals(newSize))
            {
                new ShapeResizeCommand(this, oldPos, newPos, oldSize, newSize).Execute();
            }
        }

        private void AddInitalConnectionPoints()
        {
            ConnectionPoints.Add(new ConnectionPointViewModel(this)
            {
                Vertical = VerticalAlignment.Top,
                Horizontal = HorizontalAlignment.Center
            });
            ConnectionPoints.Add(new ConnectionPointViewModel(this)
            {
                Vertical = VerticalAlignment.Bottom,
                Horizontal = HorizontalAlignment.Center
            });
            ConnectionPoints.Add(new ConnectionPointViewModel(this)
            {
                Vertical = VerticalAlignment.Center,
                Horizontal = HorizontalAlignment.Left
            });
            ConnectionPoints.Add(new ConnectionPointViewModel(this)
            {
                Vertical = VerticalAlignment.Center,
                Horizontal = HorizontalAlignment.Right
            });
        }

        #region Properties
        public ObservableCollection<ConnectionPointViewModel> ConnectionPoints { get; }
            = new ObservableCollection<ConnectionPointViewModel>();

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

        public string Name
        {
            get { return Model.Name; }
            set { SetValue(value); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public Brush ShapeColor
        {
            get
            {
                return _shapeColor;
            }

            set
            {
                _shapeColor = value;
                OnPropertyChanged();
            }
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
