using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToolLib.JsonLib;
using Wpf.Ui.Controls;

namespace ARCV4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        //系统级根常量
        public static readonly string ProgramVersion = "4.0.0.0 Indev 1";
        public static readonly string ProgramPath = Directory.GetCurrentDirectory();
        public static readonly string ConfigPath = ProgramPath + @"\Config.json";
        //备用根常量
        public static FluentWindow OOBEthisWindow;

        public class Config
        {
            public bool OOBEStatus { get; set; }
        }
        public Config OOBEvalue;



        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //预先判断是否首次启动应用
            
            if (!File.Exists(ConfigPath))
            {
                
                
                    FluentWindow window = new Views.OOBEWindow();
                    window.Show();
                
                

            }
            else if (File.Exists(ConfigPath))
            {
                OOBEvalue = Json.ReadJson<Config>(ConfigPath);
                if (OOBEvalue.OOBEStatus == true)
                {
                    FluentWindow window = new Views.MainWindow();
                    window.Show();
                }
                else if (OOBEvalue.OOBEStatus != true)
                {
                    FluentWindow window = new Views.OOBEWindow();
                    window.Show();
                }
                
            }

            
        }

        public static ImageSource ConvertByteArrayToImageSource(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0) return null;

            using (MemoryStream stream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; 
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 
                return bitmapImage;
            }
        }

        public static async Task<string[]> SplitLinesAsync(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            using (var reader = new StringReader(text))
            {
                var lines = new System.Collections.Generic.List<string>();
                string? line;
                while ((line = await reader.ReadLineAsync()) != null) // 异步逐行读取
                {
                    lines.Add(line);
                }
                return lines.ToArray();
            }
        }

        public static string[] AddItemIntoStrings(string[] Origin, string New)
        {
            List<string> list = new List<string>(Origin);
            list.Add(New);
            return list.ToArray();
        }
    }

}

    
