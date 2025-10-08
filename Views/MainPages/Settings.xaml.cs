using System.IO;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Brush = System.Windows.Media.Brush;
using MenuItem = Wpf.Ui.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using TextBox = Wpf.Ui.Controls.TextBox;

namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : Page
    {
        private Config configvalue;
        private Config ConfigWriteOnlyValue;
        private DispatcherTimer AutoIntervaltimer = new DispatcherTimer();
        private DispatcherTimer ListNumtimer = new DispatcherTimer();
        public Settings()
        {
            InitializeComponent();
            App.MainpageStatus = 10;
            //读
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            ConfigWriteOnlyValue = Json.ReadJson<Config>(App.ConfigPath);
            AutoIntervaltimer.Interval = TimeSpan.FromMilliseconds(10); // 每1秒触发一次
            AutoIntervaltimer.Tick += AutoIntervalTimer_Tick;
            AutoIntervaltimer.Start();
            ListNumtimer.Interval = TimeSpan.FromMilliseconds(10); // 每1秒触发一次
            ListNumtimer.Tick += ListNumTimer_Tick;
            ListNumtimer.Start();
        }

        private void ListNumTimer_Tick(object sender, EventArgs e)
        {
            if(int.Parse(ListNum.Text) < 1)
            {
                ListNum.Text = "1";
                ListNum.CaretIndex = ListNum.Text.Length;
                ConfigWriteOnlyValue.ListDianmingNumbers = int.Parse(ListNum.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else if(int.Parse(ListNum.Text) > 50)
            {
                ListNum.Text = "50";
                ListNum.CaretIndex = ListNum.Text.Length;
                ConfigWriteOnlyValue.ListDianmingNumbers = int.Parse(ListNum.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else if(string.IsNullOrEmpty(ListNum.Text))
            {
                ListNum.Text = "1";
                ListNum.CaretIndex = ListNum.Text.Length;
                ConfigWriteOnlyValue.ListDianmingNumbers = int.Parse(ListNum.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else
            {
                ConfigWriteOnlyValue.ListDianmingNumbers = int.Parse(ListNum.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
        }

        private void AutoIntervalTimer_Tick(object sender, EventArgs e)
        {
            if (int.Parse(AutoInterval.Text) < 1)
            {
                AutoInterval.Text = "1";
                AutoInterval.CaretIndex = AutoInterval.Text.Length;
                ConfigWriteOnlyValue.AutoDianmingintervaltick = int.Parse(AutoInterval.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);

            }
            else if(int.Parse(AutoInterval.Text) > 1000)
            {
                AutoInterval.Text = "1000";
                AutoInterval.CaretIndex = AutoInterval.Text.Length;
                ConfigWriteOnlyValue.AutoDianmingintervaltick = int.Parse(AutoInterval.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else if(string.IsNullOrEmpty(AutoInterval.Text))
            {
                AutoInterval.Text = "1";
                AutoInterval.CaretIndex = AutoInterval.Text.Length;
                ConfigWriteOnlyValue.AutoDianmingintervaltick = int.Parse(AutoInterval.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else
            {
                ConfigWriteOnlyValue.AutoDianmingintervaltick = int.Parse(AutoInterval.Text);
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
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
            //读
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            ConfigWriteOnlyValue = Json.ReadJson<Config>(App.ConfigPath);

            //初始化各控件状态
            AutoInterval.Text = configvalue.AutoDianmingintervaltick.ToString();
            ListNum.Text = configvalue.ListDianmingNumbers.ToString();
            if (configvalue.DianmingPassword == null)
            {
                DelPassword.Visibility = Visibility.Hidden;
                LockOnSettings.Visibility = Visibility.Hidden;
                EditPassword.Content = "设置修改名单密码";
                
            }

            if(configvalue.DianMingFont == null && configvalue.DianmingFontColor == null && configvalue.DianmingMusic == null)
            {
                DefaultCostumSettings.Visibility = Visibility.Hidden;
            }

            if(configvalue.Hoverball)
            {
                LaunchHoverBall.IsChecked = true;
                if(configvalue.HoverballTopMost)
                {
                    HoverBallTopButton.IsChecked = true;
                }
            }
            else
            {
                HoverBallTopButton.IsEnabled = false;
            }

            if(configvalue.IsApplicationMinimizetoTray == true)
            {
                ApplicationExittoTray.IsChecked = true;
            }

            if (configvalue.LockOnSettings)
            {
                LockOnSettings.IsChecked = true;
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                FluentWindow window = new PasswordWindow(true, "请输入密码来更改设置项：");
                bool? result = window.ShowDialog();
                if (result == false)
                {
                    win.RootNavi.Navigate(typeof(AutoDianmingPage));
                    MessageBox.Show("密码错误，请返回重试", "错误", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            ChooseNamelistButton.Content = configvalue.CurrentInuseNamelist;
            foreach(string Name in configvalue.namelistlocations)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = Name;
                menuItem.Click += NamelistItem_Click;
                NameListMenu.Items.Add(menuItem);
            }
            
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
            fontDialog.ShowColor = true;
            if(fontDialog.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Media.FontFamily fontfamilyresult = App.ConvertToFontFamily(fontDialog.Font);
                Brush colorresult = App.ConvertColorToBrush(fontDialog.Color);
                //MessageBox.Show(result.Name.ToString());
                ConfigWriteOnlyValue.DianMingFont = fontfamilyresult;
                ConfigWriteOnlyValue.DianmingFontColor = colorresult;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.RootNavi.Navigate(typeof(AutoDianmingPage));
                win.RootNavi.Navigate(typeof(Settings));
            }

            
            
        }

        

        private void DianmingMusic_Click(object sender, RoutedEventArgs e)
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
                ConfigWriteOnlyValue.DianmingMusic = saveFileDialog.FileName;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.RootNavi.Navigate(typeof(AutoDianmingPage));
                win.RootNavi.Navigate(typeof(Settings));
            }
        }

        private void EditNamelist_Click(object sender, RoutedEventArgs e)
        {
            if(configvalue.DianmingPassword != null)
            {
                FluentWindow window = new PasswordWindow(true,"请输入密码来授权本次操作");
                bool? result = window.ShowDialog();
                if (result == true)
                {
                    FluentWindow window2 = new NameListEditor(configvalue.CurrentInuseNamelist);
                    window2.ShowDialog();
                }
                else
                {
                    MessageBox.Show("密码错误，请返回尝试","错误",System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            else
            {
                FluentWindow window3 = new NameListEditor(configvalue.CurrentInuseNamelist);
                window3.ShowDialog();
            }
            
            
        }

        

        

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(configvalue.DianmingPassword == null)
            {
                FluentWindow window = new PasswordWindow(false, "请输入新密码");
                window.ShowDialog();
                MessageBox.Show("设置密码成功", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.RootNavi.Navigate(typeof(AutoDianmingPage));
                win.RootNavi.Navigate(typeof(Settings));

            }
            else
            {
                FluentWindow window = new PasswordWindow(false, "请输入旧密码");
                bool? result = window.ShowDialog();
                if (result == true)
                {
                    MessageBox.Show("修改密码成功", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    win.RootNavi.Navigate(typeof(AutoDianmingPage));
                    win.RootNavi.Navigate(typeof(Settings));
                }
                if (result == false)
                {
                    MessageBox.Show("密码错误，请返回尝试", "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }

            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogResult result2 = System.Windows.Forms.MessageBox.Show("确定要删除密码吗？此操作不可逆！！！", "警告", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);

            if (result2 == DialogResult.OK)
            {
                if(configvalue.DianmingPassword != null)
                {
                    FluentWindow window = new PasswordWindow(true, "请输入密码授权本次操作");
                    bool? result = window.ShowDialog();
                    if (result == true)
                    {
                        ConfigWriteOnlyValue.DianmingPassword = null;
                        ConfigWriteOnlyValue.LockOnSettings = false;
                        Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                        var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        win.RootNavi.Navigate(typeof(AutoDianmingPage));
                        win.RootNavi.Navigate(typeof(Settings));
                        
                    }
                    else if (result == false)
                    {
                        MessageBox.Show("密码错误，请返回尝试", "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
                else
                {
                    ConfigWriteOnlyValue.DianmingPassword = null;
                    ConfigWriteOnlyValue.LockOnSettings = false;
                    Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                    var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    win.RootNavi.Navigate(typeof(AutoDianmingPage));
                    win.RootNavi.Navigate(typeof(Settings));

                }


            }

                
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show("确定要删除配置文件吗？此操作不可逆！！！", "警告", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                if(configvalue.DianmingPassword != null)
                {
                    FluentWindow window = new PasswordWindow(true, "请输入密码授权本次操作");
                    bool? result2 = window.ShowDialog();
                    if (result2 == true)
                    {
                        ConfigHelper.ConfigInitalize();
                        //MessageBox.Show("操作成功，已删除配置项", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                        App.Restart();
                    }
                    else if (result2 == false)
                    {
                        MessageBox.Show("密码错误，操作已取消", "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
                else
                {
                    ConfigHelper.ConfigInitalize();
                    //MessageBox.Show("操作成功，已删除配置项", "提示", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    App.Restart();
                }
                
            }
        }

        private void LockOnSettings_Click(object sender, RoutedEventArgs e)
        {
            if(LockOnSettings.IsChecked == true)
            {
                ConfigWriteOnlyValue.LockOnSettings = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.RootNavi.Navigate(typeof(AutoDianmingPage));
            }
            else
            {
                

                FluentWindow window = new PasswordWindow(true, "请输入密码授权本次操作");
                bool? result2 = window.ShowDialog();
                if (result2 == true)
                {
                    ConfigWriteOnlyValue.LockOnSettings = false;
                    Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                }
                else if (result2 == false)
                {
                    LockOnSettings.IsChecked = true;
                    MessageBox.Show("密码错误，操作已取消", "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    
                }
            }
            
        }

        private void DefaultCostumSettings_Click(object sender, RoutedEventArgs e)
        {
            DialogResult result = System.Windows.Forms.MessageBox.Show("确定要将点名字体，字体颜色和点名音乐恢复默认值吗？此操作不可逆！！！", "警告", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                if(configvalue.DianmingPassword != null)
                {
                    FluentWindow window = new PasswordWindow(true, "请输入密码授权本次操作");
                    bool? result2 = window.ShowDialog();
                    if (result2 == true)
                    {

                        ConfigWriteOnlyValue.DianMingFont = null;
                        ConfigWriteOnlyValue.DianmingFontColor = null;
                        ConfigWriteOnlyValue.DianmingMusic = null;
                        Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                        var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        win.RootNavi.Navigate(typeof(AutoDianmingPage));
                        win.RootNavi.Navigate(typeof(Settings));
                    }
                    else if (result2 == false)
                    {

                        MessageBox.Show("密码错误，操作已取消", "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);

                    }
                }
                else
                {
                    ConfigWriteOnlyValue.DianMingFont = null;
                    ConfigWriteOnlyValue.DianmingFontColor = null;
                    ConfigWriteOnlyValue.DianmingMusic = null;
                    Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
                    App.Restart();
                }

                
            }

                
        }

        private void AutoInterval_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (!System.Text.RegularExpressions.Regex.IsMatch(tb.Text, @"^\d+$"))
            {
                tb.Text = System.Text.RegularExpressions.Regex.Replace(tb.Text, @"\D", "");
                if (string.IsNullOrEmpty(tb.Text))
                    tb.Text = "1";
                tb.CaretIndex = tb.Text.Length;
            }
        }

        private void ListNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (!System.Text.RegularExpressions.Regex.IsMatch(tb.Text, @"^\d+$"))
            {
                tb.Text = System.Text.RegularExpressions.Regex.Replace(tb.Text, @"\D", "");
                if (string.IsNullOrEmpty(tb.Text))
                    tb.Text = "1";
                tb.CaretIndex = tb.Text.Length;
            }
        }

        private void ListNum_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
            var tb = sender as TextBox;

            // 如果删除后长度小于1，阻止输入（只对可删除操作有效）
            
        }
        

        private void ListNum_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var tb = sender as TextBox;

            // 阻止删除键将文本清空
            if ((e.Key == Key.Back || e.Key == Key.Delete) && tb.Text.Length <= 1)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void AutoInterval_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var tb = sender as TextBox;

            // 阻止删除键将文本清空
            if ((e.Key == Key.Back || e.Key == Key.Delete) && tb.Text.Length <= 1)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void AutoInterval_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d+$");
            var tb = sender as TextBox;

            // 如果删除后长度小于1，阻止输入（只对可删除操作有效）
            
        }

        private void LaunchHoverBall_Click(object sender, RoutedEventArgs e)
        {
            if(LaunchHoverBall.IsChecked == true)
            {
                Window window1 = new HoverBall(configvalue.HoverballTopMost);
                window1.Show();
                ConfigWriteOnlyValue.Hoverball = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else
            {
                var win2 = System.Windows.Application.Current.Windows.OfType<HoverBall>().FirstOrDefault();
                win2.Close();
                ConfigWriteOnlyValue.Hoverball = false;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.RootNavi.Navigate(typeof(AutoDianmingPage));
            win.RootNavi.Navigate(typeof(Settings));
        }

        private void HoverBallTopButton_Click(object sender, RoutedEventArgs e)
        {
            if (HoverBallTopButton.IsChecked == true)
            {
                ConfigWriteOnlyValue.HoverballTopMost = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else
            {
                ConfigWriteOnlyValue.HoverballTopMost = false;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
        }

        private void ApplicationExittoTray_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationExittoTray.IsChecked == true)
            {
                ConfigWriteOnlyValue.IsApplicationMinimizetoTray = true;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
            else
            {
                ConfigWriteOnlyValue.IsApplicationMinimizetoTray = false;
                Json.WriteJson(App.ConfigPath, ConfigWriteOnlyValue);
            }
        }
    }

        //输入框主逻辑

    
}
