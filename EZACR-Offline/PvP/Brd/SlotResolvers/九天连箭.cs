using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 九天连箭 : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    if (!Qt.Instance.GetQt("和弦箭")) return -9;

    if (!PVPHelper.CanActive()
     || (41464U.GetSpell().Charges < (double)PvPBrdSettings.Instance.和弦箭)
     || (41464U.GetSpell().Charges < 1.0)) {
      return -1;
    }

    return PVPHelper.通用距离检查(25)
        || (PVPHelper.通用技能释放Check(41464U, 25) == null)
               ? -5
               : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 41464U, 25);
  }
}
