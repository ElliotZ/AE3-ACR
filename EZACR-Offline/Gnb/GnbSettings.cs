using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Helper;
using AEAssist.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb;

public class GnbSettings
{
    public static GnbSettings Instance;

    private static string path;

    public bool UsePeloton = true;

    public bool UseAOE = true;

    public JobViewSave JobViewSave = new JobViewSave();

    public bool UsePotion = false;

    public bool UseLion = true;

    public bool 上传 = true;

    public string ACRMode { get; set; }

    public static void Build(string settingPath)
    {
        path = Path.Combine(settingPath, "GnbSettings", "绝枪设置.json");
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
        catch (Exception ex)
        {
            Instance = new GnbSettings();
            LogHelper.Error(ex.ToString());
        }
    }

    public void Save()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, JsonHelper.ToJson(this));
    }

    private GnbSettings()
    {
        ACRMode = "Normal";
    }

}
