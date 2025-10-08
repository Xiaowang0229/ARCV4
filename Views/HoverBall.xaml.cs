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
using System.Windows.Shapes;
using ToolLib.JsonLib;

namespace ARCV4.Views
{
    /// <summary>
    /// HoverBall.xaml 的交互逻辑
    /// </summary>
    public partial class HoverBall : Window
    {
        public Config configvalue;
        public HoverBall(bool IsTopMost)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) =>
            {
                if (e.ButtonState == MouseButtonState.Pressed)
                    this.DragMove();
            };
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            this.Topmost = IsTopMost;
            RootImage.Source = App.ConvertByteArrayToImageSource(RootResources.HoverNormal);
            this.Left = configvalue.HoverballLeft;
            this.Top = configvalue.HoverballTop;
        }

        private void WindowBase_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            RootImage.Source = App.ConvertByteArrayToImageSource(RootResources.HoverPress);
        }

        private void WindowBase_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            RootImage.Source = App.ConvertByteArrayToImageSource(RootResources.HoverNormal);
        }

        private void WindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var win = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            win.Show();
            win.WindowState = WindowState.Normal;
            win.Activate();
        }

        private void WindowBase_LocationChanged(object sender, EventArgs e)
        {
            configvalue.HoverballLeft = this.Left;
            configvalue.HoverballTop = this.Top;
            Json.WriteJson(App.ConfigPath, configvalue);
        }
    }
}
