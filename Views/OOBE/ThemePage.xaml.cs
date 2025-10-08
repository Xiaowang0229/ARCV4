using ARCV4.Views.MainPages;
using System.Windows.Controls;
using System.Windows.Media;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Application = System.Windows.Application;
using Brush = System.Windows.Media.Brush;


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
            Auto.Source = App.ConvertByteArrayToImageSource(RootResources.AutoTheme);
            Dark.Source = App.ConvertByteArrayToImageSource(RootResources.DarkTheme);
            White.Source = App.ConvertByteArrayToImageSource(RootResources.LightTheme);
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

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LightButton.IsChecked = true;
        }

        

        private void FontButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowColor = true;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Media.FontFamily fontfamilyresult = App.ConvertToFontFamily(fontDialog.Font);
                Brush colorresult = App.ConvertColorToBrush(fontDialog.Color);
                //MessageBox.Show(result.Name.ToString());
                configvalue.DianMingFont = fontfamilyresult;
                configvalue.DianmingFontColor = colorresult;
                Json.WriteJson(App.ConfigPath, configvalue);
                
            }
        }

        private void MusicButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // 创建 SaveFileDialog
            OpenFileDialog saveFileDialog = new OpenFileDialog();

            // 设置默认文件扩展名
            saveFileDialog.DefaultExt = ".mp3";

            // 设置可选文件类型
            saveFileDialog.Filter = "点名音乐文件 (*.mp3)|*.mp3";

            // 打开对话框
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                configvalue.DianmingMusic = saveFileDialog.FileName;
                Json.WriteJson(App.ConfigPath, configvalue);
                
            }
        }
    }
}
