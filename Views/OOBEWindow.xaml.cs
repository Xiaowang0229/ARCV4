using ARCV4.Views.OOBE;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// OOBEWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OOBEWindow : FluentWindow
    {
        
        public OOBEWindow()
        {
            InitializeComponent();
            App.OOBEthisWindow = this;

            OOBEFrame.Navigate(new EulaPage());
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);


        }
    }
}
