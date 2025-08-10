using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using EZACR_Offline.Gnb.QtUI;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.Triggers;

public class TriggerAction_QT : ITriggerAction, ITriggerBase
{
    public string Key = "";

    public bool Value;

    private int _selectIndex;

    private string[] _qtArray;

    public string DisplayName { get; } = "GNB/QT";


    public string Remark { get; set; }

    public TriggerAction_QT()
    {
        _qtArray = Qt.Instance.GetQtArray();
    }

    public bool Draw()
    {
        _selectIndex = Array.IndexOf(_qtArray, Key);
        if (_selectIndex == -1)
        {
            _selectIndex = 0;
        }

        ImGuiHelper.LeftCombo("选择Key", ref _selectIndex, _qtArray);
        Key = _qtArray[_selectIndex];
        ImGui.SameLine();
        using (new GroupWrapper())
        {
            ImGui.Checkbox("", ref Value);
        }

        return true;
    }

    public bool Handle()
    {
        Qt.Instance.SetQt(Key, Value);
        return true;
    }
}