using AEAssist.Helper;
using AEAssist.IO;
using ElliotZ.ModernJobViewFramework;
// ReSharper disable FieldCanBeMadeReadOnly.Global

namespace EZACR_Offline.PvP.Brd;

public class PvPBrdSettings {
  public static PvPBrdSettings Instance;
  private static string path;
  public int 药血量 = 70;
  public int 和弦箭 = 4;
  public bool 光阴播报 = true;
  public bool 光阴队友 = true;
  public int 光阴对象 = 0;
  public string 优先对象 = "梅友仁";
  public float TargetingHpThreshold = 0.5f;
  public int TargetingDistance = 25;
  public Dictionary<string, bool> QtStates;
  public bool CommandWindowOpen = true;
  public JobViewSave JobViewSave;

  private PvPBrdSettings() {
    QtStates = new Dictionary<string, bool> {
        ["和弦箭"] = true,
        ["光阴神"] = true,
        ["沉默"] = true,
        ["爆破箭"] = true,
        ["绝峰箭"] = true,
        ["强劲射击"] = true,
        ["喝热水"] = true,
        ["职能技能"] = true,
        ["自动净化"] = true,
        ["龟壳"] = true,
        ["冲刺"] = true,
    };
    JobViewSave = new JobViewSave {
        CurrentTheme = ModernTheme.ThemePreset.森林绿,
    };
  }

  public static void Build(string settingPath) {
    path = Path.Combine(settingPath, "PvPBrdSettings.json");

    if (!File.Exists(path)) {
      Instance = new PvPBrdSettings();
      Instance.Save();
    } else {
      try {
        Instance = JsonHelper.FromJson<PvPBrdSettings>(File.ReadAllText(path));
      } catch (Exception ex) {
        Instance = new PvPBrdSettings();
        LogHelper.Error(ex.ToString());
      }
    }
  }

  public void Save() {
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    File.WriteAllText(path, JsonHelper.ToJson(this));
  }
}
