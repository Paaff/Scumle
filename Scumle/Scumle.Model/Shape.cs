﻿using Scumle.Model.Shapes;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Scumle.Model
{
    public abstract class Shape : ModelBase, IShape
    {
        private double _width;
        private double _height;
        private byte a, r, g, b;
        private SolidColorBrush color;

        public Shape(double _X, double _Y, double _Width, double _Height, Color col, string _ID)
        {
            IsSelected = false;
            Width = _Width;
            Height = _Height;
            X = _X;
            Y = _Y;
            ShapeColor = new SolidColorBrush(col);
            ID = _ID;
            InitializeConnectionPoints();

        }

        private void InitializeConnectionPoints()
        {
            ConnectionPoints = new List<IPoint>()
            {
                new ConnectionPoint(this, HorizontalAlignment.Center, VerticalAlignment.Top),
                new ConnectionPoint(this, HorizontalAlignment.Left, VerticalAlignment.Center),
                new ConnectionPoint(this, HorizontalAlignment.Right, VerticalAlignment.Center),
                new ConnectionPoint(this, HorizontalAlignment.Center, VerticalAlignment.Bottom)
            };
        }

        // For XML Serialization
        public Shape() { }

        [XmlIgnore]
        public bool IsSelected { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        [XmlIgnore]
        public IList<IPoint> ConnectionPoints { get; private set; }
        public double Width
        {
            get { return _width; }
            set { if (value > 0) _width = value; }
        }
        public double Height
        {
            get { return _height; }
            set { if (value > 0) _height = value; }
        }

        [XmlIgnore]
        public Brush ShapeColor { get; set; }       
   
        public byte ColorR
        {
            get
            {
                if(ShapeColor != null)
                {
                    return ((Color)ShapeColor.GetValue(SolidColorBrush.ColorProperty)).R;
                } else
                {
                    return r;
                }
            }
            set { r = value; }
        }
        public byte ColorG
        {
            get
            {
                if (ShapeColor != null)
                {
                    return ((Color)ShapeColor.GetValue(SolidColorBrush.ColorProperty)).G;
                }
                else
                {
                    return g;
                }
            }
            set { g = value; }
        }
        public byte ColorB
        {
            get
            {
                if (ShapeColor != null)
                {
                    return ((Color)ShapeColor.GetValue(SolidColorBrush.ColorProperty)).B;
                }
                else
                {
                    return b;
                }
            }
            set { b = value; }
        }

        public string ID  { get; set; }

        public void ShapeMove(double X, double Y)
        {
            this.X += X;
            this.Y += Y;
        }

        public void ShapeResize(double dX, double dY)
        {
            Width += dX;
            Height += dY;
        }
    }
}
