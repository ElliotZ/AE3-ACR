using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using AEAssist.Verify;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons.DalamudServices;
using ECommons.GameFunctions;
using ECommons.WindowsFormsReflector;
using ElliotZ;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP;

public class PVPHelper {
  public static List<ulong> 通用码权限列表 = new() {
      18014469511346939UL,
      18014469510423537UL,
      18014469510702542UL,
      18014469510525313UL,
      18014479510723122UL,
      18014469509612031UL,
      18014398555212021UL,
      19014409512829860UL,
      19014409517683706UL,
      18014479511064381UL,
      18014469509698926UL,
      19014409515975551UL,
      18014398549659316UL,
      19014409511786239UL,
      19014409515975799UL,
      18014479510257104UL,
      19014409516898973UL,
  };
  private const uint 狼狱停船厂 = 250;
  private const uint 赤土红沙 = 1138;
  private const uint 赤土红沙自定义 = 1139;
  private const uint 机关大殿 = 1116;
  private const uint 机关大殿自定义 = 1117;
  private const uint 角力学校 = 1032;
  private const uint 火山之心 = 1033;
  private const uint 九霄云上 = 1034;
  private const uint 角力学校自定义 = 1058;
  private const uint 火山之心自定义 = 1059;
  private const uint 九霄云上自定义 = 1060;
  public static readonly HashSet<uint> RestrictedTerritoryIds = new() {
      250U,
      1138U,
      1139U,
      1116U,
      1117U,
      1032U,
      1033U,
      1034U,
      1058U,
      1059U,
      1060U,
  };
  private static IBattleChara? Target;
  private static DateTime 冲刺time = DateTime.MinValue;
  private static DateTime 崩破time = DateTime.MinValue;
  private static bool 警报 = true;

  public static Vector3 向量位移(Vector3 position, float facingRadians, float distance) {
    float num1 = (float)Math.Sin(facingRadians) * distance;
    float num2 = (float)Math.Cos(facingRadians) * distance;
    return new Vector3(position.X + num1, position.Y + 5f, position.Z + num2);
  }

  public static Vector3 向量位移反向(Vector3 position, float facingRadians, float distance) {
    float num1 = (float)(Math.Sin(facingRadians) * -(double)distance);
    float num2 = (float)(Math.Cos(facingRadians) * -(double)distance);
    return new Vector3(position.X + num1, position.Y + 5f, position.Z + num2);
  }

  internal static float GetCameraRotation() {
    return CameraHelper.GetCameraRotation();
  }

  internal static float GetCameraRotation反向() {
    float cameraRotation反向 = GetCameraRotation() + (float)Math.PI;

    if (cameraRotation反向 > Math.PI) {
      cameraRotation反向 -= (float)(2 * Math.PI);
    } else if (cameraRotation反向 < -1.0 * Math.PI) {
      cameraRotation反向 += (float)(2 * Math.PI);
    }

    return cameraRotation反向;
  }

  public static unsafe bool 视线阻挡(IBattleChara 目标角色) => 
      MemApiSpell.LineOfSightChecker.IsBlocked(Core.Me.GameObject(), 
                                               目标角色.GameObject());

  public static bool 净化判断() => Core.Me.HasCanDispel();

  public static Spell 不等服务器Spell(uint id, IBattleChara target) {
    return new Spell(id, target) { WaitServerAcq = false };
  }

  public static Spell 等服务器Spell(uint id, IBattleChara target) {
    return new Spell(id, target) { WaitServerAcq = true };
  }

  public static bool 通用码权限 =>
      通用码权限列表.Contains(Svc.ClientState.LocalContentId)
   || (Core.Resolve<MemApiMap>().GetCurrTerrId() == 250);

  public static bool 是否55() {
    return RestrictedTerritoryIds.Contains(Core.Resolve<MemApiMap>().GetCurrTerrId());
  }

  public static bool 高级码 => Share.VIP.Level != 0;

  public static bool Check坐骑() {
    return Svc.Condition[ConditionFlag.Mounted];
  }

  public static bool CanActive() {
    uint spellId = 29711;
    return Core.Me.IsPvP()
        && (通用码权限 || 高级码)
        && (Core.Me.CastActionId != 29055U)
        && (Core.Me.CastActionId != 4U)
        && !Core.Me.HasAura(3054U)
        && !Check坐骑()
        && !spellId.RecentlyUsed(3000);
  }

  public static bool HasBuff(IBattleChara? battleChara, uint buffId) {
    return battleChara.HasAura(buffId);
  }

  public static void 技能图标(uint id) {
    var vector2 = new Vector2(60f, 60f);

    if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out IDalamudTextureWrap? textureWrap)) {
      return;
    }

    if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
  }

  private static void s1() {
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"米内有敌人时不用##{228}", ref PvPSettings.Instance.无目标坐骑范围);
    ImGui.PopItemWidth();
  }

  private static void s2() {
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"米内最近目标##{229}", ref PvPSettings.Instance.自动选中自定义范围);
    ImGui.PopItemWidth();
  }

  private static void s3() {
    ImGui.PushItemWidth(100f);
    ImGui.InputInt($"秒##{230}", ref PvPSettings.Instance.坐骑cd);
    ImGui.PopItemWidth();
  }

  public static void 通用设置配置() {
    ImGui.Text("共通配置");
    ImGui.PushItemWidth(100f);

    if (ImGui.InputInt($"攻击判定增加(仅影响ACR判断,配合长臂猿使用)##{66}", ref PvPSettings.Instance.长臂猿, 1, 1)) {
      PvPSettings.Instance.长臂猿 = Math.Clamp(PvPSettings.Instance.长臂猿, 0, 5);
    }

    ImGui.Text("自动选中默认排除:\n不死救赎3039 神圣领域1302 被保护2413 龟壳3054 地天1240");
    ImGui.Checkbox($"自动选中(只在PvP生效)##{666}", ref PvPSettings.Instance.自动选中);
    ImGui.SameLine();
    s2();
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
    s1();
    ImGui.Text("自动坐骑尝试间隔");
    ImGui.SameLine();
    s3();
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

  public static void 权限获取() {
    string text = Svc.ClientState.LocalContentId.ToString();
    ImGui.Text($"当前的码等级：[{Share.VIP.Level}]");

    if ((Share.VIP.Level == VIPLevel.Normal) && 高级码) {
      ImGui.Text("仅狼狱可用 战场无权限");
    }

    if (!通用码权限 && !高级码) {
      ImGui.TextColored(new Vector4(1f, 0.0f, 0.0f, 0.8f), "无权限");
      ImGui.SameLine();

      if (ImGui.Button("复制CID到剪贴板")) {
        Winforms.Clipboard.SetText(text);
        LogHelper.Print("已复制CID到剪贴板");
      }
    }

    if (!通用码权限 && !高级码) {
      return;
    }

    ImGui.TextColored(new Vector4(0.16470589f, 0.84313726f, 0.22352941f, 0.8f), "已解锁");
  }

  public static void 技能配置(uint 技能图标id, string 技能名字, string 描述文字, ref bool 切换配置, int id) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text($"{描述文字}: {切换配置}");

    if (ImGui.Button($"切换##{id}")) {
      切换配置 = !切换配置;
    }

    ImGui.Columns();
  }

  public static void 技能配置2(uint 技能图标id, string 技能名字, string 描述文字, ref bool 切换配置, int id) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text(描述文字 + ":");
    ImGui.Checkbox($"##{id}", ref 切换配置);
    ImGui.Columns();
  }

  public static void 技能配置3(
      uint 技能图标id,
      string 技能名字,
      string 描述文字,
      ref int 数值,
      int 幅度,
      int 快速幅度,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text(描述文字 + ":");
    ImGui.InputInt($"##{id}", ref 数值, 幅度, 快速幅度);
    ImGui.Columns();
  }

  public static void 技能配置4(
      uint 技能图标id,
      string 技能名字,
      string 数值描述,
      string 描述文字,
      ref bool 切换配置,
      ref int 数值,
      int 幅度,
      int 快速幅度,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text(描述文字 + ":");
    ImGui.Checkbox($"##{id}", ref 切换配置);
    ImGui.Text(数值描述 + ":");
    ImGui.InputInt($"##{id}+1", ref 数值, 幅度, 快速幅度);
    ImGui.Columns();
  }

  public static void 技能配置5(
      uint 技能图标id,
      string 技能名字,
      string IntDescription,
      ref float value,
      float min,
      float max,
      int id) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text(IntDescription + ":");
    ImGui.SliderFloat($"##{id}", ref value, min, max);
    ImGui.Columns();
  }

  public static void 技能解释(uint 技能图标id, string 技能名字, string 描述文字) {
    ImGui.Separator();
    ImGui.Columns(2, (string)null, false);
    ImGui.SetColumnWidth(0, 70f);
    技能图标(技能图标id);
    ImGui.NextColumn();
    ImGui.SetColumnWidth(1, 150f);
    ImGui.Text(技能名字);
    ImGui.Text(描述文字 + ":");
    ImGui.Columns();
  }

  public static Spell? 通用技能释放Check(uint skillid, int 距离) {
    switch (skillid) {
      case 29402U or 29403U or 29408U or 29406U or 29407U or 29404U
          when PvPTargetHelper.TargetSelector.Get野火目标() != null:
        return 等服务器Spell(skillid, PvPTargetHelper.TargetSelector.Get野火目标()!);

      case 29405U when PvPSettings.Instance.技能自动选中: {
        if (PvPSettings.Instance.最合适目标
         && (PvPTargetHelper.TargetSelector.Get最合适目标(距离 + PvPSettings.Instance.长臂猿, 29405U) != null)
         && (PvPTargetHelper.TargetSelector.Get最合适目标(距离 + PvPSettings.Instance.长臂猿, 29405U)
          != Core.Me)) {
          return 等服务器Spell(skillid,
                           PvPTargetHelper.TargetSelector.Get最合适目标(
                               距离 + PvPSettings.Instance.长臂猿,
                               29405U)!);
        }

        if ((PvPTargetHelper.TargetSelector.Get最近目标() != null)
         && (PvPTargetHelper.TargetSelector.Get最近目标() != Core.Me)) {
          return 等服务器Spell(skillid, PvPTargetHelper.TargetSelector.Get最近目标()!);
        }

        break;
      }
    }

    if (PvPSettings.Instance.技能自动选中) {
      if (PvPSettings.Instance.最合适目标
       && (PvPTargetHelper.TargetSelector.Get最合适目标(距离 + PvPSettings.Instance.长臂猿, skillid) != null)
       && (PvPTargetHelper.TargetSelector.Get最合适目标(距离 + PvPSettings.Instance.长臂猿, skillid)
        != Core.Me)) {
        return 等服务器Spell(skillid,
                         PvPTargetHelper.TargetSelector.Get最合适目标(
                             距离 + PvPSettings.Instance.长臂猿,
                             skillid)!);
      }

      if ((PvPTargetHelper.TargetSelector.Get最近目标() != null)
       && (PvPTargetHelper.TargetSelector.Get最近目标() != Core.Me)) {
        return 等服务器Spell(skillid, PvPTargetHelper.TargetSelector.Get最近目标()!);
      }
    }

    return (Core.Me.GetCurrTarget() != null) && (Core.Me.GetCurrTarget() != Core.Me)
               ? 等服务器Spell(skillid, Core.Me.GetCurrTarget()!)
               : null;
  }

  public static void 通用技能释放(Slot slot, uint skillid, int 距离) {
    slot.Add(通用技能释放Check(skillid, 距离));
  }

  public static bool 通用距离检查(int 距离) {
    if (PvPSettings.Instance.技能自动选中) {
      if (((double)PvPTargetHelper.TargetSelector.Get最近目标().DistanceToPlayer()
         > 距离 + PvPSettings.Instance.长臂猿)
       || (PvPTargetHelper.TargetSelector.Get最近目标() == null)
       || (PvPTargetHelper.TargetSelector.Get最近目标() == Core.Me)) {
        return true;
      }
    } else if ((!PvPSettings.Instance.技能自动选中
             && (Core.Me.GetCurrTarget().DistanceToPlayer()
               > (double)(距离 + PvPSettings.Instance.长臂猿)))
            || (Core.Me.GetCurrTarget() == Core.Me)
            || (Core.Me.GetCurrTarget() == null)) {
      return true;
    }

    return false;
  }

  public static bool 固定距离检查(int 距离) {
    if (PvPSettings.Instance.技能自动选中) {
      if (((double)PvPTargetHelper.TargetSelector.Get最近目标().DistanceToPlayer() > 距离)
       || (PvPTargetHelper.TargetSelector.Get最近目标() == null)
       || (PvPTargetHelper.TargetSelector.Get最近目标() == Core.Me)) {
        return true;
      }
    } else if ((!PvPSettings.Instance.技能自动选中
             && (Core.Me.GetCurrTarget().DistanceToPlayer() > (double)距离))
            || (Core.Me.GetCurrTarget() == Core.Me)
            || (Core.Me.GetCurrTarget() == null)) {
      return true;
    }

    return false;
  }

  public static void 配置(JobViewWindow jobViewWindow) {
    通用设置配置();
  }

  public static void 更新日志(JobViewWindow jobViewWindow) { }

  public static void PvP调试窗口() {
    if (Svc.ClientState.LocalContentId == 18014469511346939UL) {
      ImGui.Begin("调试窗口");
      ImGui.Text($"gcd:{GCDHelper.GetGCDCooldown()}");
      ImGui.Text($"CastActionId:{Core.Me.CastActionId}");
      ImGui.Text($"是否55:{是否55()}");
      SeString seString = PvPTargetHelper.TargetSelector.Get最近目标()?.Name ?? (SeString)"无";
      ImGui.Text($"视线阻挡: {视线阻挡(Core.Me.GetCurrTarget())}");
      ImGui.Text($"最近目标: {seString}");
      ImGui.Text($"最合适25米目标: {
        PvPTargetHelper.TargetSelector.Get最合适目标(25, 1U)?.Name ?? (SeString)"无"
      }");
      ImGui.Text($"自己：{Core.Me.Name},{Core.Me.DataId},{Core.Me.Position}");
      ImGui.Text($"坐骑状态：{Svc.Condition[ConditionFlag.Mounted]}");
      ImGui.Text($"血量百分比：{Core.Me.CurrentHpPercent()}");
      ImGui.Text($"盾值百分比：{Core.Me.ShieldPercentage / 100f}");
      ImGui.Text($"血量百分比：{Core.Me.CurrentHpPercent() + Core.Me.ShieldPercentage / 100.0 <= 1.0}");
      Core.Me.GetCurrTarget();
      ImGui.Text($"目标：{
        Core.Me.GetCurrTarget().Name ?? (SeString)"无"
      },{
        Core.Me.GetCurrTarget().DataId
      },{
        Core.Me.GetCurrTarget().Position
      }");
      ImGui.Text($"是否移动：{MoveHelper.IsMoving()}");
      ImGui.Text($"小队人数：{PartyHelper.CastableParty.Count}");
      ImGui.Text($"25米内敌方人数：{TargetHelper.GetNearbyEnemyCount(Core.Me, 1, 25)}");
      ImGui.Text($"20米内小队人数：{PartyHelper.CastableAlliesWithin20.Count}");
      ImGui.Text($"目标5米内人数：{TargetHelper.GetNearbyEnemyCount(Core.Me.GetCurrTarget(), 25, 5)}");
      ImGui.Text($"LB槽当前数值：{Core.Me.LimitBreakCurrentValue()}");
      ImGui.Text($"上个技能：{Core.Resolve<MemApiSpellCastSuccess>().LastSpell}");
      ImGui.Text($"上个GCD：{Core.Resolve<MemApiSpellCastSuccess>().LastGcd}");
      ImGui.Text($"上个能力技：{Core.Resolve<MemApiSpellCastSuccess>().LastAbility}");
      ImGui.Text($"上个连击技能：{Core.Resolve<MemApiSpell>().GetLastComboSpellId()})");
      ImGui.Text($"t野火目标：{PvPTargetHelper.TargetSelector.Get野火目标()?.Name ?? (SeString)"无"})");
      ImGui.Text($"技能变化：{Core.Resolve<MemApiSpell>().CheckActionChange(29102U)})");
      ImGui.Text($"烈牙cd：{29102U.GetSpell().IsReadyWithCanCast()})");
      ImGui.Text($"烈牙2cd：{29103U.GetSpell().IsReadyWithCanCast()})");
      ImGui.Text($"自身技能：{Core.Me.HasLocalPlayerAura(2282U)})");
      ImGui.Text($"IsUnlockWithCDCheck：{29649U.IsUnlockWithCDCheck()})");
      ImGui.Text($"IsReadyWithCanCast：{29649U.GetSpell().IsReadyWithCanCast()})");

      if (ImGui.Button("null")) {
        Svc.Targets.Target = null;
      }

      if (ImGui.Button("最远")) {
        Svc.Targets.Target = (IGameObject)PvPTargetHelper.TargetSelector.Get最远目标();
      }

      if (ImGui.Button("1")) {
        Core.Resolve<MemApiMove>().SetRot(GetCameraRotation反向());
      }

      if (ImGui.Button("21")) {
        Core.Resolve<MemApiMove>().SetRot(GetCameraRotation());
      }

      ImGui.Text($"？：{Core.Me.HasAura(3212U)}");

      if (!ImGui.Button("21")) {
        ;
      }

      ImGui.End();
    } else {
      ImGui.Text("你不需要用到调试");
    }
  }

  public static void 进入ACR() {
    if (通用码权限 || 高级码) {
      return;
    }

    Core.Resolve<MemApiChatMessage>().Toast2("没有权限！", 1, 1500);
  }

  public static void 战斗状态() { }

  public static void 监控(JobViewWindow jobViewWindow) {
    if (通用码权限 || 高级码) {
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

      监控窗口();
      PvPSettings.Instance.Save();
    } else {
      ImGui.Text("请获取权限");
    }

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

      foreach (var unit in TargetMgr.Instance.Units) {
        uint key = unit.Key;
        IBattleChara battleChara = unit.Value;

        if ((battleChara != null) && !battleChara.IsDead) {
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
      }

      ImGui.Columns();
    }
  }

  public static void 监控窗口() {
    if (!PvPSettings.Instance.监控) {
      return;
    }

    ImGui.SetNextWindowSize(new Vector2(PvPSettings.Instance.宽1, PvPSettings.Instance.高1),
                            (ImGuiCond)4);
    ImGui.Begin("###targetMe_Window");
    var battleCharaList = PvPTargetHelper.Get看着目标的人(Group.敌人, (IBattleChara)Core.Me);
    string path = $"Resources\\Images\\Number\\{
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

    ImGui.End();
  }

  public class SprintTracker {
    private DateTime? _lastSprintTime;
    private bool _isSprinting;

    public bool IsSprinting => Core.Me.HasAura(1342U);

    public void CheckAndTrackSprint() {
      if (IsSprinting && !_isSprinting) {
        _lastSprintTime = DateTime.Now;
      }

      _isSprinting = IsSprinting;
    }

    public bool CheckSprint() {
      return _isSprinting
          && _lastSprintTime.HasValue
          && ((DateTime.Now - _lastSprintTime.Value).TotalSeconds < PvPSettings.Instance.冲刺);
    }
  }
}
