using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 冲刺 : ISlotResolver {
  private DateTime _lastAuraTime;

  public int Check() {
    if (!Qt.Instance.GetQt(nameof(冲刺))) return -9;

    if (Core.Me.HasAura(1342U)) {
      if ((DateTime.Now - _lastAuraTime).TotalSeconds < PvPSettings.Instance.冲刺) {
        return -8;
      }

      _lastAuraTime = DateTime.Now;
    }

    if (!PvPHelper.CanActive()) return -1;
    if (GCDHelper.GetGCDCooldown() != 0) return -4;
    return MountHandler.IsMounted() ? -5 : 0;
  }

  public void Build(Slot slot) {
    slot.Add(new Spell(29057U, Core.Me) { DontUseGcdOpt = true });
  }
}
