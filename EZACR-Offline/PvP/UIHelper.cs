using System.Numerics;
using AEAssist;
using AEAssist.Helper;
using AEAssist.Verify;
using Dalamud.Bindings.ImGui;
using ECommons.DalamudServices;
using ECommons.WindowsFormsReflector;

namespace EZACR_Offline.PvP;

public static class UIHelper {
    public static void 权限获取() {
    string text = Svc.ClientState.LocalContentId.ToString();
    ImGui.Text($"当前的码等级：[{Share.VIP.Level}]");

    if ((Share.VIP.Level == VIPLevel.Normal) && PvPHelper.高级码) {
      ImGui.Text("仅狼狱可用 战场无权限");
    }

    if (!PvPHelper.通用码权限 && !PvPHelper.高级码) {
      ImGui.TextColored(new Vector4(1f, 0.0f, 0.0f, 0.8f), "无权限");
      ImGui.SameLine();

      if (ImGui.Button("复制CID到剪贴板")) {
        Winforms.Clipboard.SetText(text);
        LogHelper.Print("已复制CID到剪贴板");
      }
    }

    if (!PvPHelper.通用码权限 && !PvPHelper.高级码) {
      return;
    }

    ImGui.TextColored(new Vector4(0.16470589f, 0.84313726f, 0.22352941f, 0.8f), "已解锁");

  }

  public static void ConfigureSkillBool(
      uint skillId,
      string skillName,
      string description,
      ref bool variable,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, $"##{skillName}", false);
    ImGui.SetColumnWidth(0, 70f);
    PvPHelper.SkillIcon(skillId);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(skillName);
    ImGui.Text(description + ":");
    ImGui.Checkbox($"##{id}", ref variable);
    ImGui.Columns();
  }

  public static void ConfigureSkillInt(
      uint skillId,
      string skillName,
      string description,
      ref int value,
      int step,
      int quickstep,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, $"##{skillName}", false);
    ImGui.SetColumnWidth(0, 70f);
    PvPHelper.SkillIcon(skillId);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(skillName);
    ImGui.Text(description + ":");
    ImGui.InputInt($"##{id}", ref value, step, quickstep);
    ImGui.Columns();
  }

  public static void ConfigureSkillBoolInt(
      uint skillId,
      string skillName,
      string valDescription,
      string description,
      ref bool status,
      ref int value,
      int step,
      int quickstep,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, $"##{skillName}", false);
    ImGui.SetColumnWidth(0, 70f);
    PvPHelper.SkillIcon(skillId);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(skillName);
    ImGui.Text(description + ":");
    ImGui.Checkbox($"##{id}", ref status);
    ImGui.Text(valDescription + ":");
    ImGui.InputInt($"##{id}+1", ref value, step, quickstep);
    ImGui.Columns();
  }

  public static void ConfigureSkillSliderFloat(
      uint skillId,
      string skillName,
      string valDescription,
      ref float value,
      float min,
      float max,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, $"##{skillName}", false);
    ImGui.SetColumnWidth(0, 70f);
    PvPHelper.SkillIcon(skillId);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(skillName);
    ImGui.Text(valDescription + ":");
    ImGui.SliderFloat($"##{id}", ref value, min, max);
    ImGui.Columns();  }

  public static void ConfigureSkilldescription(uint skillId, 
                                                string skillName, 
                                                string description) {
    ImGui.Separator();
    ImGui.Columns(2, $"##{skillName}", false);
    ImGui.SetColumnWidth(0, 70f);
    PvPHelper.SkillIcon(skillId);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(skillName);
    ImGui.Text(description + ":");
    ImGui.Columns();
  }
}
