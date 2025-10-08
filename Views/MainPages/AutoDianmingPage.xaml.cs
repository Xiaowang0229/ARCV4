using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ToolLib.JsonLib;
using ToolLib.KeyboardHookLib;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// AutoDianmingPage.xaml 的交互逻辑
    /// </summary>
    public partial class AutoDianmingPage : Page
    {
        public Random random = new Random();
        public Config configvalue;
        private DispatcherTimer timer = new DispatcherTimer();
        
        public Namelist namelistvalue;        //Temppool
        public List<string> TempNamelist = new List<string>();
        
        

        public AutoDianmingPage()
        {
            InitializeComponent();
            App.MainpageStatus = 1;
            configvalue = Json.ReadJson<Config>(App.ConfigPath);

            namelistvalue = Json.ReadJson<Namelist>(configvalue.CurrentInuseNamelist);
            TempNamelist = namelistvalue.names.ToList();
            // Page 加载时绑定父窗口键盘事件
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.CurrentPage = this;

        }

        private async void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(App.IsDianmingStatus == false)
            {
                App.IsDianmingStatus = true;
                StartStopButton.Content = "停止点名";
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = false;
                win.LockAllNavigationItems();
                
                timer.Start();
                try
                {
                    if (configvalue.DianmingMusic != null)
                    {
                        await AudioPlayer.PlayLoopAsync(configvalue.DianmingMusic, "start");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"错误：{ex.Message}","Error");
                }

                
                


            }
            else if (App.IsDianmingStatus == true)
            {
                try
                {
                    if (configvalue.DianmingMusic != null)
                    {
                        await AudioPlayer.PlayLoopAsync(configvalue.DianmingMusic, "stop");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误：{ex.Message}", "Error");
                }
                timer.Stop();
                App.IsDianmingStatus = false;
                StartStopButton.Content = "开始点名";
                //缓冲池主逻辑
                int randomNumber = random.Next(0, TempNamelist.Count);
                NameBlock.Text = TempNamelist[randomNumber];
                TempNamelist.Remove(NameBlock.Text);
                if (TempNamelist.Count == 0)
                {
                    TempNamelist = namelistvalue.names.ToList();
                    TempNamelist.Remove(NameBlock.Text);
                }
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = true;
                win.UnLockAllNavigationItems();
            }
            else
            {
                timer.Stop();
                App.IsDianmingStatus = false;
                StartStopButton.Content = "开始点名";
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = true;
                win.UnLockAllNavigationItems();
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.IsDianmingStatus = false;
            timer.Interval = TimeSpan.FromMilliseconds(configvalue.AutoDianmingintervaltick);
            timer.Tick += TimerChangeName;
            timer.Stop();
            NameBlock.Text = "请开始点名";
            NamelistPath.Text += configvalue.CurrentInuseNamelist;

            if(configvalue.DianMingFont != null)
            {
                NameBlock.FontFamily = configvalue.DianMingFont;
            }

            if (configvalue.DianmingFontColor != null)
            {
                NameBlock.Foreground = configvalue.DianmingFontColor;
            }
            
        }

        private void TimerChangeName(object sender,EventArgs e)
        {
            int randomNumber = random.Next(0, namelistvalue.names.Length);
            NameBlock.Text = namelistvalue.names[randomNumber];
            
            

        }

        public async void SpaceKeyDown()
        {
            if (App.IsDianmingStatus == false)
            {
                App.IsDianmingStatus = true;
                StartStopButton.Content = "停止点名";
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = false;
                win.LockAllNavigationItems();

                timer.Start();
                try
                {
                    if (configvalue.DianmingMusic != null)
                    {
                        await AudioPlayer.PlayLoopAsync(configvalue.DianmingMusic, "start");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误：{ex.Message}", "Error");
                }





            }
            else if (App.IsDianmingStatus == true)
            {
                try
                {
                    if (configvalue.DianmingMusic != null)
                    {
                        await AudioPlayer.PlayLoopAsync(configvalue.DianmingMusic, "stop");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"错误：{ex.Message}", "Error");
                }
                timer.Stop();
                App.IsDianmingStatus = false;
                StartStopButton.Content = "开始点名";
                //缓冲池主逻辑
                int randomNumber = random.Next(0, TempNamelist.Count);
                NameBlock.Text = TempNamelist[randomNumber];
                TempNamelist.Remove(NameBlock.Text);
                if (TempNamelist.Count == 0)
                {
                    TempNamelist = namelistvalue.names.ToList();
                    TempNamelist.Remove(NameBlock.Text);
                }
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = true;
                win.UnLockAllNavigationItems();
            }
            else
            {
                timer.Stop();
                App.IsDianmingStatus = false;
                StartStopButton.Content = "开始点名";
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                //win.RootNavi.IsEnabled = true;
                win.UnLockAllNavigationItems();
            }
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
    }
}
