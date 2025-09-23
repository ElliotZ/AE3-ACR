using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;

namespace EZACR_Offline.PvP;

public static class PvPTargetHelper {
/*
  private static long 上一次AOE目标搜索时间点;
*/

  public static void 自动选中() {
    if (!Core.Me.IsPvP()
     || (!PvPHelper.通用码权限 && !PvPHelper.高级码)
     || !PvPSettings.Instance.自动选中) {
      return;
    }

    IBattleChara? tar = TargetSelector.GetLowestHPTarget() ?? TargetSelector.GetNearestTarget();

    if ((tar != Core.Me.GetCurrTarget())
     && (tar != Core.Me)
     && (tar != null)) {
      Core.Me.SetTarget(tar);
    }
  }

  public static IBattleChara? 目标模式(int 距离, uint 技能id) {
    if (PvPSettings.Instance.最合适目标 && PvPSettings.Instance.技能自动选中) {
      return TargetSelector.GetSkillTargetSmart(距离 + PvPSettings.Instance.长臂猿, 技能id);
    }

    return PvPSettings.Instance.技能自动选中
               ? TargetSelector.GetNearestTarget()
               : Core.Me.GetCurrTarget();
  }

  private static bool Check目标罩子(IBattleChara? target) {
    return PvPHelper.HasBuff(target, 3054U);
  }

  private static bool Check目标无敌(IBattleChara? target) {
    return PvPHelper.HasBuff(target, 3039U) || PvPHelper.HasBuff(target, 1302U);
  }

  private static bool Check目标地天(IBattleChara? target) {
    return PvPHelper.HasBuff(target, 1240U);
  }

  private static bool Check目标不可攻击(IBattleChara? target) {
    return Check目标无敌(target) || Check目标罩子(target) || Check目标地天(target);
  }

  private static bool Check目标可施法(IBattleChara? target) {
    return target is { IsTargetable: true } && PvPHelper.LoSBlocked(target);
  }

  private static List<uint> _免控buffList => [3054U, 3248U, 1320U];

  public static bool Check目标免控(IBattleChara? target) {
    return (target != null) && target.HasAnyAura(_免控buffList);
  }

  private static bool FilterCharacter(IBattleChara? unit, Filter filter) {
    if (filter == Filter.None) return true;
    if (!Check目标可施法(unit)) return false;

    switch (filter) {
      case Filter.可施法:
        return true;

      case Filter.可攻击:
        if (!Check目标不可攻击(unit)) return true;
        break;
    }

    return ((filter == Filter.无无敌) && !Check目标无敌(unit))
        || ((filter == Filter.可控制) && !Check目标免控(unit));
  }

  private static Dictionary<uint, IBattleChara> Get全部单位(Group type,
                                                        float range = 50f,
                                                        Filter filter = Filter.None) {
    var units = TargetMgr.Instance.Units;
    var dict = new Dictionary<uint, IBattleChara>();

    foreach (IBattleChara unit in units.Values
                                       .Where(unit => FilterCharacter(unit, filter)
                                                   && !unit.IsDead
                                                   && (unit.DistanceToPlayer() <= (double)range))) {
      switch (type) {
        case Group.队友:
          if (!unit.ValidAttackUnit()) {
            AddUnitToDict(dict, unit);
          }

          break;

        case Group.敌人:
          if (unit.ValidAttackUnit()) {
            AddUnitToDict(dict, unit);
          }

          break;

        case Group.全部:
          AddUnitToDict(dict, unit);
          break;
      }
    }

    return dict;
  }

  private static void AddUnitToDict(Dictionary<uint, IBattleChara> dict, IBattleChara unit) {
    uint gameObjectIdAsUint = GetGameObjectIdAsUint(unit.GameObjectId);
    dict.TryAdd(gameObjectIdAsUint, unit);
  }

  private static uint GetGameObjectIdAsUint(ulong gameObjectId) {
    return gameObjectId <= uint.MaxValue
               ? (uint)gameObjectId
               : throw new OverflowException("GameObjectId 超出了 uint 的范围！");
  }

  public static List<IBattleChara> Get看着目标的人(Group type,
                                             IBattleChara target,
                                             float range = 50f) {
    var result = new List<IBattleChara>();
    if (target == null) return result;

    foreach (IBattleChara item in Get全部单位(type, range).Values) {
      if (item.CurrentJob() != Jobs.Any) {
        IBattleChara? currTarget = item.GetCurrTarget();

        if ((currTarget != null)
         && ((long)currTarget.GameObjectId == (long)target.GameObjectId)
         && (item.DistanceToPlayer() <= (double)range)) {
          result.Add(item);
        }
      } else {
        break;
      }
    }

    return result;
  }

  public static class TargetSelector {
    public static IBattleChara? GetSkillTargetSmart(int range, uint skillId) {
      if (!Core.Me.IsPvP()) return null;

      IBattleChara? result = GetWildFireTargetSmart();

      if (result is not null) return result;

      float num = float.MaxValue;

      foreach (IBattleChara t in TargetMgr.Instance.Units.Values) {
        if (t is not { IsTargetable: true }
         || !t.IsEnemy()
         || PvPHelper.LoSBlocked(t)
            // (double) PvPGNBSettings.Instance.爆破血量 / 100.0
         || ((skillId == 29128U) && !(t.CurrentHpPercent() < 0.25))) {
          continue;
        }

        try {
          if (IsValidTarget(t)
           && (t.DistanceToPlayer() <= (double)range)
           && (t.CurrentHp < (double)num)) {
            result = t;
            num = t.CurrentHp;
          }
        } catch (Exception) {
          LogHelper.Error($"血量目标报错:{t.Name ?? "未知目标"}");
        }
      }

      return result;
    }

    public static IBattleChara? GetNearestTarget() {
      if (!Core.Me.IsPvP()) return null;

      int 自动选中自定义范围 = PvPSettings.Instance.自动选中自定义范围;
      IBattleChara? result = null;
      float closestDistance = Math.Min(自动选中自定义范围, 50f);

      foreach (IBattleChara t in TargetMgr.Instance.Units.Values) {
        if (t is not { IsTargetable: true }
         || !t.IsEnemy()
         || PvPHelper.LoSBlocked(t)) {
          continue;
        }

        try {
          if (IsValidTarget(t)
           && (t.DistanceToPlayer() <= (double)自动选中自定义范围)
           && (t.DistanceToPlayer() < (double)closestDistance)) {
            result = t;
            closestDistance = t.DistanceToPlayer();
          }
        } catch (Exception) {
          LogHelper.Error($"报错对象:{t.Name}");
        }
      }

      return result;
    }
    
    public static IBattleChara? GetLowestHPTarget(float threshold = 0.5f) {
      if (!Core.Me.IsPvP()) return null;

      const int range = 25;
      IBattleChara? result = null;
      float minHpPercentage = threshold;

      foreach (IBattleChara t in TargetMgr.Instance.Units.Values) {
        if (t is not { IsTargetable: true }
         || !t.IsEnemy()
         || PvPHelper.LoSBlocked(t)) {
          continue;
        }

        try {
          if (IsValidTarget(t)
           && (t.DistanceToPlayer() <= (double)range)
           && (t.CurrentHpPercent() < (double)minHpPercentage)) {
            result = t;
            minHpPercentage = t.CurrentHpPercent();
          }
        } catch (Exception) {
          LogHelper.Error($"报错对象:{t.Name}");
        }
      }

      return result;
    }

    public static IBattleChara? GetFarthestTarget() {
      if (!Core.Me.IsPvP()) return null;
      int 自动选中自定义范围 = PvPSettings.Instance.自动选中自定义范围;
      IBattleChara? result = null;
      float maxDistance = 0.0f;

      lock (TargetMgr.Instance.Enemys) {
        foreach (IBattleChara t in TargetMgr.Instance.Units.Values) {
          if (t is not { IsTargetable: true }
           || !t.IsEnemy()
           || PvPHelper.LoSBlocked(t)) {
            continue;
          }

          try {
            if (IsValidTarget(t)
             && (t.DistanceToPlayer() <= (double)自动选中自定义范围)
             && (t.DistanceToPlayer() > (double)maxDistance)) {
              result = t;
              maxDistance = t.DistanceToPlayer();
            }
          } catch (Exception) {
            LogHelper.Error($"报错对象:{t.Name}");
          }
        }
      }

      return result;
    }

    public static IBattleChara? GetWildFireTargetSmart() {
      if (!Core.Me.IsPvP() || (Core.Me.CurrentJob() != Jobs.Machinist)) {
        return null;
      }

      int 自动选中自定义范围 = PvPSettings.Instance.自动选中自定义范围;
      IBattleChara? result = null;
      float closestDistance = Math.Min(自动选中自定义范围, 50f);

      lock (TargetMgr.Instance.Enemys) {
        foreach (IBattleChara t in TargetMgr.Instance.EnemysIn25.Values) {
          if (t is not { IsTargetable: true }
           || !t.IsEnemy()
           || PvPHelper.LoSBlocked(t)) {
            continue;
          }

          try {
            if (IsValidTarget(t)
             && t.HasLocalPlayerAura(1323U)
             && (t.DistanceToPlayer() <= (double)自动选中自定义范围)
             && (t.DistanceToPlayer() < (double)closestDistance)) {
              result = t;
              closestDistance = t.DistanceToPlayer();
            }
          } catch (Exception) {
            LogHelper.Error($"报错对象:{t.Name}");
          }
        }
      }

      return result;
    }

    public static IBattleChara? Get多斩Target(int 多斩count) {
      if (!Core.Me.IsPvP() || (Core.Me.LimitBreakCurrentValue() < 4000)) return null;
      Dictionary<uint, IBattleChara>.ValueCollection values = TargetMgr.Instance.EnemysIn25.Values;
      List<IBattleChara> battleCharaList = [];
      battleCharaList.AddRange(
          values.Select(battleChara => new {
                    battleChara,
                    num = battleChara.CurrentHpPercent() + battleChara.ShieldPercentage / 100f,
                })
                .Where(@t => @t.battleChara.HasLocalPlayerAura(3202U)
                          && !@t.battleChara.HasAura(2413U)
                          && !@t.battleChara.HasAura(1301U)
                          && (@t.num <= 1.0))
                .Select(@t => @t.battleChara));

      if (battleCharaList.Count == 0) {
        return null;
      }

      foreach (IBattleChara source in battleCharaList) {
        int tgtCount = 0;

        foreach (IBattleChara unused in battleCharaList.Where(
                     target => ((int)source.DataId != (int)target.DataId)
                            && (source.Distance(target) <= 5.0))) {
          ++tgtCount;

          if (tgtCount >= 多斩count) {
            return source.IsTargetable ? source : null;
          }
        }
      }

      return null;
    }

    public static IBattleChara? Get斩铁目标() {
      if (!Core.Me.IsPvP() || (Core.Me.LimitBreakCurrentValue() < 4000)) {
        return null;
      }

      return TargetMgr.Instance.EnemysIn25.Values
                      .FirstOrDefault(Is斩铁目标Eligible);
    }

    private static bool Is斩铁目标Eligible(IBattleChara enemy) {
      float num = enemy.CurrentHpPercent() + enemy.ShieldPercentage / 100f;
      return enemy.IsTargetable
           & (num <= 1.0)
           & enemy.HasLocalPlayerAura(3202U)
           & (!enemy.HasAura(3039U)
           && !enemy.HasAura(2413U)
           && !enemy.HasAura(1301U));
    }
  }

  private static bool IsValidTarget(IBattleChara t) {
    return !t.IsDead
        && !t.HasAura(3039U)
        && !t.HasAura(2413U)
        && !t.HasAura(1301U)
        && !t.HasAura(1302U)
        && (!PvPSettings.Instance.不选冰 || (t.CurrentJob() != Jobs.Any))
        && (Core.Me.HasAura(4315U) || !t.HasAura(3054U))
        && !t.HasAura(1240U);
  }
}
