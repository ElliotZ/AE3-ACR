using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons.DalamudServices;
using ElliotZ;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP;

public static class CommonUI {
  
  public static void BuildPvPDebug(JobViewWindow instance) {
    if (Svc.ClientState.LocalContentId == 19014409518809162UL) {
      ImGui.Text($"gcd:{GCDHelper.GetGCDCooldown()}");
      ImGui.Text($"CastActionId:{Core.Me.CastActionId}");
      ImGui.Text($"是否55:{PvPHelper.是否55()}");
      SeString targetName = PvPTargetHelper.TargetSelector.GetNearestTarget()?.Name ?? "无";
      ImGui.Text($"最近目标: {targetName}");
      ImGui.Text($"最合适25米目标: {
        PvPTargetHelper.TargetSelector.GetSkillTargetSmart(25, 1U)?.Name ?? "无"
      }");
      ImGui.Text($"自己：{Core.Me.Name},{Core.Me.DataId},{Core.Me.Position}");
      ImGui.Text($"坐骑状态：{Svc.Condition[ConditionFlag.Mounted]}");
      ImGui.Text($"血量百分比：{Core.Me.CurrentHpPercent()}");
      ImGui.Text($"盾值百分比：{Core.Me.ShieldPercentage / 100f}");
      ImGui.Text($"血量百分比：{Core.Me.CurrentHpPercent() + Core.Me.ShieldPercentage / 100.0 <= 1.0}");
      ImGui.Text($"是否移动：{MoveHelper.IsMoving()}");
      ImGui.Text($"小队人数：{PartyHelper.CastableParty.Count}");
      if (Core.Me.GetCurrTarget() is { } t) {
        ImGui.Text($"视线阻挡: {PvPHelper.LoSBlocked(t)}");
        ImGui.Text($"目标：{t.Name ?? "无"},{t.DataId},{t.Position}");
        ImGui.Text($"目标5米内人数：{TargetHelper.GetNearbyEnemyCount(t, 25, 5)}");
      }
      
      ImGui.Text($"25米内敌方人数：{TargetHelper.GetNearbyEnemyCount(Core.Me, 1, 25)}");
      ImGui.Text($"20米内小队人数：{PartyHelper.CastableAlliesWithin20.Count}");
      ImGui.Text($"LB槽当前数值：{Core.Me.LimitBreakCurrentValue()}");
//      ImGui.Text($"上个技能：{Core.Resolve<MemApiSpellCastSuccess>().LastSpell}");
//      ImGui.Text($"上个GCD：{Core.Resolve<MemApiSpellCastSuccess>().LastGcd}");
//      ImGui.Text($"上个能力技：{Core.Resolve<MemApiSpellCastSuccess>().LastAbility}");
//      ImGui.Text($"上个连击技能：{Core.Resolve<MemApiSpell>().GetLastComboSpellId()}");
//      ImGui.Text($"t野火目标：{PvPTargetHelper.TargetSelector.GetWildFireTargetSmart()?.Name ?? "无"}");
//      ImGui.Text($"技能变化：{Core.Resolve<MemApiSpell>().CheckActionChange(29102U)}");
//      ImGui.Text($"烈牙cd：{29102U.GetSpell().IsReadyWithCanCast()}");
//      ImGui.Text($"烈牙2cd：{29103U.GetSpell().IsReadyWithCanCast()}");
//      ImGui.Text($"自身技能：{Core.Me.HasLocalPlayerAura(2282U)}");
//      ImGui.Text($"IsUnlockWithCDCheck：{29649U.IsUnlockWithCDCheck()}");
//      ImGui.Text($"IsReadyWithCanCast：{29649U.GetSpell().IsReadyWithCanCast()}");

      if (ImGui.Button("null")) {
        Svc.Targets.Target = null;
      }

      if (ImGui.Button("最远")) {
        Svc.Targets.Target = PvPTargetHelper.TargetSelector.GetFarthestTarget();
      }

      if (ImGui.Button("1")) {
        Core.Resolve<MemApiMove>().SetRot(CameraHelper.GetCameraRotationReversed());
      }

      if (ImGui.Button("21")) {
        Core.Resolve<MemApiMove>().SetRot(CameraHelper.GetCameraRotation());
      }

      ImGui.Text($"？：{MountHandler.CanUseMount()}");
      
    } else {
      ImGui.Text("你不需要用到调试");
    }
  }
  
  public static void BuildCommonSettings(JobViewWindow instance) {
    ImGui.Text("共通配置");
    ImGui.PushItemWidth(100f);

    if (ImGui.InputInt($"攻击判定增加(仅影响ACR判断,配合长臂猿使用)##{66}", ref PvPSettings.Instance.长臂猿, 1, 1)) {
      PvPSettings.Instance.长臂猿 = Math.Clamp(PvPSettings.Instance.长臂猿, 0, 5);
    }

    ImGui.Text("自动选中默认排除:\n不死救赎3039 神圣领域1302 被保护2413 龟壳3054 地天1240");
    ImGui.Checkbox($"自动选中(只在PvP生效)##{666}", ref PvPSettings.Instance.自动选中);
    ImGui.SameLine();
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"米内最近目标##{229}", ref PvPSettings.Instance.自动选中自定义范围);
    ImGui.PopItemWidth();
    ImGui.Checkbox($"只选中玩家(测试)##{1412}", ref PvPSettings.Instance.不选冰);
    ImGui.Checkbox($"无目标时自动坐骑(默认陆行鸟)测试##{222}", ref PvPSettings.Instance.无目标坐骑);
    ImGui.Checkbox($"自动坐骑指定相应坐骑##{678}", ref PvPSettings.Instance.指定坐骑);

    if (PvPSettings.Instance.指定坐骑) {
      ImGui.SameLine();

      if (ImGui.InputText("坐骑名字", ref PvPSettings.Instance.坐骑名, 16 /*0x10*/)) {
        ImGui.Text("设置的坐骑名字为: " + PvPSettings.Instance.坐骑名);
      }

      if (ImGui.Button("呼叫坐骑!")) {
        Core.Resolve<MemApiSendMessage>().SendMessage("/mount " + PvPSettings.Instance.坐骑名);
      }
    }

    ImGui.Text("自动坐骑在范围");
    ImGui.SameLine();
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"米内有敌人时不用##{228}", ref PvPSettings.Instance.无目标坐骑范围);
    ImGui.PopItemWidth();
    ImGui.Text("自动坐骑尝试间隔");
    ImGui.SameLine();
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"秒##{230}", ref PvPSettings.Instance.坐骑cd);
    ImGui.PopItemWidth();
    ImGui.Checkbox($"!!技能自动对最近敌人释放!!##{212}", ref PvPSettings.Instance.技能自动选中);

    if (PvPSettings.Instance.技能自动选中) {
      ImGui.SameLine();
      ImGui.Checkbox($"!!选择技能范围内血量最低目标!!##{216}", ref PvPSettings.Instance.最合适目标);
    }

    ImGui.PopItemWidth();
    PvPSettings.Instance.Save();

    if (ImGui.CollapsingHeader("7.2更新进度")) {
      ImGui.Text("只更新了绝枪及其职能技能 其他职业待更新");
    }

    if (!ImGui.CollapsingHeader("更新日志")) return;
    ImGui.Text("7/1 自动选中只在pvp生效 修复机工野火目标");
    ImGui.Text("3/8 诗人光阴神提供自定义选择");
    ImGui.Text("3/2 更新了7.1 机工");
    ImGui.Text("2/26 更新了7.1 画蛇");
    ImGui.Text("2/25 提供了只选中玩家和坐骑间隔");
    ImGui.Text("2/20 添加了黑魔自定义配置");
    ImGui.Text("2/19 更新了7.1 黑诗侍赤龙");
    ImGui.Text("12/15 修复战场武士Lb狂按的问题");
    ImGui.Text("12/4 添加了指定坐骑");
    ImGui.Text("11/7 修复黑魔热震荡");
    ImGui.Text("11/2 添加赤魔");
    ImGui.Text("11/1 机工现在会强制锁野火目标了");
    ImGui.Text("11/1 修复冲刺会打断龟壳的问题");
  }
  
  public static void BuildMonitorWindow(JobViewWindow jobViewWindow) {
    if (PvPHelper.通用码权限 || PvPHelper.高级码) {
      ImGui.Checkbox($"启用监控窗口##{28}", ref PvPSettings.Instance.监控);
      ImGui.SliderFloat("监控图片宽", ref PvPSettings.Instance.图片宽1, 0.0f, 1000f);
      ImGui.SliderFloat("监控图片高", ref PvPSettings.Instance.图片高1, 0.0f, 1000f);
      ImGui.Checkbox($"监控布局紧凑##{29}", ref PvPSettings.Instance.紧凑);
      ImGui.Checkbox($"##{35}", ref PvPSettings.Instance.名字);
      ImGui.SameLine();
      ImGui.Checkbox($"##{36}", ref PvPSettings.Instance.血量);
      ImGui.SameLine();
      ImGui.Checkbox($"##{37}", ref PvPSettings.Instance.距离);
      ImGui.SameLine();
      ImGui.Text("显示名字|血量百分比|距离");
      ImGui.Text("紧凑数量");
      ImGui.SameLine();
      ImGui.InputInt($"##{32 /*0x20*/}", ref PvPSettings.Instance.紧凑数量);
      ImGui.Text("监控最大人数");
      ImGui.SameLine();
      ImGui.InputInt($"##{33}", ref PvPSettings.Instance.监控数量);
      ImGui.Checkbox($"监控警报##{30}", ref PvPSettings.Instance.警报);
      ImGui.Text("警报数量人数");
      ImGui.SameLine();
      ImGui.InputInt($"##{34}", ref PvPSettings.Instance.警报数量);
      ImGui.Checkbox($"DevList##{31 /*0x1F*/}", ref PvPSettings.Instance.窗口开关);

      if (PvPSettings.Instance.窗口开关) {
        DrawUnitList();
      }
      PvPSettings.Instance.Save();
    } else {
      ImGui.Text("请获取权限");
    }

    return;

    static void DrawUnitList() {
      ImGui.Columns(7);
      ImGui.Text("ID");
      ImGui.NextColumn();
      ImGui.Text("名称");
      ImGui.NextColumn();
      ImGui.Text("职业");
      ImGui.NextColumn();
      ImGui.Text("血量百分比");
      ImGui.NextColumn();
      ImGui.Text("距离");
      ImGui.NextColumn();
      ImGui.Text("选中目标");
      ImGui.NextColumn();
      ImGui.Text("选中目标ID");
      ImGui.NextColumn();
      ImGui.Separator();

      foreach ((uint key, IBattleChara battleChara) in TargetMgr.Instance.Units) {
        if (battleChara is not { IsDead: false }) continue;
        ImGui.Text($"{key}");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.Name}");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.CurrentJob()}");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.CurrentHpPercent()}");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.DistanceToPlayer():F1}m");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.GetCurrTarget()?.Name}");
        ImGui.NextColumn();
        ImGui.Text($"{battleChara.GetCurrTarget()?.GameObjectId}");
        ImGui.NextColumn();
        ImGui.Separator();
      }

      ImGui.Columns();
    }
  }

  public static void MonitorWindow(ref bool windowOpen) {
    if (!windowOpen) return;

    ImGui.SetNextWindowSize(new Vector2(PvPSettings.Instance.宽1, PvPSettings.Instance.高1),
                            (ImGuiCond)4);
    ImGui.Begin("###targetMe_Window");
    var battleCharaList = PvPTargetHelper.Get看着目标的人(Group.敌人, Core.Me);
    string path = $@"Resources\Images\Number\{
      (battleCharaList.Count <= 4 ? battleCharaList.Count : "4+")
    }.png";

    if (Core.Resolve<MemApiIcon>().TryGetTexture(path, out IDalamudTextureWrap? textureWrap1)) {
      ImGui.Text("    ");
      ImGui.SameLine();

      if (textureWrap1 != null) {
        ImGui.Image(textureWrap1.Handle,
                    new Vector2(PvPSettings.Instance.图片宽1, PvPSettings.Instance.图片高1));
      }

      ImGui.Columns();
    }

    if (PvPSettings.Instance.警报 && (battleCharaList.Count >= PvPSettings.Instance.警报数量)) {
      Core.Resolve<MemApiChatMessage>().Toast2("好像有很多人在看你耶!", 1, 3000);
    }

    if (battleCharaList.Count > 0) {
      int num1 = 1;

      foreach (IBattleChara battleChara in battleCharaList) {
        if (num1 <= PvPSettings.Instance.监控数量) {
          uint num2 = (uint)battleChara.CurrentJob();

          if (Core.Resolve<MemApiIcon>().TryGetTexture($"Resources\\jobs\\{num2}.png",
                                                       out IDalamudTextureWrap? textureWrap2)) {
            if (PvPSettings.Instance.紧凑 && (num1 % PvPSettings.Instance.紧凑数量 == 0) && (num1 != 1)) {
              ImGui.NewLine();
            }

            if (textureWrap2 != null) ImGui.Image(textureWrap2.Handle, new Vector2(50f, 50f));

            if (PvPSettings.Instance.名字) {
              ImGui.SameLine();
              ImGui.Text($"{battleChara.Name}");
            }

            if (PvPSettings.Instance.血量) {
              ImGui.SameLine();
              ImGui.Text($"HP百分比:{battleChara.CurrentHpPercent() * 100f}");
            }

            if (PvPSettings.Instance.距离) {
              ImGui.SameLine();
              ImGui.Text($"距离:{battleChara.DistanceToPlayer():F1}m");
            }
          }

          if (PvPSettings.Instance.紧凑 && (num1 % PvPSettings.Instance.紧凑数量 != 0)) {
            ImGui.SameLine(0.0f, 5f);
          }

          ++num1;
        } else {
          break;
        }
      }
    }

    if (ImGui.Button("关闭##监控")) windowOpen = false;

    ImGui.End();
  }
}
