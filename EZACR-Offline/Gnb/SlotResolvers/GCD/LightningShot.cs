using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class LightningShot : ISlotResolver {
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

    if (Core.Me.Level < 15) return -5;

    if ((Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox)
       > (float)(SettingMgr.GetSetting<GeneralSettings>().AttackRange + 2))
     && (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox)
       < (float)(SettingMgr.GetSetting<GeneralSettings>().AttackRange + 17))
     && Qt.Instance.GetQt("闪雷弹")) {
      return 0;
    }

    return -1;
  }

  public void Build(Slot slot) {
    slot.Add(16143u.GetSpell());
  }
}
