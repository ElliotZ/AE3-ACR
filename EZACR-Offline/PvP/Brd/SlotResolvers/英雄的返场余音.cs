using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 英雄的返场余音 : ISlotResolver {
  public int Check() {
    if (!PvPHelper.CanActive()) return -1;
    if (!Core.Me.HasAura(4312U)) return -2;
    return PvPHelper.CommonDistanceCheck(25)
        || (PvPHelper.CommonSkillCheck(41467U, 25) == null)
               ? -5
               : 0;
  }

  public void Build(Slot slot) {
    PvPHelper.CommonSkillCast(slot, 41467U, 25);
  }
}
