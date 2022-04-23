using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Line2D;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Win32;
using System.ComponentModel;

namespace Paint
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool _isDrawing = false;
        List<IShape> _shapes = new List<IShape>();
        IShape _preview;
        string _selectedShapeName = "";
        Dictionary<string, IShape> _prototypes = new Dictionary<string, IShape>();
        System.Drawing.Color _color = System.Drawing.Color.Black;
        double _strokeThickness = 1;
        BindingList<StrokeDashArray> strokeStyles = new BindingList<StrokeDashArray>
        {
            new StrokeDashArray(new double[] { }),
            new StrokeDashArray(new double[] { 1 }),
            new StrokeDashArray(new double[] { 6, 1 }),
            new StrokeDashArray(new double[] { 4, 1, 1, 1, 1, 1 }),
        };

        private void canvas_MouseDown(object sender,
            MouseButtonEventArgs e)
        {
            _isDrawing = true;

            Point pos = e.GetPosition(canvas);

            _preview.HandleStart(pos.X, pos.Y);

            _preview.SetColor(_color);

            _preview.SetStrokeThickness(_strokeThickness);

            _preview.SetStrokeDashArray(strokeStyles[strokeStyleCombo.SelectedIndex].DashArray);

        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDrawing)
            {
                Point pos = e.GetPosition(canvas);
                _preview.HandleEnd(pos.X, pos.Y);

                // Xoá hết các hình vẽ cũ
                canvas.Children.Clear();

                // Vẽ lại các hình trước đó
                foreach (var shape in _shapes)
                {
                    UIElement element = shape.Draw();
                    canvas.Children.Add(element);
                }

                // Vẽ hình preview đè lên
                canvas.Children.Add(_preview.Draw());               
            }
        }

        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDrawing = false;

            // Thêm đối tượng cuối cùng vào mảng quản lí
            Point pos = e.GetPosition(canvas);
            _preview.HandleEnd(pos.X, pos.Y);
            _shapes.Add(_preview);

            // Sinh ra đối tượng mẫu kế
            _preview = _prototypes[_selectedShapeName].Clone();

            // Ve lai Xoa toan bo
            canvas.Children.Clear();

            // Ve lai tat ca cac hinh
            foreach (var shape in _shapes)
            {
                var element = shape.Draw();
                canvas.Children.Add(element);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (type.IsClass)
                    {
                        if (typeof(IShape).IsAssignableFrom(type))
                        {
                            var shape = Activator.CreateInstance(type) as IShape;
                            _prototypes.Add(shape.Name, shape);                        }
                    }
                }
            }

            // Tạo ra các nút bấm hàng mẫu
            foreach (var item in _prototypes)
            {
                var shape = item.Value as IShape;

                Image img = new Image();
                switch(shape.Name)
                {
                    case "Line":
                        img.Source = new BitmapImage(new Uri(exeFolder + "\\Images\\line.png"));
                        break;
                    case "Rectangle":
                        img.Source = new BitmapImage(new Uri(exeFolder + "\\Images\\rectangle.png"));
                        break;
                    case "Ellipse":
                        img.Source = new BitmapImage(new Uri(exeFolder + "\\Images\\ellipse.png"));
                        break;
                }                

                StackPanel stackPnl = new StackPanel();
                stackPnl.Orientation = Orientation.Horizontal;
                stackPnl.Children.Add(img);

                var button = new Button()
                {
                    Content = stackPnl,
                    Background = Brushes.White,
                    Width = 30,
                    Height = 30,
                    Margin = new Thickness(5, 5, 5, 5),
                    Tag = shape.Name
                };
                button.Click += prototypeButton_Click;
                prototypesStackPanel.Children.Add(button);
            }

            _selectedShapeName = _prototypes.First().Value.Name;
            _preview = _prototypes[_selectedShapeName].Clone();
            strokeStyleCombo.ItemsSource = strokeStyles;
        }

        private void prototypeButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedShapeName = (sender as Button).Tag as string;

            _preview = _prototypes[_selectedShapeName];
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            _shapes.Clear();
        }
        private void SavePaint()
        {
            string FILE_PATH;
            if (Title == "Untitled")
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                saveFileDialog.Filter = "BIN Files (*.bin)|*.bin";
                if (saveFileDialog.ShowDialog() == true)
                {
                    FILE_PATH = saveFileDialog.FileName;
                    Title = FILE_PATH;
                }
                else return;
            }
            else
            {
                FILE_PATH = Title;
            }
            object ob1 = _shapes;
            byte[] ba = ConvertObjectToByteArray(ob1);
            File.WriteAllBytes(FILE_PATH, ba);
        }
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SavePaint();
        }
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            string FILE_PATH;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog.Filter = "BIN Files (*.bin)|*.bin";
            if (openFileDialog.ShowDialog() == true)
            {
                FILE_PATH = openFileDialog.FileName;
            }
            else return;
            Title = FILE_PATH;
            byte[] ba = File.ReadAllBytes(FILE_PATH);
            List<IShape> ob2 = (List<IShape>)ConvertByteArrayToObject(ba);
            canvas.Children.Clear();
            _shapes.Clear();
            _shapes = ob2;
            foreach (var shape in _shapes)
            {
                UIElement element = shape.Draw();
                canvas.Children.Add(element);
            }
        }
        public static byte[] ConvertObjectToByteArray(object ob)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, ob);
            return ms.ToArray();
        }

        public static object ConvertByteArrayToObject(byte[] ba)
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            bf.Binder = new MyBinder();
            Stream stream = new MemoryStream(ba);
            return bf.Deserialize(stream);
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save the current paint?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SavePaint();
            }                           
            Title = "Untitled";
            canvas.Children.Clear();
            _shapes.Clear();
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            string FILE_PATH;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            saveFileDialog.Filter = "Image Files (*.png)|*.png";
            if (saveFileDialog.ShowDialog() == true)
            {
                FILE_PATH = saveFileDialog.FileName;
            }
            else return;
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                (int)canvas.RenderSize.Height, 96d, 96d, PixelFormats.Default);
            rtb.Render(canvas);

            var crop = new CroppedBitmap(rtb, new Int32Rect());

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(crop));

            using (var fs = File.OpenWrite(FILE_PATH))
            {
                pngEncoder.Save(fs);
            }
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)ClrPcker_Background.SelectedColor;
            _color = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        private void strokeThicknessText_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _strokeThickness = double.Parse(strokeThicknessText.Text);
            }
            catch (Exception)
            {
            }
        }
    }
}
