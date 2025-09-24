using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Log;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.Setting;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Interface.Textures.TextureWraps;
using ECommons.DalamudServices;
using ECommons.GameFunctions;

namespace EZACR_Offline.PvP;

public static class PvPHelper {
  private static readonly List<ulong> 通用码权限列表 = [];
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
  public static readonly HashSet<uint> RestrictedTerritoryIds = [
      狼狱停船厂,
      赤土红沙,
      赤土红沙自定义,
      机关大殿,
      机关大殿自定义,
      角力学校,
      火山之心,
      九霄云上,
      角力学校自定义,
      火山之心自定义,
      九霄云上自定义,
  ];
  
  private static DateTime 冲刺time = DateTime.MinValue;
  private static DateTime 崩破time = DateTime.MinValue;
  private static bool 警报 = true;

  public static unsafe bool LoSBlocked(IBattleChara target) => 
      MemApiSpell.LineOfSightChecker.IsBlocked(Core.Me.GameObject(), 
                                               target.GameObject());

  public static bool CanDispelMe() => Core.Me.HasCanDispel();

  public static Spell SpellNoWaitAcq(uint id, IBattleChara target) {
    return new Spell(id, target) { WaitServerAcq = false, DontUseGcdOpt = true};
  }

  public static Spell SpellWaitAcq(uint id, IBattleChara target) {
    return new Spell(id, target) { WaitServerAcq = true, DontUseGcdOpt = true};
  }

  public static bool 通用码权限 =>
      通用码权限列表.Contains(Svc.ClientState.LocalContentId)
   || (Core.Resolve<MemApiMap>().GetCurrTerrId() == 250);

  public static bool 是否55() {
    return RestrictedTerritoryIds.Contains(Core.Resolve<MemApiMap>().GetCurrTerrId());
  }

  public static bool 高级码 => Share.VIP.Level != 0;

  public static bool CanActive() {
    //const uint recuperate = 29711;  // 热水
    return Core.Me.IsPvP()
        && (通用码权限 || 高级码)
        && (Core.Me.CastActionId != 29055U)
        && (Core.Me.CastActionId != 4U)
        && !Core.Me.HasAura(3054U)
        && !MountHandler.IsMounted();
    //&& !recuperate.RecentlyUsed(3000);
  }

  public static bool HasBuff(IBattleChara? battleChara, uint buffId) {
    return battleChara.HasAura(buffId);
  }

  public static void SkillIcon(uint id) {
    var vector2 = new Vector2(60f, 60f);

    if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out IDalamudTextureWrap? textureWrap)) {
      return;
    }

    if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
  }

  public static Spell? CommonSkillCheck(uint skillid, int distance) {
    IBattleChara? target =
        PvPTargetHelper.TargetSelector.GetSkillTargetSmart(distance + PvPSettings.Instance.长臂猿,
                                                           skillid);
    IBattleChara? nearTarget = PvPTargetHelper.TargetSelector.GetNearestTarget();
    
    switch (skillid) {
      case 29402U or 29403U or 29408U or 29406U or 29407U or 29404U
          when PvPTargetHelper.TargetSelector.GetWildFireTargetSmart() != null:
        return SpellWaitAcq(skillid, PvPTargetHelper.TargetSelector.GetWildFireTargetSmart()!);

      case 29405U when PvPSettings.Instance.技能自动选中: {  // Drill
        if (PvPSettings.Instance.最合适目标
         && target != null
         && target != Core.Me) {
          return SpellWaitAcq(skillid, target);
        }

        if ((nearTarget != null)
         && (nearTarget != Core.Me)) {
          return SpellWaitAcq(skillid, nearTarget);
        }

        break;
      }
    }

    if (PvPSettings.Instance.技能自动选中) {
      if (PvPSettings.Instance.最合适目标
       && target != null
       && target != Core.Me) {
        return SpellWaitAcq(skillid, target);
      }

      if (nearTarget != null
       && nearTarget != Core.Me) {
        return SpellWaitAcq(skillid, nearTarget);
      }
    }

    return (Core.Me.GetCurrTarget() != null) && (Core.Me.GetCurrTarget() != Core.Me)
               ? SpellWaitAcq(skillid, Core.Me.GetCurrTarget()!)
               : null;
  }

  public static void CommonSkillCast(Slot slot, uint skillid, int 距离) {
    slot.Add(CommonSkillCheck(skillid, 距离)!);
  }

  public static bool CommonDistanceCheck(int distance) {
    if (PvPSettings.Instance.技能自动选中) {
      if (((double)PvPTargetHelper.TargetSelector.GetNearestTarget().DistanceToPlayer()
         > distance + PvPSettings.Instance.长臂猿)
       || (PvPTargetHelper.TargetSelector.GetNearestTarget() == null)
       || (PvPTargetHelper.TargetSelector.GetNearestTarget() == Core.Me)) {
        return true;
      }
    } else if ((!PvPSettings.Instance.技能自动选中
             && (Core.Me.GetCurrTarget().DistanceToPlayer()
               > (double)(distance + PvPSettings.Instance.长臂猿)))
            || (Core.Me.GetCurrTarget() == Core.Me)
            || (Core.Me.GetCurrTarget() == null)) {
      return true;
    }

    return false;
  }

  public static bool FixedDistanceCheck(int distance) {
    if (PvPSettings.Instance.技能自动选中) {
      if (((double)PvPTargetHelper.TargetSelector.GetNearestTarget().DistanceToPlayer() > distance)
       || (PvPTargetHelper.TargetSelector.GetNearestTarget() == null)
       || (PvPTargetHelper.TargetSelector.GetNearestTarget() == Core.Me)) {
        return true;
      }
    } else if ((!PvPSettings.Instance.技能自动选中
             && (Core.Me.GetCurrTarget().DistanceToPlayer() > (double)distance))
            || (Core.Me.GetCurrTarget() == Core.Me)
            || (Core.Me.GetCurrTarget() == null)) {
      return true;
    }

    return false;
  }

  public static void AcrInit() {
    LogHelper.Print("如未下载，请下载ElliotZ的镰刀ACR，本ACR需要的公用库有一部分在那边。");
    
    if (通用码权限 || 高级码) {
      return;
    }

    Core.Resolve<MemApiChatMessage>().Toast2("没有权限！", 1, 1500);
  }

  public class SprintTracker {
    private DateTime? _lastSprintTime;
    private bool _isSprinting;

    public static bool IsSprinting => Core.Me.HasAura(1342U);

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

  public static void CommonBattleUpdate(int currTime, IPvPBattleData battleData) {
    IBattleChara? target = Core.Me.GetCurrTarget();
    if (target is not null && target.IsDead) Core.Me.ClearTarget();
    
    // reset hp delta timer
    if (battleData.HPDeltaTime == 0) battleData.HPDeltaTime = currTime;
    // reset hp delta at 2.5 secs since last hp delta write
    if (currTime - battleData.HPDeltaTime >= 2500) {
      battleData.LastHp = (int)Core.Me.CurrentHp;
      battleData.HPDelta = 0;
      battleData.TotalHPDelta = 0;
      battleData.HPDeltaTime = currTime;
    }
    // write hp delta
    if (battleData.LastHp == Core.Me.CurrentHp) return;
    battleData.HPDelta = (int)Core.Me.CurrentHp - battleData.LastHp;
    battleData.LastHp = (int)Core.Me.CurrentHp;
    battleData.TotalHPDelta += battleData.HPDelta;
    battleData.HPDeltaTime = currTime;
  }
}
