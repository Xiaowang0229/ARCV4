using System.Windows.Controls;
using System.Windows.Media;
using ToolLib.JsonLib;
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
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
        }

        public Config configvalue;

        private void DarkButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica, true);
            
            configvalue.AppuseTheme = "Dark";
            Json.WriteJson(App.ConfigPath, configvalue);
            var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
            //tb01.Foreground = brush;
            //tb02.Foreground = brush;
            ContinueButton.IsEnabled = true;
            //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Dark);
        }

        private void LightButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica, true);
            configvalue.AppuseTheme = "Light";
            Json.WriteJson(App.ConfigPath, configvalue);
            var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
            //tb01.Foreground = brush;
            //tb02.Foreground = brush;
            ContinueButton.IsEnabled = true;
            //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Light);
        }

        private void AutoButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var appTheme = App.Reg_AppsUseLightMode() switch
            {
                true => ApplicationTheme.Light,
                false => ApplicationTheme.Dark
            };

            // 传入 ApplicationTheme（避免类型不匹配）
            ApplicationThemeManager.Apply(appTheme, WindowBackdropType.Mica, true);
            configvalue.AppuseTheme = "Auto";
            Json.WriteJson(App.ConfigPath, configvalue);
            if (appTheme == ApplicationTheme.Light)
            {
                //var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
                //tb01.Foreground = brush;
                //tb02.Foreground = brush;
                //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Light);
            }
            if(appTheme == ApplicationTheme.Dark)
            {
                //var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                //tb01.Foreground = brush;
                //tb02.Foreground = brush;
                //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Dark);
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
