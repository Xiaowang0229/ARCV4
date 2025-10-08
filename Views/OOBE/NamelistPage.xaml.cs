using System.Text;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using ToolLib.IniLib;
using ToolLib.JsonLib;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;
using FileDialog = System.Windows.Forms.FileDialog;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;


namespace ARCV4.Views.OOBE
{
    /// <summary>
    /// NamelistPage.xaml 的交互逻辑
    /// </summary>
    public partial class NamelistPage : Page
    {
        public static string filename;
        public Config configvalue;

        public Config ConfigReadOnlyValue;
        public NamelistPage()
        {
            InitializeComponent();
            var appTheme = Wpf.Ui.Appearance.ApplicationThemeManager.GetAppTheme();
            if (appTheme == ApplicationTheme.Dark)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#FFFFFF");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
            else if(appTheme == ApplicationTheme.Light)
            {
                var brush = (System.Windows.Media.Brush)new BrushConverter().ConvertFromString("#000000");
                tb01.Foreground = brush;
                tb02.Foreground = brush;
            }
            configvalue = Json.ReadJson<Config>(App.ConfigPath);
            ConfigReadOnlyValue = Json.ReadJson<Config>(App.ConfigPath);
        }

        
        public Namelist namevalue;
        public class Namelist2
        {
            public string[] names { get; set; }
        }
        public Namelist2 namevaluenew;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Finally.IsEnabled = false;
            Rich123.IsReadOnly = true;
            // 创建 SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // 设置默认文件扩展名
            saveFileDialog.DefaultExt = ".json";

            // 设置可选文件类型
            saveFileDialog.Filter = "用于储存名单的 Json 文件 (*.json)|*.json";
            saveFileDialog.Title = "请确定名单储存位置以及名单名字";
            // 打开对话框
            bool? result = saveFileDialog.ShowDialog();

            if (result == true && saveFileDialog.FileName != App.ConfigPath)
            {
                filename = saveFileDialog.FileName;
                NameEditor.Text = "";
                Rich123.IsReadOnly = false;
                Finally.IsEnabled = true;
                Rich123.Focus();
            }
            else if (saveFileDialog.FileName == App.ConfigPath)
            {
                MessageBox.Show("不可将配置项作为名单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void Finally_Click(object sender, RoutedEventArgs e)
        {
            if(NameEditor.Text == "" || NameEditor.Text == null)
            {
                MessageBox.Show("名单不可置空！","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                string text = new TextRange(
            Rich123.Document.ContentStart,
            Rich123.Document.ContentEnd
            ).Text;

                string[] lines = text.Split(
                    new[] { "\r\n", "\n", "\r" },
                    StringSplitOptions.RemoveEmptyEntries
                );


                namevalue = new Namelist
                {
                    names = lines
                };
                Json.WriteJson(filename, namevalue);
                configvalue.namelistlocations.Add(filename);
                configvalue.CurrentInuseNamelist = filename;
                configvalue.OOBEStatus = true;
                Json.WriteJson(App.ConfigPath, configvalue);
                if (PasswordBox.IsChecked == true)
                {
                    FluentWindow window = new PasswordWindow(false, "请设置新密码");
                    window.ShowDialog();
                }



                App.Restart();
            }

            


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Rich123.IsReadOnly = true;
            Finally.IsEnabled = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = App.ProgramPath;
            openFileDialog.Filter = "旧版本的名单文件 (namelist.ini)|namelist.ini|旧版本的名单文件 (teamlist.ini)|teamlist.ini";
            openFileDialog.Title = "请选择旧版本的名单位置";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string oldnamelistpath = openFileDialog.FileName;


                // 创建 SaveFileDialog
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                // 设置默认文件扩展名
                saveFileDialog.DefaultExt = ".json";

                // 设置可选文件类型
                saveFileDialog.Filter = "用于储存名单的 Json 文件 (*.json)|*.json";
                saveFileDialog.Title = "新名单储存位置以及新名单名字";
                // 打开对话框
                bool? result = saveFileDialog.ShowDialog();

                if (result == true && saveFileDialog.FileName != App.ConfigPath)
                {
                    NameEditor.Text = "";
                    filename = saveFileDialog.FileName;
                    
                    try
                    {
                        App.ConvertAnsiFileToUtf8InPlace(oldnamelistpath);
                        for (int i = 1; i <= int.Parse(Ini.ReadIni(oldnamelistpath, "info", "num")); i++)
                        {
                            
                            NameEditor.Text = NameEditor.Text + Ini.ReadIni(oldnamelistpath, "namelist", i.ToString()) + "\r\n";
                        }
                        Rich123.IsReadOnly = false;
                        Finally.IsEnabled = true;
                        Rich123.Focus();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"操作已取消，添加名单时发生错误:{ex.Message}");
                    }
                    
                }
                else if (saveFileDialog.FileName == App.ConfigPath)
                {
                    MessageBox.Show("不可将配置项作为名单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
                

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Rich123.IsReadOnly = true;
            Finally.IsEnabled = false;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = App.ProgramPath;
            openFileDialog.Filter = "4.0.0.0版本的名单文件 (*.json)|*.json";
            openFileDialog.Title = "请选择未导入的新版本名单位置";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string namelistpath = openFileDialog.FileName;
                if (namelistpath != App.ConfigPath && openFileDialog.FileName != App.ConfigPath)
                {




                    NameEditor.Text = "";
                    namevaluenew = Json.ReadJson<Namelist2>(namelistpath);
                        foreach (string i in namevaluenew.names)
                        {
                            NameEditor.Text = NameEditor.Text + i + "\r\n";
                        }
                    filename = openFileDialog.FileName;
                        Rich123.IsReadOnly = false;
                        Finally.IsEnabled = true;
                        Rich123.Focus();
                    
                    
                }
                else if (openFileDialog.FileName == App.ConfigPath)
                {
                    MessageBox.Show("不可将配置项作为名单", "错误", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }



            }
            
        }
    }
}
