using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;

namespace EZACR_Offline.Gnb;

public class GnbSettings
{
    public static GnbSettings Instance;

    #region 标准模板代码 可以直接复制后改掉类名即可

    private static string path;

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, $"{nameof(GnbSettings)}.json");
        if (!File.Exists(path))
        {
            Instance = new GnbSettings();
            Instance.Save();
            return;
        }

        try
        {
            Instance = JsonHelper.FromJson<GnbSettings>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Instance = new();
            LogHelper.Error(e.ToString());
        }
    }

    public void Save()
    {
        _ = Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }

    #endregion

    public bool UsePeloton = true;

    public bool UseAOE = true;

    public JobViewSave JobViewSave = new()
    {
        QtLineCount = 3,
        QtUnVisibleList = []
    };

    public bool UsePotion = false;
    public bool CommandWindowOpen = true;
    public bool HandleStopMechs = true;
    public bool NoBurst = true;
    public float MinMobHpPercent = 0.1f;
    public int minTTK = 10;
    public bool UseLion = true;
    public bool Debug = false;
    public bool 上传 = true;

    public int opener = 0;
    public string targetName = "";
    public float 铁壁阈值 = 0.82f;
    public float 伪装阈值 = 0.79f;
    public float 超火流星阈值 = 0.15f;
    public float 极光阈值 = 0.8f;
    public float 星云阈值 = 0.75f;
    public float 刚玉之心阈值 = 0.8f;
    public bool 无情延后 = true;
    public int Time = 400;
    public int 铁壁Time = 2000;
    public int 大星云Time = 1000;
    public int TPTime = 50;
    public int 攻击距离 = 3;
    public int 闪雷使用距离 = 7;
    public float Boss死刑阈值 = 0.4f;
    public float 自动拉怪停止时间 = 3f;
    public float 粗分斩距离 = 10f;
    public bool 自动拉怪 = true;
    public bool 起手给MT刚玉 = false;
    public bool 倒计时开铁壁 = false;
    public bool 倒计时开大星云 = false;
    public bool 倒计时自动盾姿 = true;
    public bool 倒计时是否ST关盾姿 = true;
    public int 最多减伤数目 = 2;

    //private JobViewSave 职业视图保存1 = new JobViewSave
    //{
    //    MainColor = new Vector4(8f / 51f, 0.6784314f, 14f / 51f, 0.8f)
    //};
    public Dictionary<string, object> StyleSetting = [];

    //public JobViewSave 职业视图保存
    //{
    //    get
    //    {
    //        return 职业视图保存1;
    //    }
    //    set
    //    {
    //        职业视图保存1 = value;
    //    }
    //}

    public string ACRMode { get; set; }

    public Dictionary<String, bool> QtStates = [];

    private GnbSettings()
    {
        ACRMode = "Normal";
    }
}
