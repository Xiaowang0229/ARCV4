using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ToolLib.JsonLib;

namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// ListNamePage.xaml 的交互逻辑
    /// </summary>
    public partial class ListNamePage : Page
    {
        public Random random = new Random();
        public Config configvalue;
        public Namelist namelistvalue;
        public List<string> TempNamelist = new List<string>();

        public ListNamePage()
        {
            InitializeComponent();
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            namelistvalue = Json.ReadJson<Namelist>(configvalue.CurrentInuseNamelist);
            NamelistPath.Text += configvalue.CurrentInuseNamelist;
            TempNamelist = namelistvalue.names.ToList();
            App.MainpageStatus = 3;
            if (configvalue.DianMingFont != null)
            {
                roottextbox.FontFamily = configvalue.DianMingFont;
            }

            if (configvalue.DianmingFontColor != null)
            {
                roottextbox.Foreground = configvalue.DianmingFontColor;
            }
            // Page 加载时绑定父窗口键盘事件
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.CurrentPage = this;

        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            roottextbox.Text = "";
            for(int i = 1;i<=configvalue.ListDianmingNumbers; i++)
            {
                
                int randomNumber = random.Next(0, TempNamelist.Count);
                string temp = TempNamelist[randomNumber];
                roottextbox.Text += temp + "\r\n";
                TempNamelist.Remove(temp);
                if (TempNamelist.Count == 0)
                {
                    TempNamelist = namelistvalue.names.ToList();
                    TempNamelist.Remove(temp);
                }
            }
        }

        public void SpaceKeyDown()
        {
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            App.IsDianmingStatus = false;
            roottextbox.Text = "";
            for (int i = 1; i <= configvalue.ListDianmingNumbers; i++)
            {
                int randomNumber = random.Next(0, namelistvalue.names.Length);
                roottextbox.Text += namelistvalue.names[randomNumber] + "\r\n";
            }
            win.UnLockAllNavigationItems();
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
    }
}
