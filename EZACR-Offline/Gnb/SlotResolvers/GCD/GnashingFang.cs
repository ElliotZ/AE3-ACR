using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class GnashingFang : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Gcd;

  private static bool CheckSpell() {
    if (Core.Resolve<MemApiSpell>().CheckActionChange(16146u).GetSpell().IsReadyWithCanCast()) {
      return true;
    }

    return false;
  }

  public int Check() {
    //if (Core.Me.HasAnyAura(Buff.无法发动技能类))
    //{
    //    return -150;
    //}

    //if (Core.Me.HasAnyAura(Buff.无法造成伤害))
    //{
    //    return -151;
    //}

    //if (Core.Me.GetCurrTarget().HasAnyAura(Buff.敌人无敌BUFF))
    //{
    //    return -152;
    //}

    if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox)
      > (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange) {
      return -11;
    }

    if (Qt.Instance.GetQt("自动拉怪")) return -1;

    if (!Qt.Instance.GetQt("爆发")) return -2;

    if (!Qt.Instance.GetQt("子弹连")) return -3;

    //if (!战斗爽Helper.战斗爽())
    //{
    //    return -2;
    //}

    if ((Core.Resolve<MemApiSpell>().CheckActionChange(36937u) == 36938)
     || (Core.Resolve<MemApiSpell>().CheckActionChange(36937u) == 36939)) {
      return -25;
    }

    if ((Core.Me.Level < 100)
     && 25760u.IsUnlock()
     && Qt.Instance.GetQt("倍攻")
     && (25760u.GetSpell().IsReadyWithCanCast() || 25760u.CoolDownInGCDs(3))) {
      return -4;
    }

    if (Core.Me.Level >= 100) {
      if (25760u.GetSpell().IsReadyWithCanCast()
       && Qt.Instance.GetQt("倍攻")
       && (Core.Resolve<MemApiSpell>().CheckActionChange(16146u) == 16147)
       && (Core.Resolve<JobApi_GunBreaker>().Ammo >= 1)) {
        return -8;
      }

      if (25760u.CoolDownInGCDs(1)
       && Qt.Instance.GetQt("倍攻")
       && (Core.Me.HasAura(1831u) || Qt.Instance.GetQt("无视无情"))
       && (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139)
       && (Core.Resolve<JobApi_GunBreaker>().Ammo == 1)
       && !16147u.GetSpell().IsReadyWithCanCast()
       && !16150u.GetSpell().IsReadyWithCanCast()) {
        return -9;
      }
    }

    if (!16146u.GetSpell().IsReadyWithCanCast()) {
      if (16147u.GetSpell().IsReadyWithCanCast()
       && (!25760u.GetSpell().IsReadyWithCanCast() || !Qt.Instance.GetQt("倍攻"))) {
        return 155;
      }

      if (16150u.GetSpell().IsReadyWithCanCast()
       && (!25760u.GetSpell().IsReadyWithCanCast() || !Qt.Instance.GetQt("倍攻"))) {
        return 156;
      }

      return -45;
    }

    int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);

    if (Qt.Instance.GetQt("倾泻爆发")
     && (Core.Resolve<JobApi_GunBreaker>().Ammo > 0)
     && (nearbyEnemyCount < 3)
     && (Core.Me.Level >= 72)) {
      return 10;
    }

    if (Core.Resolve<JobApi_GunBreaker>().Ammo <= 0) return -60;

    if ((Core.Me.Level >= 72) && (nearbyEnemyCount >= 3)) return -9;

    if (16138u.CoolDownInGCDs(2)
     && !Qt.Instance.GetQt("无视无情")
     && !16147u.GetSpell().IsReadyWithCanCast()
     && !16150u.GetSpell().IsReadyWithCanCast()) {
      return -5;
    }

    return 0;
  }

  public void Build(Slot slot) {
    slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(16146u).GetSpell());
  }
}
