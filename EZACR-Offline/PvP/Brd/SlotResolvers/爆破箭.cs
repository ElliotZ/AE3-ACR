using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 爆破箭 : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    if (!Qt.Instance.GetQt(nameof(爆破箭))) return -233;
    if (!PVPHelper.CanActive()) return -1;
    if (!Core.Me.HasAura(3142U)) return -9;
    if (!29394U.GetSpell().IsReadyWithCanCast()) return -2;
    if (GCDHelper.GetGCDCooldown() > 600) return -3;
    if (PVPHelper.通用距离检查(25)) return -5;
    return PVPHelper.通用技能释放Check(29394U, 25) == null ? -6 : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 29394U, 25);
  }
}
