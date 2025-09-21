using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 药 : ISlotResolver {
  public uint 技能药 = 29711;

  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    if (!Qt.Instance.GetQt("喝热水")) return -9;
    if (!PVPHelper.CanActive()) return -3;
    if (!技能药.GetSpell().IsReadyWithCanCast() || (Core.Me.CurrentMp < 2500U)) return -2;
    return Core.Me.CurrentHpPercent() <= PvPBrdSettings.Instance.药血量 / 100.0 ? 0 : -1;
  }

  public void Build(Slot slot) {
    slot.Add(PVPHelper.等服务器Spell(29711U, Core.Me));
  }
}
