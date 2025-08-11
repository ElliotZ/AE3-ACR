using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using ImGuiNET;
using System.Numerics;

namespace EZACR_Offline.Gnb.Triggers;

public class TriggerAction_新QT : ITriggerAction, ITriggerBase
{
    public List<TriggerQTSetting> QTList = [];

    private int _selectIndex;

    private string[] _qtArray;

    public string DisplayName { get; } = "绝枪/QT设置(新)";


    public string Remark { get; set; }

    public bool Draw()
    {
        ImGui.BeginChild("###TriggerWhm", new Vector2(0f, 0f));
        ImGuiHelper.DrawSplitList("QT开关", QTList, DrawHeader, AddCallBack, DrawCallback);
        ImGui.EndChild();
        return true;
    }

    public bool Handle()
    {
        foreach (TriggerQTSetting qT in QTList)
        {
            qT.action();
        }

        return true;
    }

    private TriggerQTSetting DrawCallback(TriggerQTSetting arg)
    {
        arg.draw();
        return arg;
    }

    private string DrawHeader(TriggerQTSetting arg)
    {
        string text = (arg.Value ? "开" : "关");
        return text + "-" + arg.Key;
    }

    private TriggerQTSetting AddCallBack()
    {
        return new TriggerQTSetting();
    }
}

