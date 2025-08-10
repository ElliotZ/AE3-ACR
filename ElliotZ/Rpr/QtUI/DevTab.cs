using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using ImGuiNET;

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
                    Dictionary<uint, IBattleChara> dictionary = [];
                    Core.Resolve<MemApiTarget>().GetNearbyGameObjects(20f, dictionary);
                    dictionary.Remove(Core.Me.EntityId);
                    string text2 = string.Join(", ",
                                               dictionary.Values.Select(character => $"{character.Name}"));
                    ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + 410f);
                    ImGui.Text("周围20m目标: " + text2);
                    string targetGOID;
                    if (Core.Me.GetCurrTarget() is null)
                    {
                        targetGOID = "null";
                    }
                    else
                    {
                        targetGOID = Core.Me.GetCurrTarget()!.GameObjectId.ToString();
                    }
                    ImGui.Text("Target GameObjectId:" + targetGOID);
                    ImGui.Text("Self Casting Spell ID" + (Core.Me.CastActionId).ToString());
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
                    if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count != 0)
                    {
                        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Dequeue();
                    }
                    if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count != 0)
                    {
                        AI.Instance.BattleData.HighPrioritySlots_GCD.Dequeue();
                    }
                }

                ImGui.Text("-------能力技-------");
                if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
                {
                    foreach (SlotAction item in
                             AI.Instance.BattleData.HighPrioritySlots_OffGCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(item.Spell.Name);
                    }
                }

                ImGui.Text("-------GCD-------");
                if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
                {
                    foreach (SlotAction item2 in
                             AI.Instance.BattleData.HighPrioritySlots_GCD.SelectMany(spell => spell.Actions))
                    {
                        ImGui.Text(item2.Spell.Name);
                    }
                }
            }

            if (ImGui.CollapsingHeader("更新日志"))
            {
                ImGui.BeginChild("UpdateLog",
                                 new System.Numerics.Vector2(0f, 300f),
                                 border: true,
                                 ImGuiWindowFlags.HorizontalScrollbar);
                ImGui.TextWrapped("更新日志：\n\n" +
                                  "2025/08/03: 初版。\n" +
                                  "2025/08/10: 重构宏命令系统。");
                ImGui.EndChild();
            }
        });
    }
}
