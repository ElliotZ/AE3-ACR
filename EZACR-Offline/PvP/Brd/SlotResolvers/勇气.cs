using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 勇气 : ISlotResolver {
  public uint 勇气u = 43250;
  public uint 勇气释放buff = 4490;
  public uint 勇气buff = 4479;

  public int Check() {
    if (!Qt.Instance.GetQt("职能技能")) return -9;
    if (!Core.Me.HasAura(勇气释放buff)) return -2;
    if (!PvPHelper.CanActive() || !勇气u.GetSpell().IsReadyWithCanCast()) return -1;
    return MountHandler.IsMounted() ? -5 : 0;
  }

  public void Build(Slot slot) {
    slot.Add(new Spell(勇气u, Core.Me) { DontUseGcdOpt = true });
  }
}
