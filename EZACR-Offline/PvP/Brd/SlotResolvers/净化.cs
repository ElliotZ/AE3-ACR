using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 净化 : ISlotResolver {
  private const uint _技能净化 = 29056;

  public int Check() {
    if (!Qt.Instance.GetQt("自动净化")) {
      return -9;
    }

    if (!_技能净化.GetSpell().IsReadyWithCanCast()) {
      return -2;
    }

    return PvPHelper.CanDispelMe() ? 1 : -3;
  }

  public void Build(Slot slot) {
    slot.Add(PvPHelper.SpellWaitAcq(_技能净化, Core.Me));
  }
}
