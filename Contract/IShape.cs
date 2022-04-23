using System;
using System.Windows;
using System.Windows.Media;

namespace Contract
{
    public interface IShape
    {
        string Name { get; }
        System.Drawing.Color Color { get; set; }
        public double StrokeThickness { get; set; }
        public double[] StrokeDashArray { get; set; }
        void HandleStart(double x, double y);
        void HandleEnd(double x, double y);

        UIElement Draw();
        IShape Clone();
        void SetColor(System.Drawing.Color _color)
        {
            Color = _color;
        }
        void SetStrokeThickness(double _strokeThickness)
        {
            StrokeThickness = _strokeThickness;
        }
        void SetStrokeDashArray(double[] _strokeDashArray)
        {
            StrokeDashArray = _strokeDashArray;           
        }
    }
}
