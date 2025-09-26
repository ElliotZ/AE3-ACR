using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 药 : ISlotResolver {
  private const uint _技能药 = 29711;

  public int Check() {
    if (!Qt.Instance.GetQt("喝热水")) return -9;
    if (!PvPHelper.CanActive()) return -3;
    if (!_技能药.GetSpell().IsReadyWithCanCast() || (Core.Me.CurrentMp < 2500U)) return -2;
    return Core.Me.CurrentHpPercent() <= PvPBrdSettings.Instance.药血量 / 100.0 ? 0 : -1;
  }

  public void Build(Slot slot) {
    slot.Add(PvPHelper.SpellWaitAcq(_技能药, Core.Me));
  }
}
