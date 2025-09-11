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

public class Base : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Gcd;

  public static Spell GetSpell() {
    if (Qt.Instance.GetQt("AOE") && (Core.Me.Level > 10)) {
      int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 8, 8);
      int nearbyEnemyCount2 = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);

      if (Qt.Instance.GetQt("自动拉怪") && (PartyHelper.Party.Count <= 4)) {
        if ((Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141)
         && (Core.Me.Level >= 40)
         && (nearbyEnemyCount >= 2)) {
          return 16149u.GetSpell();
        }

        return 16141u.GetSpell();
      }

      if ((Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141) && (Core.Me.Level >= 40)) {
        if ((Core.Me.Level < 88) && (Core.Resolve<JobApi_GunBreaker>().Ammo < 2)) {
          return 16149u.GetSpell();
        }

        if ((Core.Me.Level >= 88) && (Core.Resolve<JobApi_GunBreaker>().Ammo < 3)) {
          return 16149u.GetSpell();
        }
      }

      if ((nearbyEnemyCount2 >= 2) && (GnbSettings.Instance.ACRMode != "Normal")) {
        if ((Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141) && (Core.Me.Level >= 40)) {
          return 16149u.GetSpell();
        }

        return 16141u.GetSpell();
      }

      if ((nearbyEnemyCount >= 2) && (GnbSettings.Instance.ACRMode == "Normal")) {
        if ((Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141) && (Core.Me.Level >= 40)) {
          return 16149u.GetSpell();
        }

        return 16141u.GetSpell();
      }
    }

    if ((Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139) && (Core.Me.Level >= 26)) {
      return 16145u.GetSpell();
    }

    if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16137) return 16139u.GetSpell();
    return 16137u.GetSpell();
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

    if (!Qt.Instance.GetQt("使用基础Gcd")) return -3;

    if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox)
      > (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange) {
      return -1;
    }

    if (Qt.Instance.GetQt("自动拉怪")) return 1;

    if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16146) {
      return -20; // while in gnashing fang
    }

    if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16147) return -30;

    if (Core.Me.Level >= 60) {
      if ((Core.Resolve<JobApi_GunBreaker>().Ammo == 3)
       && (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139)
       && Qt.Instance.GetQt("爆发")
       && Qt.Instance.GetQt("子弹连")
       && 16146u.CoolDownInGCDs(1)) // gnashing fang
      {
        return -5;
      }

      if (SettingMgr.GetSetting<GeneralSettings>().OptimizeGcd
       && (Core.Resolve<JobApi_GunBreaker>().Ammo >= 2)
       && Qt.Instance.GetQt("爆发")
       && (25760u.GetSpell().Cooldown.TotalMilliseconds > 0.0)
       && (25760u.GetSpell().Cooldown.TotalMilliseconds <= 500.0)) // double down
      {
        return -55;
      }

      if (SettingMgr.GetSetting<GeneralSettings>().OptimizeGcd
       && (Core.Resolve<JobApi_GunBreaker>().Ammo >= 1)
       && Qt.Instance.GetQt("爆发")
       && Qt.Instance.GetQt("子弹连")
       && (16146u.GetSpell().Cooldown.TotalMilliseconds > 0.0)
       && (16146u.GetSpell().Cooldown.TotalMilliseconds <= 500.0)) // gnashing fang
      {
        return -54;
      }
    }

    return 0;
  }

  public void Build(Slot slot) {
    Spell spell = GetSpell();
    if (spell != null) slot.Add(spell);
  }
}
