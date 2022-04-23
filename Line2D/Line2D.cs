using Contract;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Line2D
{
    [Serializable]
    public class Line2D : IShape
    {
        private Point2D _start = new Point2D();
        private Point2D _end = new Point2D();

        public string Name => "Line";
        public System.Drawing.Color Color { get; set; }
        public double StrokeThickness { get; set; }
        public double[] StrokeDashArray { get; set; }

        public void HandleStart(double x, double y)
        {
            _start = new Point2D() { X = x, Y = y };
        }

        public void HandleEnd(double x, double y)
        {
            _end = new Point2D() { X = x, Y = y };
        }

        public UIElement Draw()
        {
            DoubleCollection _strokeDashArray = new DoubleCollection();
            foreach (var element in StrokeDashArray)
            {
                _strokeDashArray.Add(element);
            }
            Line l = new Line()
            {
                X1 = _start.X,
                Y1 = _start.Y,
                X2 = _end.X,
                Y2 = _end.Y,
                StrokeThickness = StrokeThickness,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),
                StrokeDashArray = _strokeDashArray
            };

            return l;
        }

        public IShape Clone()
        {
            return new Line2D();
        }
    }
}
