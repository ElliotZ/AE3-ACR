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

public class SonicBreak : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Gcd;

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

    if (!Core.Me.HasAura(3886u)) return -88;
    if (Core.Me.Level < 54) return -51;
    if (Qt.Instance.GetQt("自动拉怪")) return -1;

    if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox)
      > (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange) {
      return -16;
    }

    if (!16153u.GetSpell().IsReadyWithCanCast()) return -50;
    if (16153u.RecentlyUsed() && !Qt.Instance.GetQt("优先音速破")) return -5;

    if (16146u.GetSpell().IsReadyWithCanCast()
     && Qt.Instance.GetQt("子弹连")
     && !Qt.Instance.GetQt("优先音速破")) {
      return -6;
    }

    if (Qt.Instance.GetQt("倾泻爆发")) return 4;
    if (!Qt.Instance.GetQt("爆发")) return -10;
    if (!Qt.Instance.GetQt("音速破")) return -11;

    if (16146u.CoolDownInGCDs(2)
     && Qt.Instance.GetQt("子弹连")
     && !Qt.Instance.GetQt("优先音速破")
     && (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139)
     && (Core.Resolve<JobApi_GunBreaker>().Ammo == 0)) {
      return -9;
    }

    return 0;
  }

  public void Build(Slot slot) {
    slot.Add(16153u.GetSpell());
  }
}
