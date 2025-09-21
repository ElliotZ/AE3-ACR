using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 英雄的返场余音 : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    if (!PVPHelper.CanActive()) return -1;
    if (!Core.Me.HasAura(4312U)) return -2;
    return PVPHelper.通用距离检查(25)
        || (PVPHelper.通用技能释放Check(41467U, 25) == null)
               ? -5
               : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 41467U, 25);
  }
}
