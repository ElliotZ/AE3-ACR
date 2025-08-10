using AEAssist.CombatRoutine.Trigger;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.Triggers;

public class TriggerAction_OpenerSelection : ITriggerAction, ITriggerBase
{
    public int Red { get; set; }

    public string DisplayName { get; } = "GNB/起手选择";

    public string Remark { get; set; }

    public int Target { get; set; }

    public void Check()
    {
    }

    public bool Draw()
    {
        int target = Target;
        string text = target switch
        {
            0 => "Normal",
            1 => "HighEnd1",
            2 => "HighEnd2",
            3 => "HighEnd3",
            4 => "HighEnd4",
            5 => "绝亚2G",
            6 => "绝欧1G",
            7 => "神兵5G",
            8 => "HighEnd5",
            _ => GnbSettings.Instance.ACRMode,
        };
        string preview_value = text;
        if (ImGui.BeginCombo("", preview_value))
        {
            if (ImGui.Selectable("日随"))
            {
                Target = 0;
            }

            if (ImGui.Selectable("100级2.46 2G循环"))
            {
                Target = 1;
            }

            if (ImGui.Selectable("100级2.5 2G循环改"))
            {
                Target = 2;
            }

            if (ImGui.Selectable("100级2.5 1G快速起手"))
            {
                Target = 8;
            }

            if (ImGui.Selectable("100级2.46 1G循环改"))
            {
                Target = 3;
            }

            if (ImGui.Selectable("100级2.5 3G循环改"))
            {
                Target = 4;
            }

            if (ImGui.Selectable("80级 2G血壤绝亚起手"))
            {
                Target = 5;
            }

            if (ImGui.Selectable("90级 1G血壤无情绝欧起手"))
            {
                Target = 6;
            }

            if (ImGui.Selectable("70级 5G无情绝神兵起手"))
            {
                Target = 7;
            }

            ImGui.EndCombo();
        }

        return true;
    }

    public bool Handle()
    {
        int target = Target;
        string text = target switch
        {
            0 => "Normal",
            1 => "HighEnd1",
            2 => "HighEnd2",
            3 => "HighEnd3",
            4 => "HighEnd4",
            5 => "绝亚2G",
            6 => "绝欧1G",
            7 => "神兵5G",
            8 => "HighEnd5",
            _ => GnbSettings.Instance.ACRMode,
        };
        string aCRMode = text;
        GnbSettings.Instance.ACRMode = aCRMode;
        return true;
    }
}
