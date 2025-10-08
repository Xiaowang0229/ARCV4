
using System.Diagnostics;
using ToolLib.DownloaderLib;
using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// UpdateWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateWindow : FluentWindow
    {
        CancellationTokenSource cts;
        public UpdateWindow()
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            RootImage2.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            ProductName.Text = App.ProgramVersion;
            
        }

        private async void FluentWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            string a = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestVersion");

            if (a == App.ProgramVersion)
            {
                ExitButton.IsEnabled = true;
                DescribeBlock.Text = "当前版本为最新版";
            }
            else
            {
                //System.Windows.MessageBox.Show(a,App.ProgramVersion);
                DescribeBlock.Text = "已检测到更新，正在获取更新数据……";
                string b = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestLog");
                string c = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestLink");
                DescribeBlock.Text = "最新版本：" + a;
                RootRun.Text = b;
                App.Latestlink = c;
                RootExpander.IsEnabled = true;
                RootExpander.IsExpanded = true;
                ExitButton.IsEnabled = true;
            }
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Process.Start(new ProcessStartInfo
            //{
            //FileName = App.Latestlink,
            //UseShellExecute = true
            //});

            string a = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestVersion");
            string b = @"Upgrade " + a  + ".exe";
            string c = await App.GetWebPageAsync("https://gitee.com/huamouren110/UpdateService/raw/main/ARCV4/LatestLink");
            RootTitle.ShowClose = false;
            RootTitle.ShowMinimize = false;
            UpdateNow.IsEnabled = false;
            ExitButton.IsEnabled = true;
            
            DownloadProgress.Value = 0;
            DownloadProgress.Visibility = System.Windows.Visibility.Visible;
            DownloadTextBlock.Visibility = System.Windows.Visibility.Visible;
            cts = new CancellationTokenSource();
            try
            {
                await Downloader.DownloadFileAsync(c, System.IO.Path.GetTempPath() + b, percent =>
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                DownloadProgress.Value = percent), cts.Token);

                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = System.IO.Path.GetTempPath() + b,
                    UseShellExecute = true,
                };
                Process.Start(startInfo);
                var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                win._trayManager.Dispose();
                Environment.Exit(0);
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("下载失败：" + ex.Message, "错误", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);

            }
        }

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            cts?.Cancel();
            this.Close();
        }
    }
}
