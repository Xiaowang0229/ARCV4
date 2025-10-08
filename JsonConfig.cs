using ToolLib.JsonLib;
using Brush = System.Windows.Media.Brush;

namespace ARCV4
{
/*  //JsonLib使用时：
    //定义一个类
    public class JsonConfig
    {
        //一个项
        public string Text { get; set; }
    }

    //变量
    public JsonConfig GlobalConfig;

    //初始化变量(程序集内)
    GlobalConfig =new JsonConfig
    {
        Text = "Hello world!"
    }

    //写
    Json.WriteJson(@"C:\456.json", GlobalConfig);



//==========================

    // 读
    //把读到的东西给变量
    GlobalConfig = Json.ReadJson<JsonConfig>(@"C:\456.json");

    //取出数据
    Console.WriteLine(GlobalConfig.Text);
*/
    public class Config
    {
        public bool OOBEStatus { get; set; }
        public List<string> namelistlocations { get; set; }
        public bool IsStartUpCheckUpdate { get; set; }
        public string AppuseTheme { get; set; }
        public int AutoDianmingintervaltick { get; set; }
        public int ListDianmingNumbers { get; set; }
        public string CurrentInuseNamelist { get; set; }
        public System.Windows.Media.FontFamily DianMingFont { get; set; }
        public Brush DianmingFontColor { get; set; }
        public string DianmingMusic { get; set; }
        public string DianmingPassword { get; set; }
        public bool LockOnSettings { get; set; }
        public bool? IsApplicationMinimizetoTray { get; set; }
        public bool Hoverball { get; set; }
        public bool HoverballTopMost { get; set; }
        public double HoverballLeft { get; set; }
        public double HoverballTop { get; set; }
    }

    public class Namelist
    {
        public string[] names { get; set; }
    }
        
    public class ConfigHelper
    {
        public static void ConfigInitalize()
        {
            Config configvalue;
            configvalue = new Config
            {
                OOBEStatus = false,
                namelistlocations = [],
                IsStartUpCheckUpdate = true,
                AppuseTheme = "",
                AutoDianmingintervaltick = 1,
                ListDianmingNumbers = 5,
                CurrentInuseNamelist = "",
                DianMingFont = null,
                DianmingFontColor = null,
                DianmingMusic = null,
                DianmingPassword = null,
                LockOnSettings = false,
                IsApplicationMinimizetoTray = null,
                Hoverball = false,
                HoverballTopMost = true,
                HoverballLeft = 0,
                HoverballTop = 0
            };

            Json.WriteJson(App.ConfigPath, configvalue);
        }
    }

    

}
