using Contract;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ellipse2D
{
    [Serializable]
    public class Ellipse2D : IShape
    {
        private Point2D _leftTop = new Point2D();
        private Point2D _rightBottom = new Point2D();

        public string Name => "Ellipse";
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
            var ellipse = new Ellipse()
            {
                Width = Math.Abs(_rightBottom.X - _leftTop.X),
                Height = Math.Abs(_rightBottom.Y - _leftTop.Y),
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                StrokeThickness = StrokeThickness,
                StrokeDashArray = _strokeDashArray
            };
            Canvas.SetLeft(ellipse, _leftTop.X);
            Canvas.SetTop(ellipse, _leftTop.Y);

            return ellipse;
        }

        public void HandleStart(double x, double y)
        {
            _leftTop.X = x;
            _leftTop.Y = y;
        }

        public void HandleEnd(double x, double y)
        {
            _rightBottom.X = x;
            _rightBottom.Y = y;
        }

        public IShape Clone()
        {
            return new Ellipse2D();
        }
    }
}
