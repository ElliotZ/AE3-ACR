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
            ImGui.Text("欢迎使用EZRpr！开始使用之前请确保阅读以下QT选项说明以及设置页面，如果有问题欢迎DC留言！");
            ImGuiHelper.Separator();
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("单魂衣：神秘环期间只会有一个免费送的变身。暂时不要开，逻辑还没有优化完。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("神秘环/大丰收/灵魂割/挥割/暴食/魂衣/完人……：比较简单，开了就会打这个技能，不开就不会打。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("智能AOE：AOE技能以及类似暴食、团契这类多目标衰减的单体技能会自动寻找能打到最多目标的目标来施放。" +
                             "目前只会判断目标数，没有任何其他判断，一般来讲日随环境以外不建议开启。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("真北优化：开了之后挥割以及挥割变化的缢杀/绞决爪技能会在保证不溢出红条的情况下等待" +
                             "身位正确或者真北施放后才打出。逻辑比较保守，基本不会导致资源溢出，正常情况下可以启用后隐藏。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("自动突进：在关闭勾刃QT的情况下或者循环中为了止损禁止打勾刃的情况下，如果足够远离Boss则会" +
                             "自动向目标方向施放地狱入境。“足够远离”的标准是打不到目标，并且施放地狱入境之后的落点不会超出近战范围。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("倾泻资源：开启后会在可用情况下尽快打出收获月，如果有足够蓝条也会施放变身。" +
                             "挥割/爪也会忽略一些延迟打出的判定。");
            ImGui.Bullet();
            ImGui.SameLine();
            ImGui.Text("设置中有一些自动使用职能技能的设置，开启后会自动隐藏对应的技能Hotkey。");
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
