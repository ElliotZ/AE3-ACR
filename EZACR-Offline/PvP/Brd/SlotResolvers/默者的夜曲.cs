using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 默者的夜曲 : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.Always;

  public int Check() {
    IBattleChara? target = PvPTargetHelper.TargetSelector
                                          .GetSkillTargetSmart(15 + PvPSettings.Instance.长臂猿,
                                                    29395U);
    if (!Qt.Instance.GetQt("沉默")) return -9;
    if (!PvPHelper.CanActive()) return -1;
    if (!29395U.GetSpell().IsReadyWithCanCast()) return -2;
    if (PvPHelper.CommonDistanceCheck(15)) return -5;
    if (PvPHelper.CommonSkillCheck(29395U, 15) == null) return -6;
    return (PvPSettings.Instance.技能自动选中
         && ((PvPSettings.Instance.最合适目标
           && (target != null)
           && (target != Core.Me)
           && PvPTargetHelper.Check目标免控(target))
          || ((PvPTargetHelper.TargetSelector.GetNearestTarget() != null)
           && (PvPTargetHelper.TargetSelector.GetNearestTarget() != Core.Me)
           && PvPTargetHelper.Check目标免控(PvPTargetHelper.TargetSelector.GetNearestTarget()))))
        || (!PvPSettings.Instance.技能自动选中
         && PvPTargetHelper.Check目标免控(Core.Me.GetCurrTarget()))
               ? -3
               : 0;
  }

  public void Build(Slot slot) {
    PvPHelper.CommonSkillCast(slot, 29395U, 15);
  }
}
