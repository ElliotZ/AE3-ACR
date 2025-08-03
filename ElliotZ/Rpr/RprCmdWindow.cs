using ElliotZ.Rpr.QtUI;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr;

public class RprCmdWindow
{
    //private static readonly (string, string, string)[] cmdList = 
    //    [
    //    ("HotKey", "/EZRpr 入境_hk", "/EZRpr ingress_hk"),
    //    ("HotKey", "/EZRpr 出境_hk", "/EZRpr egress_hk"),
    //    ("HotKey", "/EZRpr 目标入境_hk", "/EZRpr ingresst_hk"),
    //    ("HotKey", "/EZRpr 目标出境_hk", "/EZRpr egresst_hk"),
    //    ("HotKey", "/EZRpr 镜头方向入境_hk", "/EZRpr ingresscam_hk"),
    //    ("HotKey", "/EZRpr 镜头方向出境_hk", "/EZRpr egresscam_hk"),
    //    ("HotKey", "/EZRpr 神秘纹_hk", "/EZRpr crest_hk"),
    //    ("HotKey", "/EZRpr LB_hk", "/EZRpr lb_hk"),
    //    ("HotKey", "/EZRpr 亲疏_hk", "/EZRpr armslength_hk"),
    //    ("HotKey", "/EZRpr 内丹_hk", "/EZRpr secondwind_hk"),
    //    ("HotKey", "/EZRpr 浴血_hk", "/EZRpr bloodbath_hk"),
    //    ("HotKey", "/EZRpr 牵制_hk", "/EZRpr feint_hk"),
    //    ("HotKey", "/EZRpr 真北_hk", "/EZRpr truenorth_hk"),
    //    ("HotKey", "/EZRpr 播魂种_hk", "/EZRpr soulsow_hk"),
    //    ("HotKey", "/EZRpr 疾跑_hk", "/EZRpr sprint_hk"),
    //    ("HotKey", "/EZRpr 爆发药_hk", "/EZRpr pot_hk"),

    //    ("QT", "/EZRpr 爆发药_qt", "/EZRpr pot_qt"),
    //    ("QT", "/EZRpr 爆发药2分_qt", "/EZRpr pot2min_qt"),
    //    ("QT", "/EZRpr 起手_qt", "/EZRpr opener_qt"),
    //    ("QT", "/EZRpr 单魂衣_qt", "/EZRpr singleshroud_qt"),
    //    ("QT", "/EZRpr 神秘环_qt", "/EZRpr arcanecircle_qt"),
    //    ("QT", "/EZRpr 大丰收_qt", "/EZRpr plenhar_qt"),
    //    ("QT", "/EZRpr 灵魂割_qt", "/EZRpr soulskill_qt"),
    //    ("QT", "/EZRpr 挥割_qt", "/EZRpr bloodstalk_qt"),
    //    ("QT", "/EZRpr 暴食_qt", "/EZRpr gluttony_qt"),
    //    ("QT", "/EZRpr 魂衣_qt", "/EZRpr enshroud_qt"),
    //    ("QT", "/EZRpr 完人_qt", "/EZRpr perfectio_qt"),
    //    ("QT", "/EZRpr 真北_qt", "/EZRpr truenorth_qt"),
    //    ("QT", "/EZRpr 收获月_qt", "/EZRpr harvestmoon_qt"),
    //    ("QT", "/EZRpr 勾刃_qt", "/EZRpr harpe_qt"),
    //    ("QT", "/EZRpr AOE_qt", "/EZRpr aoe_qt"),
    //    ("QT", "/EZRpr 播魂种_qt", "/EZRpr soulsow_qt"),
    //    ("QT", "/EZRpr 祭牲_qt", "/EZRpr sacrificium_qt"),
    //    ("QT", "/EZRpr 倾泻资源_qt", "/EZRpr dump_qt"),
    //    ("QT", "/EZRpr 真北优化_qt", "/EZRpr optinorth_qt"),
    //    ("QT", "/EZRpr 智能AOE_qt", "/EZRpr smartaoe_qt"),
    //    ("QT", "/EZRpr 自动突进_qt", "/EZRpr autoingress_qt"),
    //    ];

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
