using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 和弦箭 : ISlotResolver {
  public int Check() {
    if (!Qt.Instance.GetQt("和弦箭")) return -9;

    if (!PvPHelper.CanActive()
     || (41464U.GetSpell().Charges < (double)PvPBrdSettings.Instance.和弦箭)
     || (41464U.GetSpell().Charges < 1.0)) {
      return -1;
    }

    return PvPHelper.CommonDistanceCheck(25)
        || (PvPHelper.CommonSkillCheck(41464U, 25) == null)
               ? -5
               : 0;
  }

  public void Build(Slot slot) {
    PvPHelper.CommonSkillCast(slot, 41464U, 25);
  }
}
