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
        
        public OOBEWindow(bool isCreateNewNameList = false)
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);

            if (isCreateNewNameList == false)
            {
                OOBEFrame.Navigate(new EulaPage());
            }

            if (isCreateNewNameList == true)
            {
                OOBEFrame.Navigate(new NamelistPage());
                WindowBase.Title = "新建名单";
                BaseTitlebar.Title = "新建名单";
            }
            


        }
    }
}
