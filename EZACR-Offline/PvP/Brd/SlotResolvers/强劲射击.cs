using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 强劲射击 : ISlotResolver {
  public SlotMode SlotMode { get; }

  public int Check() {
    if (!Qt.Instance.GetQt(nameof(强劲射击))) return -9;
    if (!PVPHelper.CanActive()) return -1;
    if (!29391U.GetSpell().IsReadyWithCanCast()) return -2;
    if (GCDHelper.GetGCDCooldown() > 600) return -3;
    if (PVPHelper.通用距离检查(25)) return -5;
    return PVPHelper.通用技能释放Check(29391U, 25) == null ? -6 : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 29391U, 25);
  }
}
