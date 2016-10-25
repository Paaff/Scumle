using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Scumle.View.Behaviour
{

 public class MouseScroll : MouseGesture
    {
        public WheelDirection Direction { get; set; }


        public static MouseScroll CtrlDown
        {
            get
            {
                return new MouseScroll(ModifierKeys.Control) { Direction = WheelDirection.Down };
            }
        }

        public MouseScroll() : base(MouseAction.WheelClick)
        {
        }

        public MouseScroll(ModifierKeys modifiers) : base(MouseAction.WheelClick, modifiers)
        {
        }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (!base.Matches(targetElement, inputEventArgs)) return false;
            if (!(inputEventArgs is MouseWheelEventArgs)) return false;
            var args = (MouseWheelEventArgs)inputEventArgs;
            switch (Direction)
            {
                case WheelDirection.None:
                    return args.Delta == 0;
                case WheelDirection.Up:
                    return args.Delta > 0;
                case WheelDirection.Down:
                    return args.Delta < 0;
                default:
                    return false;
            }
        }

        public enum WheelDirection
        {
            None,
            Up,
            Down,
        }

    }
}
