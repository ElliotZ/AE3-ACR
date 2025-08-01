using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
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
                    //float totalHealthPercentage = MnkRotationEventHandler.TotalHealthPercentage;
                    //ImGui.Text($"周围小怪总当前血量百分比: {totalHealthPercentage:F2}%" + "%");
                    //float averageTTK = MnkRotationEventHandler.AverageTTK;
                    //ImGui.Text($"预估周围小怪平均死亡时间: {averageTTK / 1000f:F2} 秒");
                    //ImGui.Text($"当前脉轮状态: {Core.Resolve<JobApi_Monk>().Nadi}");
                    //ImGui.Text($"震脚次数: {MnkRotationEventHandler.PerfectBalanceCount - 1}");
                    //ImGui.Text($"震脚层数: {Core.Resolve<MemApiSpell>().GetCharges(69u):F2}");
                    //float num = (int)Core.Resolve<JobApi_Monk>().BlitzTimeRemaining;
                    //ImGui.Text($"必杀技剩余时间: {num / 1000f:F1} 秒");
                    //ImGui.Text($"自动对齐红莲与团辅: {MnkRotationEventHandler.BurstAlignmentLocked} ");
                    ImGui.Text($"当前地图ID: {Core.Resolve<MemApiZoneInfo>().GetCurrTerrId()} ");
                    ImGui.Text($"角色当前坐标: {Core.Me.Position} ");
                    if (ImGui.Button("CID:" + ECHelper.ClientState.LocalContentId))
                    {
                        ImGui.SetClipboardText(ECHelper.ClientState.LocalContentId.ToString());
                        LogHelper.Print("已复制CID到剪贴板");
                    }

                    //string text = string.Join(", ", Core.Me.StatusList.Select(delegate (Status status)
                    //{
                    //    //IL_0027: Unknown result type (might be due to invalid IL or missing references)
                    //    //IL_002c: Unknown result type (might be due to invalid IL or missing references)
                    //    //IL_002f: Unknown result type (might be due to invalid IL or missing references)
                    //    //IL_0034: Unknown result type (might be due to invalid IL or missing references)
                    //    //IL_0037: Unknown result type (might be due to invalid IL or missing references)
                    //    DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
                    //    defaultInterpolatedStringHandler.AppendFormatted(status.StatusId);
                    //    defaultInterpolatedStringHandler.AppendLiteral(": ");
                    //    Status value2 = status.GameData.Value;
                    //    defaultInterpolatedStringHandler.AppendFormatted<ReadOnlySeString>(((Status)(ref value2)).Name);
                    //    return defaultInterpolatedStringHandler.ToStringAndClear();
                    //}));
                    Dictionary<uint, IBattleChara> dictionary = new Dictionary<uint, IBattleChara>();
                    Core.Resolve<MemApiTarget>().GetNearbyGameObjects<IBattleChara>(20f, dictionary);
                    dictionary.Remove(Core.Me.EntityId);
                    string text2 = string.Join(", ", dictionary.Values.Select((IBattleChara character) => $"{character.Name}"));
                    ImGui.PushTextWrapPos(ImGui.GetCursorPosX() + 410f);
                    //ImGui.Text("自身Buff (" + text + ")");
                    ImGui.Text("周围20m目标: " + text2);
                    ImGui.PopTextWrapPos();
                    //int num2 = GcdCalculator.CalculateSyncedGcd();
                    //ImGui.Text($"GCD复唱时间 ({(float)num2 / 1000f})");
                    float rotation = Core.Me.Rotation;
                    //float value = rotation.RadToNormalizedDeg();
                    ImGui.Text($"自身面向 ({rotation:F2})");
                }

                ImGuiHelper.Separator(7u, 7u);
                //MnkRotationEventHandler mnkRotationEventHandler = new MnkRotationEventHandler();
                //IBattleChara battleChara = mnkRotationEventHandler.CheckTank();
                //if (battleChara != null)
                //{
                //    mnkRotationEventHandler.CheckEnemiesAroundTank(battleChara);
                //    ImGui.Text($"找到的T: {battleChara.Name}");
                //    ImGui.Text($"周围25米小怪数量: {mnkRotationEventHandler.VisibleEnemiesIn25m}");
                //    ImGui.Text($"周围5米小怪数量: {mnkRotationEventHandler.VisibleEnemiesIn5m}");
                //    ImGui.Text($"小怪集中度: {mnkRotationEventHandler.concentrationPercentage}%%");
                //}
                //else
                //{
                //    ImGui.Text("未找到T");
                //}
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

            //if (ImGui.CollapsingHeader("更新日志"))
            //{
            //    ImGui.BeginChild("UpdateLog", new System.Numerics.Vector2(0f, 300f), border: true, ImGuiWindowFlags.HorizontalScrollbar);
            //    ImGui.TextWrapped("更新日志：\n\n- 2024/10/15: 优化红莲/疾风派生到期自动使用逻辑\n- 2024/10/16: 新增TP身位。重做身位指示器绘制逻辑，现在会更早出现了\n    - 优化红莲极意，现在红莲极意开启后不会再错误插入能力技\n    - 新增停手功能，检测到自身有热病/加速度炸弹会选中自己并且停手；\n    - 检测到目标有无敌类buff会停手\n- 2024/10/17: 优化TP身位算法。新增震脚内身位绘制\n    - 新增支持自定义的真北与TP身位时间节点\n- 2024/10/18: 新增四人本道中小怪不开爆发的实验性功能\n    - 新增红莲与团辅差距5s内自动对齐功能\n- 2024/10/19: 新增自动减伤功能\n    - 优化小怪血量与TTK计算逻辑\n- 2024/10/20: 修复停手功能，现在会正确在加速度/热病剩余3s时选中自己然后踢六合星导脚\n    - 将开场的爆发药挪至第2GCD，防止部分玩家震脚与爆发药双插卡GCD问题\n- 2024/10/21: 新增HotKey：爆发药、轻身步法MO(记得绑快捷键)\n    - 新增金刚极意HotKey使用后变为金刚周天的适配\n- 2024/10/24: 新增QT：快速爆发\n    - 重写120第一个震脚开启时机，避免过早开启\n- 2024/10/25: 新增QT：最终爆发\n    - 新增HotKey：六合+LB\n    - 优化没习得必杀前AOE攒震脚的多余操作\n- 2024/10/31: 新增QT：下一个打阴，与“下一个打阳”互斥\n- 2024/11/1: 支持自定义红莲极意插入时机\n    - 延后搓豆子进队列的时间点，现在处理钢铁会更舒服\n- 2024/11/4: 新增M1S时间轴\n    - 新增与时间轴配套使用的“轴控设置”\n    - 拆分自动减伤 → 自动金刚极意 + 自动牵制\n    - 新增自动真言\n- 2024/11/5: 新增M2S时间轴\n- 2024/11/6: 新增QT：优先红莲派生\n- 2024/11/7: 新增HotKey：红玩步法\n- 2024/11/8: 新增HotKey：回退\n    - 新增自动真言可勾选buff(还有想加的可以留言反馈)\n- 2024/11/9: 现在TP身位应该更安全了？\n- 2024/11/10: 新增爆发药时间点选择\n- 2024/11/11: 新增M3S、M4S时间轴\n    - 新增延后绝空拳进红莲buff的设置(默认启用)\n- 2024/11/12: 略微优化最终爆发震脚开法\n- 2024/11/14: 优化低等级写法，现在技能没学应该不会卡循环了\n- 2024/11/17: 调低搓豆子进队列的时机\n    - 删除红玩步法设置，改为按距离线性增加延迟(与目标距离大于25m时禁止TP)\n    - 新增远离时自动清理六合星导脚队列，且尝试使用时间修改为200ms\n- 2024/11/21: 安全TP小更新\n- 2024/11/28: 新增“新版团辅红莲插入方法”\n- 2024/11/30: 新增QT：攒功力\n- 2024/12/12: 优化AOE攒震脚逻辑\n- 2024/12/17: 重做HotKey：轻身步法MO\n- 2024/12/18: 新增真北期间不绘制身位\n    - 优化震脚开启时机\n- 2024/12/29: 调整六合星导脚HotKey，兼容优化GCD偏移\n    - 修复红莲后插入真北的问题\n- 2025/1/8: 新增时间轴接口：检测自身是否打出1GCD\n- 2025/1/19: 修复时间轴不能正确触发震脚计数的问题\n    - 现已支持宏指令操作HotKey和QT\n- 2025/7/4: 修复宏指令错别字，支持宏指令Toast2提示QT状态\n    - 新增Boss含有某些无视身位的Buff时不绘制身位\n- 2025/7/20: 新增更直观的Qt设置界面(TriggerAction)");
            //    ImGui.EndChild();
            //}
        });
    }
}
