using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 锐眼 : ISlotResolver {
  private const uint _技能锐眼 = 43251u;
  private IBattleChara? _target;
  
  public int Check() {
    _target = PvPTargetHelper.TargetSelector.GetSkillTargetSmart(40, _技能锐眼);
    if (_技能锐眼.GetSpell().IsReadyWithCanCast() is false) return -99;
    if (Qt.Instance.GetQt("职能技能") is false) return -98;  //QT
    if (_target is null) return -3;
    if (_target.CurrentHp > 12000) return -4;
    return 0;
  }

  public void Build(Slot slot) {
    slot.Add(new Spell(_技能锐眼, _target!) {DontUseGcdOpt = true});
  }
}
