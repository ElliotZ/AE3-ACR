using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class BowShock : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.OffGcd;

  public int Check() {
    int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);
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

    if (!16159u.GetSpell().IsReadyWithCanCast()) return -3;

    if ((Core.Me.Level < 62) || !16159u.IsUnlock()) return -5;

    if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 5f) return -3;

    if (Qt.Instance.GetQt("自动拉怪")) return -4;

    if (!Qt.Instance.GetQt("弓形冲波")) return -1;

    if (Core.Me.HasAnyAura(GnbHelper.ContBuffs)) return -99;

    if (Qt.Instance.GetQt("倾泻爆发")) return 2;

    if (!Qt.Instance.GetQt("爆发")) return -20;

    if (25760u.GetSpell().IsReadyWithCanCast()) {
      if (Core.Me.HasAura(1831u)) return 0;

      if (Qt.Instance.GetQt("无视无情")) return 7;

      return -8;
    }

    if (16138u.CoolDownInGCDs(2) && !Qt.Instance.GetQt("无视无情")) return -50;

    return 0;
  }

  public void Build(Slot slot) {
    slot.Add(16159u.GetSpell());
  }
}
