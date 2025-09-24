using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 速度之星 : ISlotResolver {
  public uint 速度之星u = 43249;
  public uint 速度之星buff = 4489;

  public int Check() {
    if (!Qt.Instance.GetQt("职能技能")) {
      return -9;
    }

    if (!Core.Me.HasAura(速度之星buff)) {
      return -2;
    }

    if (!PvPHelper.CanActive() || !速度之星u.GetSpell().IsReadyWithCanCast()) {
      return -1;
    }

    return MountHandler.IsMounted() ? -5 : 0;
  }

  public void Build(Slot slot) {
    slot.Add(new Spell(速度之星u, Core.Me) {DontUseGcdOpt = true});
  }
}
