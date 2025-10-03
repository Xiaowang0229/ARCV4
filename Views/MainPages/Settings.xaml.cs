using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Brush = System.Windows.Media.Brush;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Page
    {
        private Config configvalue;
        private Config ConfigWriteOnlyValue;
        public Settings()
        {
            InitializeComponent();
            //读
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            ConfigWriteOnlyValue = Json.ReadJson<Config>(App.ConfigPath);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FluentWindow window = new OOBEWindow(true);
            window.ShowDialog();
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.Close();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //初始化各控件状态
            ChooseNamelistButton.Content = configvalue.CurrentInuseNamelist;
            foreach(string Name in configvalue.namelistlocations)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = Name;
                menuItem.Click += NamelistItem_Click;
                NameListMenu.Items.Add(menuItem);
            }
            AutoDianmingIntervalTickBox.Text = configvalue.AutoDianmingintervaltick.ToString();
            ListDianmingNumbers.Text = configvalue.ListDianmingNumbers.ToString();
            if(configvalue.IsStartUpCheckUpdate)
            {
                StartUpCheckUpdate.IsChecked = true;
            }

            if(configvalue.AppuseTheme == "Dark")
            {
                DarkModeButton.IsChecked = true;
            }
            else if (configvalue.AppuseTheme == "Light")
            {
                LightModeButton.IsChecked = true;
            }
            else if (configvalue.AppuseTheme == "Auto")
            {
                FollowSystemButton.IsChecked = true;
            }
        }

        private void NamelistItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem ClickedMenuItem = sender as MenuItem;
            //System.Windows.MessageBox.Show(ClickedMenuItem.Header.ToString(),"Information");
            ConfigWriteOnlyValue.CurrentInuseNamelist = ClickedMenuItem.Header.ToString();
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.RootNavi.Navigate(typeof(AutoDianmingPage));
            win.RootNavi.Navigate(typeof(Settings));
        }

        private void DarkModeButton_Checked(object sender, RoutedEventArgs e)
        {
            ConfigWriteOnlyValue.AppuseTheme = "Dark";
            ApplicationThemeManager.Apply(ApplicationTheme.Dark, WindowBackdropType.Mica, true);
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Dark);
        }

        private void LightModeButton_Checked(object sender, RoutedEventArgs e)
        {
            ConfigWriteOnlyValue.AppuseTheme = "Light";
            ApplicationThemeManager.Apply(ApplicationTheme.Light, WindowBackdropType.Mica, true);
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Light);

        }

        private void FollowSystemButton_Checked(object sender, RoutedEventArgs e)
        {
            var appTheme = App.Reg_AppsUseLightMode() switch
            {
                true => ApplicationTheme.Light,
                false => ApplicationTheme.Dark
            };

            // 传入 ApplicationTheme（避免类型不匹配）
            ApplicationThemeManager.Apply(appTheme, WindowBackdropType.Mica, true);
            ConfigWriteOnlyValue.AppuseTheme = "Auto";
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            if (appTheme == ApplicationTheme.Light)
            {
                //var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
                //tb01.Foreground = brush;
                //tb02.Foreground = brush;
                //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Light);
            }
            if (appTheme == ApplicationTheme.Dark)
            {
                //var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                //tb01.Foreground = brush;
                //tb02.Foreground = brush;
                //ThemeHelpers.ApplyTextBlockForeground(Wpf.Ui.Appearance.ApplicationTheme.Dark);
            }

        }

        private void StartUpCheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (StartUpCheckUpdate.IsChecked == true)
            {
                ConfigWriteOnlyValue.IsStartUpCheckUpdate = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else if (StartUpCheckUpdate.IsChecked == false)
            {
                ConfigWriteOnlyValue.IsStartUpCheckUpdate = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
        }

        

        private void DianMingFont_Click(object sender, RoutedEventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.ShowDialog();

            Font result = fontDialog.Font;
            //MessageBox.Show(result.Name.ToString());
            ConfigWriteOnlyValue.DianMingFont = result.Name.ToString();
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            
        }

        private void DianMingFontColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.ShowDialog();

            Brush result = App.ConvertColorToBrush(colorDialog.Color);
            ConfigWriteOnlyValue.DianmingFontColor = result;
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);

        }

        private void DianmingMusic_Click(object sender, RoutedEventArgs e)
        {
            // 创建 SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置默认文件扩展名
            saveFileDialog.DefaultExt = ".mp3";

            // 设置可选文件类型
            saveFileDialog.Filter = "点名音乐文件 (*.mp3)|*.mp3";

            // 打开对话框
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                ConfigWriteOnlyValue.DianmingMusic = saveFileDialog.FileName;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
        }

        private void EditNamelist_Click(object sender, RoutedEventArgs e)
        {
            FluentWindow window = new NameListEditor(configvalue.CurrentInuseNamelist);
            window.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 显示消息框，包含“是”和“否”按钮
            DialogResult result = (DialogResult)MessageBox.Show(
            "此操作不可逆！是否继续操作？",
            "提示",
            (System.Windows.MessageBoxButton)MessageBoxButtons.YesNo,
            (MessageBoxImage)MessageBoxIcon.Warning
            );

            // 判断用户点击的按钮
            if (result == DialogResult.Yes)
            {
                MessageBox.Show("请重新启动自动点名器来完成更改","提示",System.Windows.MessageBoxButton.OK,MessageBoxImage.Information);
                Environment.Exit(0);
            }
        }

        

        private void AutoDianmingIntervalTickBox_PreviewTextInput_1(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        private void ListDianmingNumbers_PreviewTextInput_1(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
        }

        


        private void AutoDianmingIntervalTickBox_TextChanged(object sender, NumberBoxValueChangedEventArgs args)
        {
            ConfigWriteOnlyValue.AutoDianmingintervaltick = int.Parse(AutoDianmingIntervalTickBox.Text);
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
        }

        private void ListDianmingNumbers_TextChanged(object sender, NumberBoxValueChangedEventArgs args)
        {
            ConfigWriteOnlyValue.ListDianmingNumbers = int.Parse(ListDianmingNumbers.Text);
            Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
        }

        private void ListDianmingNumbers_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void AutoDianmingIntervalTickBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        
    }
}
