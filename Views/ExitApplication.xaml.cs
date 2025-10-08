using ARCV4.Views.MainPages;
using System.Windows;
using System.Windows.Input;
using ToolLib.JsonLib;
using Wpf.Ui.Controls;
using Wpf.Ui.Input;

using MessageBox = System.Windows.MessageBox;



namespace ARCV4.Views
{
    /// <summary>
    /// ExitApplication.xaml 的交互逻辑
    /// </summary>
    public partial class ExitApplication : FluentWindow
    {
        public Config ConfigValue;

        public ExitApplication(bool? IsMinimizeToTray)
        {
            InitializeComponent();
            
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            ConfigValue = Json.ReadJson<Config>(App.ConfigPath);
            if(IsMinimizeToTray == true)
            {
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.Hide();
            }
            else if(IsMinimizeToTray == false)
            {
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                Environment.Exit(0);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(Minimize.IsChecked == true)
            {
                if(Remember.IsChecked == true)
                {
                    ConfigValue.IsApplicationMinimizetoTray = true;
                    Json.WriteJson(App.ConfigPath, ConfigValue);
                }
                
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win.Hide();
                this.Close();
            }
            else if(Quit.IsChecked == true)
            {
                if(Remember.IsChecked == true)
                {
                    ConfigValue.IsApplicationMinimizetoTray = false;
                    Json.WriteJson(App.ConfigPath, ConfigValue);
                }
                
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win._trayManager.Dispose();
                Environment.Exit(0);
                this.Close();
            }
            else if(Minimize.IsChecked == false && Quit.IsChecked == false)
            {
                MessageBox.Show("必须选择一种后再退出程序","错误",System.Windows.MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }


    }
}

