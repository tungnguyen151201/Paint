using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Contract
{
    [Serializable]
    public class Point2D : IShape
    {
        public double X { get; set; }
        public double Y { get; set; }

        public string Name => "Point";
        public System.Drawing.Color Color { get; set; }
        public double StrokeThickness { get; set; }
        public double[] StrokeDashArray { get; set; }
        public void HandleStart(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void HandleEnd(double x, double y)
        {
            X = x;
            Y = y;
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
                X1 = X,
                Y1 = Y,
                X2 = X,
                Y2 = Y,
                StrokeThickness = StrokeThickness,
                Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B)),                
                StrokeDashArray = _strokeDashArray
            };

            return l;
        }

        public IShape Clone()
        {
            return new Point2D();
        }
    }
}
