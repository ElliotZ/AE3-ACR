using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using ElliotZ.Common;
using ImGuiNET;
using System.Numerics;
using System.Runtime;

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

            if (ImGui.CollapsingHeader("一般设定", ImGuiTreeNodeFlags.DefaultOpen))
            {
                ImGui.Dummy(new Vector2(5, 0));
                ImGui.SameLine();
                ImGui.BeginGroup();
                ImGui.Checkbox("起手三插爆发药", ref RprSettings.Instance.TripleWeavePot);
                ImGui.DragInt("倒数勾刃读条时间", ref RprSettings.Instance.PrepullCastTimeHarpe, 100, 500, 2000);
                ImGui.DragInt("动画锁长度", ref RprSettings.Instance.AnimLock, 10, 10, 1000);
                ImGui.Checkbox("读条忽略移动状态", ref RprSettings.Instance.ForceCast);
                ImGui.Checkbox("Debug", ref RprSettings.Instance.Debug);
                ImGui.Checkbox("StopHelper Debug", ref StopHelper.Debug);
                ImGui.EndGroup();
                ImGui.Dummy(new Vector2(0, 10));
            }

            ImGuiHelper.Separator();

            if (ImGui.Button("记录当前QT设置"))
            {
                Qt.SaveQtStates();
            }

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