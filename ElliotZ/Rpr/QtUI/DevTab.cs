using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using ElliotZ.Common;
using ImGuiNET;
using Lumina.Text.ReadOnly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.QtUI;

public static class DevTab
{
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("Dev", window =>
        {
            if (ImGui.CollapsingHeader("Dev信息"))
            {
                if (ECHelper.ClientState.LocalPlayer != null)
                {
                    var rre = new RprRotationEntry();
                    ImGui.Text($"周围小怪总当前血量百分比: {BattleData.Instance.TotalHpPercentage:F2}%" + "%");
                    ImGui.Text($"预估周围小怪平均死亡时间: {BattleData.Instance.AverageTTK / 1000f:F2} 秒");
                    ImGui.Text($"上一个连击: {Core.Resolve<MemApiSpell>().GetLastComboSpellId()}");
                    ImGui.Text($"上一个GCD: {Core.Resolve<MemApiSpellCastSuccess>().LastGcd}");
                    ImGui.Text($"上一个能力技: {Core.Resolve<MemApiSpellCastSuccess>().LastAbility}");
                    ImGui.Text("下一个GCD技能：" + rre.CheckFirstAvailableSkillGCD());
                    ImGui.Text("下一个offGCD技能：" + rre.CheckFirstAvailableSkilloffGCD());
                    ImGui.Text($"当前地图ID: {Core.Resolve<MemApiZoneInfo>().GetCurrTerrId()} ");
                    ImGui.Text($"角色当前坐标: {Core.Me.Position} ");
                    if (ImGui.Button("CID:" + ECHelper.ClientState.LocalContentId))
                    {
                        ImGui.SetClipboardText(ECHelper.ClientState.LocalContentId.ToString());
                        LogHelper.Print("已复制CID到剪贴板");
                    }
                    Dictionary<uint, IBattleChara> dictionary = new Dictionary<uint, IBattleChara>();
                    Core.Resolve<MemApiTarget>().GetNearbyGameObjects<IBattleChara>(20f, dictionary);
                    dictionary.Remove(Core.Me.EntityId);
                    string text2 = string.Join(", ", dictionary.Values.Select((IBattleChara character) => $"{character.Name}"));
                    ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + 410f);
                    ImGui.Text("周围20m目标: " + text2);
                    ImGui.PopTextWrapPos();
                    ImGui.Text("IsPulling: " + BattleData.Instance.IsPulling);
                    ImGui.Text($"自身面向 ({Core.Me.Rotation:F2})");
                }

                ImGuiHelper.Separator(7u, 7u);
            }

            if (ImGui.CollapsingHeader("插入技能状态"))
            {
                if (ImGui.Button("清除队列"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
                }

                ImGui.SameLine();
                if (ImGui.Button("清除一个"))
                {
                    AI.Instance.BattleData.HighPrioritySlots_OffGCD.Dequeue();
                    AI.Instance.BattleData.HighPrioritySlots_GCD.Dequeue();
                }

                ImGui.Text("-------能力技-------");
                if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                {
                    foreach (SlotAction item in AI.Instance.BattleData.HighPrioritySlots_OffGCD.SelectMany((Slot spell) => spell.Actions))
                    {
                        ImGui.Text(item.Spell.Name);
                    }
                }

                ImGui.Text("-------GCD-------");
                if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                {
                    foreach (SlotAction item2 in AI.Instance.BattleData.HighPrioritySlots_GCD.SelectMany((Slot spell) => spell.Actions))
                    {
                        ImGui.Text(item2.Spell.Name);
                    }
                }
            }

            if (ImGui.CollapsingHeader("更新日志"))
            {
                ImGui.BeginChild("UpdateLog", new System.Numerics.Vector2(0f, 300f), border: true, ImGuiWindowFlags.HorizontalScrollbar);
                ImGui.TextWrapped("更新日志：PlaceHolder txt");
                ImGui.EndChild();
            }
        });
    }
}
