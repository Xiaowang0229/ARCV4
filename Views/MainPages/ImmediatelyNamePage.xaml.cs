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
using System.Windows.Threading;
using ToolLib.JsonLib;

namespace ARCV4.Views.MainPages
{
    /// <summary>
    /// ImmediatelyNamePage.xaml 的交互逻辑
    /// </summary>
    public partial class ImmediatelyNamePage : Page
    {
        public Random random = new Random();
        public Config configvalue;
        public Namelist namelistvalue;
        public List<string> TempNamelist = new List<string>();
        public ImmediatelyNamePage()
        {
            InitializeComponent();
            App.MainpageStatus = 2;
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            namelistvalue = Json.ReadJson<Namelist>(configvalue.CurrentInuseNamelist);
            NamelistPath.Text += configvalue.CurrentInuseNamelist;
            TempNamelist = namelistvalue.names.ToList();
            if (configvalue.DianMingFont != null)
            {
                NameBlock.FontFamily = configvalue.DianMingFont;
            }

            if (configvalue.DianmingFontColor != null)
            {
                NameBlock.Foreground = configvalue.DianmingFontColor;
            }
            // Page 加载时绑定父窗口键盘事件
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.CurrentPage = this;


        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            int randomNumber = random.Next(0, TempNamelist.Count);
            NameBlock.Text = TempNamelist[randomNumber];
            TempNamelist.Remove(NameBlock.Text);
            if (TempNamelist.Count == 0)
            {
                TempNamelist = namelistvalue.names.ToList();
                TempNamelist.Remove(NameBlock.Text);
            }
        }

        public void SpaceKeyDown()
        {
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            App.IsDianmingStatus = false;
            int randomNumber = random.Next(0, namelistvalue.names.Length);
            NameBlock.Text = namelistvalue.names[randomNumber];
            win.UnLockAllNavigationItems();
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            
        }
    }
}
