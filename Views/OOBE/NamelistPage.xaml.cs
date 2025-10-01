using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;
using MessageBox = System.Windows.Forms.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;


namespace ARCV4.Views.OOBE
{
    /// <summary>
    /// NamelistPage.xaml 的交互逻辑
    /// </summary>
    public partial class NamelistPage : Page
    {
        public static string filename;
        public NamelistPage()
        {
            InitializeComponent();
            var appTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
            if (appTheme == ApplicationTheme.Dark)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
            else if(appTheme == ApplicationTheme.Light)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
        }

        public class Namelist
        {
            public string[] names { get; set; }
        }
        public Namelist namevalue;

        public class Config
        {
            public bool OOBEStatus { get; set; }
            public string[] namelistlocations { get; set; }
        }
        public Config configvalue;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 创建 SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置默认文件扩展名
            saveFileDialog.DefaultExt = ".json";

            // 设置可选文件类型
            saveFileDialog.Filter = "用于储存名单的 Json 文件 (*.json)|*.json";

            // 打开对话框
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                filename = saveFileDialog.FileName;
                
                Rich123.IsReadOnly = false;
                Finally.IsEnabled = true;
            }
        }

        private async void Finally_Click(object sender, RoutedEventArgs e)
        {
            //测试专用代码

            string text = new TextRange(
            Rich123.Document.ContentStart,
            Rich123.Document.ContentEnd
            ).Text;

            string[] lines = text.Split(
                new[] { "\r\n", "\n", "\r" },
                StringSplitOptions.RemoveEmptyEntries
            );
            string[] filenamelist = { filename };

            namevalue = new Namelist
            {
                names = lines
            };
            Json.WriteJson(filename, namevalue);
            configvalue = new Config
            {
                OOBEStatus = true,
                namelistlocations = filenamelist
            };
            Json.WriteJson(App.ConfigPath, configvalue);

            
            FluentWindow window = new MainWindow();
            window.Show();
            var win = System.Windows.Application.Current.Windows.OfType<OOBEWindow>().FirstOrDefault();
            win.Close();

        }
    }
}
