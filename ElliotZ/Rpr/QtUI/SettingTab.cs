using AEAssist.CombatRoutine.View.JobView;
using ElliotZ.Common;
using ImGuiNET;

namespace ElliotZ.Rpr.QtUI;

public static class SettingTab
{
    private static int _8幻药;
    private static int 宝药;
    private static int _2宝药;

    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("设置", window =>
        {
            // one huge ass TODO here

            //if (ImGui.CollapsingHeader("时间轴", ImGuiTreeNodeFlags.DefaultOpen))
            //{
            //    ImGui.Dummy(new Vector2(5, 0));
            //    ImGui.SameLine();
            //    ImGui.BeginGroup();
            //    ImGui.Checkbox("启用时间轴debug", ref RprSettings.Instance.TimeLinesDebug);
            //    ImGui.Checkbox("启用自动更新", ref RprSettings.Instance.AutoUpdataTimeLines);
            //    if (ImGui.Button("手动更新")) TimeLineUpdater.UpdateFiles(Helper.RprTimeLineUrl);
            //    ImGui.SameLine();
            //    if (ImGui.Button("源码"))
            //        Process.Start(new ProcessStartInfo(Helper.TimeLineLibraryUrl) { UseShellExecute = true });
            //    ImGui.EndGroup();
            //    ImGui.Dummy(new Vector2(0, 10));
            //}

            //if (ImGui.CollapsingHeader("舞伴窗口", ImGuiTreeNodeFlags.DefaultOpen))
            //{
            //    ImGui.Dummy(new Vector2(5, 0));
            //    ImGui.SameLine();
            //    ImGui.BeginGroup();
            //    ImGui.Checkbox("显示舞伴窗口", ref DncSettings.Instance.DancePartnerPanelShow);
            //    ImGui.Checkbox("4人本自动绑舞伴", ref DncSettings.Instance.AutoPartner);
            //    ImGui.DragInt("舞伴窗口大小", ref DncSettings.Instance.DancePartnerPanelIconSize, 1, 20, 100);
            //    ImGui.EndGroup();
            //    ImGui.Dummy(new Vector2(0, 10));
            //}

            //ImGuiHelper.Separator();

            if (ImGui.Button("获取爆发药情况"))
            {
                _8幻药 = cItemHelper.FindItem((uint)Potion._8级刚力之幻药);
                宝药 = cItemHelper.FindItem((uint)Potion.刚力之宝药);
                _2宝药 = cItemHelper.FindItem((uint)Potion._2级刚力之宝药);
            }

            if (_8幻药 > 0)
            {
                ImGui.Text($"8级巧力之幻药：{_8幻药} 瓶");
                DrawPotion(Potion._8级刚力之幻药);
            }

            if (宝药 > 0)
            {
                ImGui.Text($"巧力之宝药：{宝药} 瓶");
                DrawPotion(Potion.刚力之宝药);
            }

            if (_2宝药 > 0)
            {
                ImGui.Text($"2级巧力之宝药：{_2宝药} 瓶");
                DrawPotion(Potion._2级刚力之宝药);
            }
        });
    }

    private static void DrawPotion(Potion potion)
    {
        var id = (int)potion;
        ImGui.SameLine();
        if (ImGui.Button("复制id###" + id))
        {
            ImGui.SetClipboardText(id.ToString());
        }
    }
}