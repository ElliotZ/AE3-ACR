
using AEAssist.Helper;
using AEAssist.IO;
using ElliotZ.Common.ModernJobViewFramework;

namespace ElliotZ.Rpr;

public class RprSettings
{
    public static RprSettings Instance;

    /// <summary>
    /// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
    /// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
    /// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
    /// </summary>

    #region 标准模板代码 可以直接复制后改掉类名即可

    private static string path;

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, $"{nameof(RprSettings)}.json");
        if (!File.Exists(path))
        {
            Instance = new RprSettings();
            Instance.Save();
            return;
        }

        try
        {
            Instance = JsonHelper.FromJson<RprSettings>(File.ReadAllText(path));
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

    // General Settings
    public int AnimLock = 550;
    public bool ForceCast = false;
    public bool ForceNextSlotsOnHKs = false;
    public bool NoPosDrawInTN = false;
    public int PosDrawStyle = 2;
    public bool RestoreQtSet = true;
    //public bool SmartAOE = true;
    public bool CommandWindowOpen = true;
    public bool ShowToast = false;
    public bool Debug = false;

    // Roulette Utility Settings
    public bool NoBurst = true;
    public bool PullingNoBurst = true;
    public bool AutoCrest = false;
    public float CrestPercent = 0.8f;
    public bool AutoSecondWind = true;
    public float SecondWindPercent = 0.8f;
    public bool AutoBloodBath = true;
    public float BloodBathPercent = 0.6f;
    public bool AutoFeint = false;
    public float MinMobHpPercent = 0.1f;
    public float ConcentrationThreshold = 0.75f;
    public int minTTK = 15;
    public bool HandleStopMechs = true;

    // Opener Settings
    public bool TripleWeavePot = false;
    public int PrepullCastTimeHarpe = 1700;
    public bool PrepullSprint = true;
    public bool PrepullIngress = true;

    //public bool AutoUpdateTimeLines = true;
    //public bool TimeLinesDebug = false;

    // QT设置存档
    public Dictionary<String, bool> QtStates = [];
    public JobViewSave JobViewSave = new()
    {
        CurrentTheme = ModernTheme.ThemePreset.RPR,
        QtLineCount = 3,
        QtUnVisibleList = ["挥割/爪", "暴食", "灵魂割", "祭牲",]
    };
}
