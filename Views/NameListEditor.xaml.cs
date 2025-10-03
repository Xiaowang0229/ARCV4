using System.Windows.Documents;
using ToolLib.JsonLib;
using Wpf.Ui.Controls;

namespace ARCV4.Views
{
    /// <summary>
    /// CostumMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class NameListEditor : FluentWindow
    {
        public class Namelist
        {
            public string[] names { get; set; }
        }
        public Namelist namevalue;
        private string currentnamelistlocationpublic;
        public NameListEditor(string Currentnamelistlocation)
        {
            InitializeComponent();
            RootIcon.Source = App.ConvertByteArrayToImageSource(RootResources.IconPNG);
            //RootTitleBar.Title = RootTitleBar.Title + " - " + Currentnamelistlocation;
            WindowBase.Title = WindowBase.Title + " - " + Currentnamelistlocation;
            currentnamelistlocationpublic = Currentnamelistlocation;
            namevalue = Json.ReadJson<Namelist>(Currentnamelistlocation);
            foreach(string i in namevalue.names)
            {
                run.Text = run.Text + i + "\r\n";
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string text = new TextRange(
            RootTextBox.Document.ContentStart,
            RootTextBox.Document.ContentEnd
            ).Text;

            string[] lines = text.Split(
                new[] { "\r\n", "\n", "\r" },
                StringSplitOptions.RemoveEmptyEntries
            );


            namevalue = new Namelist
            {
                names = lines
            };
            Json.WriteJson(currentnamelistlocationpublic, namevalue);
            this.Close();
        }
    }
}
