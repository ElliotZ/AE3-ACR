using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;

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
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }

    #endregion

    public JobViewSave JobViewSave = new()
    {
        QtLineCount = 3,
        QtUnVisibleList = ["挥割/爪", "暴食", "灵魂割", "真北", "播魂种", "祭牲"]
    }; // QT设置存档

    public bool TripleWeavePot = false;
    public int PrepullCastTimeHarpe = 1700;
    public int AnimLock = 550;
    public bool ForceCast = false;
    public bool Debug = false;
    //public bool AutoUpdateTimeLines = true;
    //public bool TimeLinesDebug = false;
}
