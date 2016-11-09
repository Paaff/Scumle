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


namespace Scumle.ViewModel
{

    public class ShapeViewModel : ViewModelBase<Shape>
    {
        private bool _isSelected = false;
        private Point oldPos;
        private Point newPos;

        public RelayCommand MoveStartedCommand => new RelayCommand(MoveStarted);
        public RelayCommand MoveCompletedCommand => new RelayCommand(MoveCompleted);

        public ShapeViewModel(Shape shape) : base(shape)
        {
            AddInitalConnectionPoints();
        }
            

        private void MoveStarted()
        {
            oldPos = new Point(X, Y);
        }

        private void MoveCompleted()
        {
            newPos = new Point(X, Y);
            if (!oldPos.Equals(newPos))
            {
                new ShapeMoveCommand(this, oldPos, newPos).Execute();
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
            set { SetValue(value); }
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
            set { SetValue(value); }
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
