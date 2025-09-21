using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 净化 : ISlotResolver {
  public uint 技能净化 = 29056;

  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    if (!Qt.Instance.GetQt("自动净化")) {
      return -9;
    }

    if (!技能净化.GetSpell().IsReadyWithCanCast()) {
      return -2;
    }

    return PVPHelper.净化判断() ? 1 : -3;
  }

  public void Build(Slot slot) {
    slot.Add(PVPHelper.等服务器Spell(29056U, Core.Me));
  }
}
