using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    class StrokeDashArray : INotifyPropertyChanged
    {
        public double[] DashArray { get; set; }
        public string StringDashArray { get; set; }
        public StrokeDashArray(double[] array)
        {
            DashArray = array;
            if (array.Length == 0)
            {
                StringDashArray = string.Empty;
                return;
            }
            StringDashArray = array[0].ToString();
            for (int i = 1; i < array.Length; i++)
            {
                StringDashArray += " " + array[i].ToString();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
