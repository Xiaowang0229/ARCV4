using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
        }
    }
}
