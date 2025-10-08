using System.Runtime.CompilerServices;
using System.Windows;
using ToolLib.JsonLib;
using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// PasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordWindow : FluentWindow
    {
        public Config configvalue;
        public bool isvalidateorcreatepassword;
        public string Describepublic;
        public PasswordWindow(bool IsValidateorCreatePassword,string Describetion)
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            isvalidateorcreatepassword = IsValidateorCreatePassword;
            Describepublic = Describetion;
            if (isvalidateorcreatepassword && configvalue.DianmingPassword == null)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(isvalidateorcreatepassword)//如果真则进入验证
            {
                if(RootPassWordBox.Password == configvalue.DianmingPassword)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                
                else
                {
                    this.DialogResult = false;
                    this.Close();
                }
            }
            else
            {
                if(configvalue.DianmingPassword == null)
                {
                    configvalue.DianmingPassword = RootPassWordBox.Password;
                    Json.WriteJson(App.ConfigPath, configvalue);
                    this.DialogResult = true;
                    this.Close();
                }
                else//如果假则创建
                {
                    if (RootPassWordBox.Password == configvalue.DianmingPassword)
                    {
                        configvalue.DianmingPassword = null;
                        Json.WriteJson(App.ConfigPath, configvalue);
                        FluentWindow window = new PasswordWindow(false,"请输入新密码");
                        window.ShowDialog();
                        this.DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        
                        this.DialogResult = false;
                        this.Close();
                    }
                }
                    
            }
        }

        private void FluentWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            RootTextBox.Text = Describepublic; 
            

        }
    }
}
