using AEAssist;
using Dalamud.Bindings.ImGui;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP.Brd.QtUI;

public class PvPBrdOverlay {
  private static bool 调试窗口;

  public void DrawGeneral(JobViewWindow jobViewWindow) {
    Share.Pull = true;
    new 职业配置().配置诗人技能();

    if (!ImGui.CollapsingHeader("调试窗口")) {
      return;
    }

    PVPHelper.PvP调试窗口();
  }

/*  public static class BrdQt
  {
    public static bool GetQt(string qtName) => RotationEntry.QtInstance.GetQt(qtName);

    public static bool ReverseQt(string qtName)
    {
      return RotationEntry.QtInstance.ReverseQt(qtName);
    }

    public static void SetQt(string qtName, bool qtValue)
    {
      RotationEntry.QtInstance.SetQt(qtName, qtValue);
    }

    public static void NewDefault(string qtName, bool newDefault)
    {
      RotationEntry.QtInstance.NewDefault(qtName, newDefault);
    }

    public static void SetDefaultFromNow() => RotationEntry.QtInstance.SetDefaultFromNow();

    public static string[] GetQtArray() => RotationEntry.QtInstance.GetQtArray();
  }*/
}
