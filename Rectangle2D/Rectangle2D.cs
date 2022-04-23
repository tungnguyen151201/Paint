using Contract;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Rectangle2D
{
    [Serializable]
    public class Rectangle2D : IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public string Name => "Rectangle";
        public System.Drawing.Color Color { get; set; }
        public double StrokeThickness { get; set; }
        public double[] StrokeDashArray { get; set; }
        public UIElement Draw()
        {
            DoubleCollection _strokeDashArray = new DoubleCollection();
            foreach (var element in StrokeDashArray)
            {
                _strokeDashArray.Add(element);
            }
            var rect = new Rectangle()
            {
                Width = Math.Abs(_rightBottom.X - _leftTop.X),
                Height = Math.Abs(_rightBottom.Y - _leftTop.Y),
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                StrokeThickness = StrokeThickness,
                StrokeDashArray = _strokeDashArray
            };

            Canvas.SetLeft(rect, _leftTop.X);
            Canvas.SetTop(rect, _leftTop.Y);

            return rect;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom = new Point2D() { X = x, Y = y };
        }

        public IShape Clone()
        {
            return new Rectangle2D();
        }
    }
}
