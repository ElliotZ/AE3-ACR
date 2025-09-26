using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 绝峰箭 : ISlotResolver {
  public int Check() {
    if (!PvPHelper.CanActive()) return -1;
    if (!Qt.Instance.GetQt(nameof(绝峰箭))) return -233;
    if (!29393U.GetSpell().IsReadyWithCanCast()) return -2;
    if (GCDHelper.GetGCDCooldown() > 600) return -3;
    if (PvPHelper.CommonDistanceCheck(25)) return -5;
    return PvPHelper.CommonSkillCheck(29393U, 25) == null ? -6 : 0;
  }

  public void Build(Slot slot) {
    PvPHelper.CommonSkillCast(slot, 29393U, 25);
  }
}
