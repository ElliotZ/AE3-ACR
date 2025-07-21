using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using Dalamud.Interface.Colors;
using ImGuiNET;

namespace ElliotZ.Rpr.QtUI;

public static class ReadmeTab
{
    private static readonly InfoBox Box = new()
    {
        AutoResize = true,
        BorderColor = ImGuiColors.ParsedGold,
        ContentsAction = () =>
        {
            if (ImGui.Button("查看更新日志")) { }

            ImGuiHelper.Separator();
            ImGui.BulletText("Placeholder Txt");
        }
    };

    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("README", window =>
        {
            ImGui.Dummy(new System.Numerics.Vector2(0, 1));
            ImGui.Dummy(new System.Numerics.Vector2(5, 0));
            ImGui.SameLine();
            Box.DrawStretched();
        });
    }
}
