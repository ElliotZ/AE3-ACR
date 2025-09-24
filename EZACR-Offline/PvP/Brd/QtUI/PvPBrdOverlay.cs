using AEAssist;
using AEAssist.Helper;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP.Brd.QtUI;

public static class PvPBrdOverlay {
  public static void DrawGeneral(JobViewWindow jobViewWindow) {
    Share.Pull = true;
    UIHelper.权限获取();

    ImGui.Separator();
    ImGui.Text("职业PvP目标选择设定");
    ImGui.PushItemWidth(50f);
    ImGui.InputInt($"米内##228", ref PvPBrdSettings.Instance.TargetingDistance);
    ImGui.SameLine();
    ImGui.InputFloat($"比例血量以下最低血量目标##{229}", ref PvPBrdSettings.Instance.TargetingHpThreshold);
    ImGui.PopItemWidth();
    ImGui.Text("如果指定范围内不存在血量低于设定比例的目标则会默认按照共通配置中的范围\n"
             + "寻找最近的目标。如果共通配置中关闭目标选择，则此选项无效。");
    
    PvPBrdSettings.Instance.药血量 = Math.Clamp(PvPBrdSettings.Instance.药血量, 1, 100);
    UIHelper.ConfigureSkillInt(29711U,
                               "喝热水",
                               "热水阈值",
                               ref PvPBrdSettings.Instance.药血量,
                               3,
                               10,
                               5);
    PvPBrdSettings.Instance.和弦箭 = Math.Clamp(PvPBrdSettings.Instance.和弦箭, 1, 4);
    UIHelper.ConfigureSkillInt(41464U,
                               "和弦箭",
                               "使用层数",
                               ref PvPBrdSettings.Instance.和弦箭,
                               1,
                               1,
                               87);
    自定义光阴神();
    //PvPBrdSettings.Instance.Save();
  }

  private static void 自定义光阴神() {
      ImGui.Separator();
      ImGui.Columns(2, "##光阴神", false);
      ImGui.SetColumnWidth(0, 70f);
      PvPHelper.SkillIcon(29400U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text("光阴神");
      ImGui.Text("解控队友|聊天框打印:");
      PvPBrdSettings.Instance.光阴对象 = Math.Clamp(PvPBrdSettings.Instance.光阴对象,
                                                0,
                                                PartyHelper.Party.Count - 1);
      ImGui.Checkbox($"##{1}", ref PvPBrdSettings.Instance.光阴队友);
      ImGui.SameLine();
      ImGui.Checkbox($"##{54}", ref PvPBrdSettings.Instance.光阴播报);
      ImGui.Text("优先玩家名");
      ImGui.InputText($"##{678}", ref PvPBrdSettings.Instance.优先对象, 10);
      IBattleChara? battleChara =
          PartyHelper.Party.FirstOrDefault(x => x?.Name?.TextValue == PvPBrdSettings.Instance.优先对象);
      ImGui.Text("优先对象:");

      if (battleChara != null) {
        int num = PartyHelper.Party.IndexOf(battleChara);
        PvPBrdSettings.Instance.光阴对象 = num;
        ImGui.SameLine();

        if ((PartyHelper.Party.Count > 0)
         && (PvPBrdSettings.Instance.光阴对象 >= 0)
         && (PvPBrdSettings.Instance.光阴对象 < PartyHelper.Party.Count)) {
          ImGui.Text($"{PartyHelper.Party[PvPBrdSettings.Instance.光阴对象]?.Name ?? "未存在此玩家"}");
        } else {
          ImGui.Text("未存在此玩家");
        }
      } else {
        ImGui.InputInt($"##{78}", ref PvPBrdSettings.Instance.光阴对象, 1, 100);
        PvPBrdSettings.Instance.光阴对象 = Math.Clamp(PvPBrdSettings.Instance.光阴对象,
                                                  0,
                                                  PartyHelper.Party.Count - 1);
        ImGui.SameLine();

        if ((PartyHelper.Party.Count > 0)
         && (PvPBrdSettings.Instance.光阴对象 >= 0)
         && (PvPBrdSettings.Instance.光阴对象 < PartyHelper.Party.Count)) {
          ImGui.Text($"{PartyHelper.Party[PvPBrdSettings.Instance.光阴对象]?.Name ?? "未存在此玩家"}");
        } else {
          ImGui.Text("未存在此玩家");
        }

        if (ImGui.Button("优先玩家名设定该对象")) {
          PvPBrdSettings.Instance.优先对象 = PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].Name
                                                    .TextValue;
        }
      }

      ImGui.Columns();
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
