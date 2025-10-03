using ARCV4.Views.MainPages;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Abstractions;

namespace ARCV4.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public Config configvalue;
        
        public MainWindow()
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            configvalue = Json.ReadJson<Config>(App.ConfigPath);

        }

        private void FluentWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RootNavi.Navigate(typeof(AutoDianmingPage));
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += ThemeTimerTickEvent();
        }

        private EventHandler ThemeTimerTickEvent()
        {
            
            if (configvalue.AppuseTheme == "Auto")
            {
                var appTheme = App.Reg_AppsUseLightMode() switch
                {
                    true => ApplicationTheme.Light,
                    false => ApplicationTheme.Dark
                };

                // 传入 ApplicationTheme（避免类型不匹配）
                ApplicationThemeManager.Apply(appTheme, WindowBackdropType.Mica, true);
                

            }
            return null;
        }
    }
}
