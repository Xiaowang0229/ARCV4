
using System.Diagnostics;
using System.Security.Policy;
using System.Windows.Controls;
using Wpf.Ui.Controls;


namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();
            App.MainpageStatus = 11;
            ProductName.Text = "自动点名器 " + App.ProgramVersion;
            RootImage.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            RootRun.Text = App.Versionlog;
        }

        private void BugReport_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Xiaowang0229/ARCV4/issues/new",
                UseShellExecute = true
            });
        }

        private void ChkUpd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FluentWindow window = new UpdateWindow();
            window.ShowDialog();
            
        }

        private void UserAgreement_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FluentWindow window = new CostumMessageBox("用户协议", "软件许可协议\r\n\r\n重要提示\r\n在安装、复制或使用本软件前，请仔细阅读本协议条款。一旦您安装、复制或使用本软件，即表示您已接受本协议条款的约束。如果您不同意本协议条款，请不要安装、复制或使用本软件。\r\n著作权人：寒星（智名）开发组 版权所有\r\n生效日期：自2025/10/01起\r\n\r\n一、定义\r\n1.1 \"软件\"指本\"自动点名器\"软件程序及其相关文档、更新版本。\r\n1.2 \"用户\"指安装、复制或使用本软件的个人或实体。\r\n\r\n二、许可授予\r\n2.1 本软件著作权人授予用户一项非独占、不可转让的有限使用许可。\r\n2.2 用户可在单一计算机上安装和使用本软件。\r\n2.3 本软件仅供个人或内部使用，不得用于商业用途。\r\n\r\n三、限制条款\r\n3.1 用户不得对本软件进行反向工程、反编译、反汇编或以其他方式尝试获取软件源代码。\r\n3.2 用户不得修改、租赁、出借、出售、分发本软件或其任何部分。\r\n3.3 用户不得移除本软件中的任何版权标识、商标或其他专有声明。\r\n\r\n四、知识产权\r\n4.1 本软件及其所有副本的知识产权归著作权人（寒星）所有。\r\n4.2 本协议不转让任何软件的知识产权给用户。\r\n\r\n五、免责声明\r\n5.1 本软件按\"现状\"提供，不提供任何明示或暗示的担保。\r\n5.2 在任何情况下，著作权人不承担因使用或不能使用本软件所发生的任何损害赔偿责任。\r\n\r\n六、隐私条款\r\n6.1 本软件不会收集、存储或传输用户的个人隐私数据。\r\n6.2 软件运行过程中产生的临时数据仅存储在用户本地设备中。\r\n\r\n七、协议终止\r\n7.1 如用户违反本协议任何条款，本协议将自动终止。\r\n7.2 协议终止后，用户必须立即销毁本软件的所有副本。\r\n\r\n八、适用法律\r\n8.1 本协议受中华人民共和国法律管辖。\r\n8.2 因本协议引起的任何争议，应通过友好协商解决；协商不成的，提交著作权人所在地有管辖权的人民法院诉讼解决。\r\n\r\n九、完整协议\r\n9.1 本协议构成用户与著作权人就软件使用所达成的完整协议。\r\n");
            window.ShowDialog();
        }

        public void OpenRootPane()
        {
            RootExpander.Width = 700;
        }

        public void CloseRootPane()
        {
            RootExpander.Width = 570;
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(MainWindow.IsPaneOpened)
            {
                RootExpander.Width = 570;
            }
            else
            {
                RootExpander.Width = 700;
            }
        }
    }
}
