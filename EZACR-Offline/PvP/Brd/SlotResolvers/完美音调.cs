using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 完美音调 : ISlotResolver {
  public SlotMode SlotMode { get; }

  public int Check() {
    if (!Qt.Instance.GetQt("强劲射击")) return -9;
    if (!PVPHelper.CanActive()) return -1;
    if (!29392U.GetSpell().IsReadyWithCanCast() || !Core.Me.HasAura(3137U)) return -2;
    if (GCDHelper.GetGCDCooldown() > 600) return -3;
    if (PVPHelper.通用距离检查(25)) return -5;
    return PVPHelper.通用技能释放Check(29392U, 25) == null ? -6 : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 29392U, 25);
  }
}
