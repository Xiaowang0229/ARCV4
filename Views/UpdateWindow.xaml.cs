
using System.Diagnostics;
using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// UpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : FluentWindow
    {
        public UpdateWindow()
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            RootImage2.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            ProductName.Text = App.ProgramVersion;
            
        }

        private async void FluentWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string a = await App.GetWebPageAsync("https://raw.bgithub.xyz/isHuaMouRen/UpdateService/refs/heads/main/ARCV4/LatestVersion");

            if (a == App.ProgramVersion)
            {
                ExitButton.IsEnabled = true;
                DescribeBlock.Text = "当前版本为最新版";
            }
            else
            {
                //System.Windows.MessageBox.Show(a,App.ProgramVersion);
                DescribeBlock.Text = "已检测到更新，正在获取更新数据……";
                string b = await App.GetWebPageAsync("https://raw.bgithub.xyz/isHuaMouRen/UpdateService/refs/heads/main/ARCV4/LatestLog");
                string c = await App.GetWebPageAsync("https://raw.bgithub.xyz/isHuaMouRen/UpdateService/refs/heads/main/ARCV4/LatestLink");
                DescribeBlock.Text = "最新版本：" + a;
                RootRun.Text = b;
                App.Latestlink = c;
                RootExpander.IsEnabled = true;
                RootExpander.IsExpanded = true;
                ExitButton.IsEnabled = true;
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = App.Latestlink,
                UseShellExecute = true
            });
        }

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
