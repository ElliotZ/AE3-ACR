using ElliotZ.Rpr.QtUI;
using ImGuiNET;
using System.Numerics;

namespace ElliotZ.Rpr;

public class RprCmdWindow
{
    public static void Draw()
    {
        if (!RprSettings.Instance.CommandWindowOpen) { return; }

        ImGuiViewportPtr mainViewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowSize(new Vector2(mainViewport.Size.X / 2f,
                                            mainViewport.Size.Y / 1.5f),
                                    ImGuiCond.Always);
        ImGui.Begin("Reaper Command Help", ref RprSettings.Instance.CommandWindowOpen);
        ImGui.TextWrapped("通过 " + RprHelper.TxtCmdHandle + " 使用快捷指令。结合游戏内宏使用可以方便手柄用户的操作。");
        ImGui.Separator();
        ImGui.Columns(3, "CommandColumns", true);
        ImGui.SetColumnWidth(0, mainViewport.Size.X / 10f);
        ImGui.SetColumnWidth(1, mainViewport.Size.X / 5f);
        ImGui.SetColumnWidth(2, mainViewport.Size.X / 5f);
        ImGui.Text("指令类型");
        ImGui.NextColumn();
        ImGui.Text("中文指令");
        ImGui.NextColumn();
        ImGui.Text("英文指令");
        ImGui.NextColumn();
        ImGui.Separator();
        foreach (var (CmdType, CNCmd, ENCmd) in Qt.CmdList())
        {
            ImGui.Text(CmdType);
            ImGui.NextColumn();
            if (ImGui.Button("复制##" + CNCmd)) { ImGui.SetClipboardText(CNCmd); }
            ImGui.SameLine();
            ImGui.Text(CNCmd);
            ImGui.NextColumn();
            if (ImGui.Button("复制##" + ENCmd)) { ImGui.SetClipboardText(ENCmd); }
            ImGui.SameLine();
            ImGui.Text(ENCmd);
            ImGui.NextColumn();
        }

        ImGui.Columns(1);
        ImGui.Separator();
        if (ImGui.Button("关闭"))
        {
            RprSettings.Instance.CommandWindowOpen = false;
            RprSettings.Instance.Save();
        }
        ImGui.End();
    }
}
