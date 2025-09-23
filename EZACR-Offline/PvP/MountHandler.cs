using AEAssist;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Conditions;
using ECommons.DalamudServices;

namespace EZACR_Offline.PvP;

public static class MountHandler {
  private static DateTime _lastMountTime;

  public static bool Check坐骑() {
    return Svc.Condition[ConditionFlag.Mounted];
  }

  public static void 无目标坐骑() {
    if (!PvPHelper.通用码权限 && !PvPHelper.高级码
     || !CanUseMount()
     || 29055U.RecentlyUsed(2000)
     || IsMountCooldownInEffect()) {
      return;
    }

    if (PvPSettings.Instance.指定坐骑) {
      Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
      Core.Resolve<MemApiSendMessage>().SendMessage("/mount " + PvPSettings.Instance.坐骑名);
    } else {
      Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
      Core.Resolve<MemApiSendMessage>().SendMessage("/gaction 随机坐骑");
    }

    _lastMountTime = DateTime.Now;
  }

  public static void Dismount() {
    if (!IsMounted()) return;
    Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
    Core.Resolve<MemApiSendMessage>().SendMessage("/gaction 随机坐骑");
  }

  public static bool IsMounted() {
    return Svc.Condition[ConditionFlag.Mounted];
  }

  public static bool CanUseMount() {
    return PvPSettings.Instance.无目标坐骑
        && Core.Me.IsPvP()
        //&& GCDHelper.GetGCDCooldown() == 0
        && !IsMounted()
        && (Core.Me.GetCurrTarget() == null || Core.Me.GetCurrTarget().DistanceToPlayer() > 80.0)
        && TargetHelper.GetNearbyEnemyCount(PvPSettings.Instance.无目标坐骑范围) < 1
        && !Core.Me.IsCasting
        && !IsInRestrictedTerritory();
  }

  private static bool IsInRestrictedTerritory() {
    uint currTerrId = Core.Resolve<MemApiMap>().GetCurrTerrId();
    return PvPHelper.RestrictedTerritoryIds.Contains(currTerrId);
  }

  private static bool IsMountCooldownInEffect() {
    return (DateTime.Now - _lastMountTime).TotalSeconds < PvPSettings.Instance.坐骑cd;
  }
}
