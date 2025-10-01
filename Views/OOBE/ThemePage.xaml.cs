using System.Windows.Controls;
using System.Windows.Media;
using ToolLib.JsonLib;
using ToolLib.RegistryLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;


namespace ARCV4.Views.OOBE
{
    /// <summary>
    /// ThemePage.xaml 的交互逻辑
    /// </summary>
    public partial class ThemePage : Page
    {
        public ThemePage()
        {
            InitializeComponent();
            Auto.Source = App.ConvertByteArrayToImageSource(RootResources.ThemeAuto);
            Dark.Source = App.ConvertByteArrayToImageSource(RootResources.ThemeBlack);
            White.Source = App.ConvertByteArrayToImageSource(RootResources.ThemeWhite);
        }
        
        public class ApplicationsTheme
        {
            public string Theme { get; set; }
        }
        public ApplicationsTheme theme;

        private void DarkButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica, true);

            theme = new ApplicationsTheme
            {
                Theme = "Dark"
            };
            Json.WriteJson(App.ConfigPath, theme);
            var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
            tb01.Foreground = brush;
            tb02.Foreground = brush;
            ContinueButton.IsEnabled = true;
        }

        private void LightButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica, true);
            theme = new ApplicationsTheme
            {
                Theme = "Light"
            };
            Json.WriteJson(App.ConfigPath, theme);
            var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
            tb01.Foreground = brush;
            tb02.Foreground = brush;
            ContinueButton.IsEnabled = true;
        }

        private void AutoButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var systemTheme = SystemThemeManager.GetCachedSystemTheme(); // SystemTheme

            // 映射成 ApplicationTheme（显式转换）
            var appTheme = systemTheme switch
            {
                SystemTheme.Light => ApplicationTheme.Light,
                SystemTheme.Dark => ApplicationTheme.Dark,
                _ => ApplicationTheme.Light
            };

            // 传入 ApplicationTheme（避免类型不匹配）
            ApplicationThemeManager.Apply(appTheme, WindowBackdropType.Mica, true);
            theme = new ApplicationsTheme
            {
                Theme = "Auto"
            };
            Json.WriteJson(App.ConfigPath, theme);
            if (appTheme == ApplicationTheme.Light)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
            if(appTheme == ApplicationTheme.Dark)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
            ContinueButton.IsEnabled = true;
        }

        private void ContinueButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var win = Application.Current.Windows.OfType<OOBEWindow>().FirstOrDefault();
            win.OOBEFrame.Navigate(new NamelistPage());
            win.OOBEProgress.Value = 66;
        }
    }
}
