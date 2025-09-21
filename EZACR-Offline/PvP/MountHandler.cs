using AEAssist;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects.Types;
using ECommons.DalamudServices;

namespace EZACR_Offline.PvP;

public static class MountHandler {
  private static DateTime _lastMountTime;
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

  private static bool check坐骑() {
    return Svc.Condition[ConditionFlag.Mounted];
  }

  public static void 无目标坐骑() {
    if (!Core.Me.IsPvP()
     || (!PVPHelper.通用码权限 && !PVPHelper.高级码)
     || !CanUseMount()
     || (Core.Me.GetCurrTarget() != null)
     || (TargetHelper.GetNearbyEnemyCount(Core.Me,
                                          PvPSettings.Instance.无目标坐骑范围,
                                          PvPSettings.Instance.无目标坐骑范围)
       > 1)
     || 29055U.RecentlyUsed(2000)
     || IsMountCooldownInEffect()) {
      return;
    }

    if (PvPSettings.Instance.指定坐骑) {
      Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
      Core.Resolve<MemApiSendMessage>().SendMessage("/mount " + PvPSettings.Instance.坐骑名);
    } else {
      Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
      Core.Resolve<MemApiSendMessage>().SendMessage("/mount 专属陆行鸟");
    }

    _lastMountTime = DateTime.Now;
  }

  private static bool IsMounted() {
    return Svc.Condition[ConditionFlag.Mounted];
  }

  public static void UseMountWithoutTarget() {
    if (!CanUseMount()
     || (TargetHelper.GetNearbyEnemyCount(Core.Me,
                                          PvPSettings.Instance.无目标坐骑范围,
                                          PvPSettings.Instance.无目标坐骑范围)
       > 1)
     || 29055U.RecentlyUsed(2000)
     || IsMountCooldownInEffect()) {
      return;
    }

    Core.Resolve<MemApiSendMessage>().SendMessage("/mcancel");
    Core.Resolve<MemApiSendMessage>().SendMessage("/ac 随机坐骑");
    _lastMountTime = DateTime.Now;
  }

  private static bool CanUseMount() {
    return (PvPSettings.Instance.无目标坐骑
         && (GCDHelper.GetGCDCooldown() == 0)
         && !IsMounted()
         && (Core.Me.GetCurrTarget() == null))
        || ((Core.Me.GetCurrTarget().DistanceToPlayer() > 80.0)
         && !Core.Me.IsCasting
         && Core.Me.IsPvP()
         && !IsInRestrictedTerritory());
  }

  private static bool IsInRestrictedTerritory() {
    uint currTerrId = Core.Resolve<MemApiMap>().GetCurrTerrId();
    return RestrictedTerritoryIds.Contains(currTerrId);
  }

  private static bool IsMountCooldownInEffect() {
    return (DateTime.Now - _lastMountTime).TotalSeconds < PvPSettings.Instance.坐骑cd;
  }
}
