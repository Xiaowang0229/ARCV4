using ARCV4.Views.MainPages;
using ARCV4.YourNamespace;
using KeyBoard;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using ToolLib.JsonLib;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ARCV4.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public Config configvalue;
        public System.Windows.Controls.Page CurrentPage;
        public static bool IsPaneOpened = true;
        public TrayManager _trayManager;


        public MainWindow()
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            this.Loaded += OnLoaded;
            this.Closed += (s, e) => GlobalKeyboardHook.Stop();
            _trayManager = new TrayManager();


        }

        private void FluentWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {



            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += ThemeTimerTickEvent;
            timer.Start();
            RootTitle.Title = RootTitle.Title + App.ProgramVersion;
            RootNavi.Navigate(typeof(AutoDianmingPage));
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
            
        }

        private void ThemeTimerTickEvent(object sender, EventArgs e)
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
            
        }

        public void LockAllNavigationItems()
        {
            NavItem1.IsEnabled = false;
            NavItem2.IsEnabled = false;
            NavItem3.IsEnabled = false;
            NavItem4.IsEnabled = false;
            NavItem5.IsEnabled = false;
        }

        public void UnLockAllNavigationItems()
        {
            NavItem1.IsEnabled = true;
            NavItem2.IsEnabled = true;
            NavItem3.IsEnabled = true;
            NavItem4.IsEnabled = true;
            NavItem5.IsEnabled = true;
        }

        private void NavItem1_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NavItem2_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void NavItem3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }

        private void NavItem4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void NavItem5_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            GlobalKeyboardHook.Start(handle);

            GlobalKeyboardHook.KeyDown += key =>
            {
                if (key == Keys.Space)
                {
                    Dispatcher.Invoke(() =>
                    {
                        if(App.MainpageStatus == 1)
                        {
                             var page = System.Windows.Application.Current.Windows
                            .OfType<MainWindow>() // 遍历所有窗口
                            .Select(w => App.FindVisualChild<AutoDianmingPage>(w)) // 在窗口里查找 Page
                            .FirstOrDefault(p => p != null);

                            page.SpaceKeyDown();
                        }
                        else if(App.MainpageStatus == 2)
                        {
                            var page = System.Windows.Application.Current.Windows
                            .OfType<MainWindow>() // 遍历所有窗口
                            .Select(w => App.FindVisualChild<ImmediatelyNamePage>(w)) // 在窗口里查找 Page
                            .FirstOrDefault(p => p != null);

                            page.SpaceKeyDown();
                        }
                        else if(App.MainpageStatus == 3)
                        {
                            var page = System.Windows.Application.Current.Windows
                            .OfType<MainWindow>() // 遍历所有窗口
                            .Select(w => App.FindVisualChild<ListNamePage>(w)) // 在窗口里查找 Page
                            .FirstOrDefault(p => p != null);

                            page.SpaceKeyDown();
                        }
                    });
                }
            };
        }

        private void RootNavi_PaneOpened(NavigationView sender, RoutedEventArgs args)
        {
            IsPaneOpened = true;
            if(App.MainpageStatus == 11)
            {
                var page = System.Windows.Application.Current.Windows
                            .OfType<MainWindow>() // 遍历所有窗口
                            .Select(w => App.FindVisualChild<About>(w)) // 在窗口里查找 Page
                            .FirstOrDefault(p => p != null);
                page.CloseRootPane();
            }
        }

        private void RootNavi_PaneClosed(NavigationView sender, RoutedEventArgs args)
        {
            IsPaneOpened = false;
            if (App.MainpageStatus == 11)
            {
                var page = System.Windows.Application.Current.Windows
                            .OfType<MainWindow>() // 遍历所有窗口
                            .Select(w => App.FindVisualChild<About>(w)) // 在窗口里查找 Page
                            .FirstOrDefault(p => p != null);
                page.OpenRootPane();
            }
        }

        private void FluentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            if (configvalue.IsApplicationMinimizetoTray == true)
            {
                e.Cancel = true;
                this.Hide();
            }
            else if(configvalue.IsApplicationMinimizetoTray == false)
            {
                _trayManager.Dispose();
                Environment.Exit(0);
            }
            else
            {
                e.Cancel = true;
                FluentWindow window = new ExitApplication(null);
                window.ShowDialog();
            }
        }
    }
}

