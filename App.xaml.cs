using ARCV4.Views;
using ARCV4.Views.MainPages;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToolLib.JsonLib;
using ToolLib.RegistryLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;
using FontFamily = System.Drawing.FontFamily;

namespace ARCV4
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        //系统级根常量
        public static readonly string ProgramVersion = "Release 4.0.0.0";
        public static readonly string ProgramPath = Directory.GetCurrentDirectory();
        public static readonly string ConfigPath = ProgramPath + @"\Config.json";
        public static bool IsDianmingStatus = false;
        public static string Latestversion;
        public static string Latestlog;
        public static string Latestlink;
        public static int MainpageStatus;
        public static bool IsAutoDianmingPage = true;
        public static string Versionlog = "--[点名器 4.0.0.0 更新说明]--\r\n\r\n[全新架构]\r\n本次版本由零开始使用 C# 重写，逻辑更清晰，性能更稳定，响应速度显著提升。\r\n\r\n[界面升级]\r\n采用 WPF-UI 打造现代化界面，交互更流畅，主题风格和控件体验全面优化，操作更直观。\r\n\r\n[核心优化]\r\n点名算法升级，随机性更均匀，点名结果更可靠，同时支持更大班级和更多功能扩展。\r\n\r\n[扩展基础]\r\n此次重构为未来新增功能提供了坚实基础，后续更新可快速迭代，无需担心兼容问题。\r\n\r\n[总结]\r\n4.0.0.0 是一次真正的全新升级版本，不仅外观焕然一新，也让点名体验更高效、更智能。";
        //备用根常量
        public static FluentWindow OOBEthisWindow;


        public Config configvalue;



        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            //预先判断是否首次启动应用

            if (!File.Exists(ConfigPath))
            {
                ConfigHelper.ConfigInitalize();
                FluentWindow window = new Views.OOBEWindow();
                window.Show();

            }
            else if (File.Exists(ConfigPath))
            {
                
                configvalue = Json.ReadJson<Config>(ConfigPath);
                if (configvalue.OOBEStatus == true)
                {
                    if (configvalue.AppuseTheme == "Dark")
                    {
                        ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica, true);
                    }
                    else if (configvalue.AppuseTheme == "Light")
                    {
                        ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica, true);
                    }
                    else if (configvalue.AppuseTheme == "Auto")
                    {
                        var appTheme = App.Reg_AppsUseLightMode() switch
                        {
                            true => ApplicationTheme.Light,
                            false => ApplicationTheme.Dark
                        };

                        // 传入 ApplicationTheme（避免类型不匹配）
                        ApplicationThemeManager.Apply(appTheme, WindowBackdropType.Mica, true);


                    }

                    FluentWindow window = new Views.MainWindow();
                    window.Show();
                    
                    if (configvalue.IsStartUpCheckUpdate)
                    {
                        string a = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestVersion");
                        if (a != App.ProgramVersion)
                        {
                            FluentWindow window2 = new UpdateWindow();
                            window2.ShowDialog();
                            window2.Topmost = true;
                            window2.Topmost = false;
                            if (configvalue.Hoverball)
                            {
                                Window window1 = new HoverBall(configvalue.HoverballTopMost);
                                window1.Show();
                            }
                        }
                    }
                    else if (configvalue.IsStartUpCheckUpdate == false)
                    {
                        if (configvalue.Hoverball)
                        {
                            Window window1 = new HoverBall(configvalue.HoverballTopMost);
                            window1.Show();
                        }
                    }

                }
                else if (configvalue.OOBEStatus != true)
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



        public static async Task<string> GetWebPageAsync(string url)
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            string result = await client.GetStringAsync(url);
            return result;
        }

        public static Brush ConvertColorToBrush(Color color)
        {
            return new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                color.A,
                color.R,
                color.G,
                color.B));
        }

        public static bool Reg_AppsUseLightMode()
        {
            int themestatus = (int)RegistryHelper.ReadRegistryValue(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme");
            if (themestatus == 0x00000001)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Restart()
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(exePath);
            try
            {
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win._trayManager.Dispose();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            System.Windows.Application.Current.Shutdown();
        }

        public static System.Windows.Media.FontFamily ConvertToFontFamily(Font font)
        {
            if (font == null)
                throw new ArgumentNullException(nameof(font));

            // 用字体名称构造 WPF 的 FontFamily
            return new System.Windows.Media.FontFamily(font.FontFamily.Name);
        }

        public static void ConvertAnsiFileToUtf8InPlace(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            // 按 GB2312 读取文本
            string content = File.ReadAllText(filePath, Encoding.GetEncoding("GB2312"));

            // 原地写入 UTF-8（带 BOM）
            File.WriteAllText(filePath, content, new UTF8Encoding(true));
        }

        public static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child is T t) return t;
                var result = FindVisualChild<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static Icon ByteArrayToIcon(byte[] iconBytes)
        {
            using (MemoryStream ms = new MemoryStream(iconBytes))
            {
                return new Icon(ms);
            }

        }
    }

    

}

    
