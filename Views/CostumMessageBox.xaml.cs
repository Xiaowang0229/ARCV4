using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// CostumMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class CostumMessageBox : FluentWindow
    {
        public CostumMessageBox(string WindowTitle,string WindowContent)
        {
            InitializeComponent();
            RootTitleBar.Title = WindowTitle;
            WindowBase.Title = WindowTitle;
            run.Text = WindowContent;
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
