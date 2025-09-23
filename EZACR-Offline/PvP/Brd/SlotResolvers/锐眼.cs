using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 锐眼 : ISlotResolver {
  private const uint _技能锐眼 = 43251u;
  private IBattleChara? _target;
  
  public int Check() {
    _target = PvPTargetHelper.TargetSelector.GetLowestHPTarget();
    if (_技能锐眼.GetSpell().IsReadyWithCanCast() is false) return -99;
    //QT
    if (_target is null) return -3;
    if (_target.CurrentHp > 12000) return -4;
    return 0;
  }

  public void Build(Slot slot) {
    slot.Add(_技能锐眼.GetSpell(_target!));
  }
}
