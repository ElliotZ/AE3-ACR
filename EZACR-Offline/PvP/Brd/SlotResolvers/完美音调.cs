using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 完美音调 : ISlotResolver {
  public int Check() {
    if (!Qt.Instance.GetQt("强劲射击")) return -9;
    if (!PvPHelper.CanActive()) return -1;
    if (!29392U.GetSpell().IsReadyWithCanCast() || !Core.Me.HasAura(3137U)) return -2;
    if (GCDHelper.GetGCDCooldown() > 600) return -3;
    if (PvPHelper.CommonDistanceCheck(25)) return -5;
    return PvPHelper.CommonSkillCheck(29392U, 25) == null ? -6 : 0;
  }

  public void Build(Slot slot) {
    PvPHelper.CommonSkillCast(slot, 29392U, 25);
  }
}
