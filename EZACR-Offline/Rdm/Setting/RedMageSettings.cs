using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Rdm.Setting;

/// <summary>
/// 配置文件适合放一些一般不会在战斗中随时调整的开关数据
/// 如果一些开关需要在战斗中调整 或者提供给时间轴操作 那就用QT
/// 非开关类型的配置都放配置里 比如诗人绝峰能量配置
/// </summary>
public class RedMageSettings
{
    public static RedMageSettings Instance;
    public bool test;
    #region 标准模板代码 可以直接复制后改掉类名即可
    private static string path;
    private static string logpath;
    public bool 留一层;
    public bool 自动昏乱;
    public bool 拉人;
    public bool 范围拉人;
    public bool 日记开关;
    public int 复活时间 = 2;
    public string 导随记录文件 { get; set; } = "D:\\ff14act\\Plugins\\RouletteRecorder\\数据.csv"; // 默认值

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, nameof(RedMageSettings) + ".json");
        if (!File.Exists(path))
        {
            Instance = new RedMageSettings();
            Instance.Save();
            return;
        }
        try
        {
            Instance = JsonHelper.FromJson<RedMageSettings>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Instance = new();
            LogHelper.Error(e.ToString());
        }
        logpath = Path.Combine(settingPath, "救人日记.csv");

        // 检查文件是否存在，如果不存在则创建并写入表头
        if (!File.Exists(logpath))
        {
            File.WriteAllText(logpath, "时间,地图ID,副本名,目标名,技能\n");
        }
    }

    public void Save()
    {
        try
        {
            if (File.Exists(path))
            {
                留一层 = QT.QTGET(QTKey.短交留一层);
                自动昏乱 = QT.QTGET(QTKey.自动昏乱);
                拉人 = QT.QTGET(QTKey.拉人);
                范围拉人 = QT.QTGET(QTKey.范围拉人);
            }
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, JsonHelper.ToJson(this));
        }
        catch (Exception ex)
        {
            LogHelper.Error($"Error in Save(): {ex.Message}");
            throw;
        }
    }
    #endregion
    // 提供一个公共只读属性来访问logpath
    public static string LogPath
    {
        get { return logpath; }
    }
    //public int ApexArrowValue = 100; // 非爆发期绝峰多少能量再用
    //public bool UsePeloton = true; // 使用速行 默认开启

    public JobViewSave JobViewSave = new() { MainColor = new Vector4(177 / 255f, 47 / 255f, 47 / 255f, 0.8f) }; // QT设置存档
}
