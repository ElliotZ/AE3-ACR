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
                                          .Get最合适目标(15 + PvPSettings.Instance.长臂猿,
                                                    29395U);
    if (!Qt.Instance.GetQt("沉默")) return -9;
    if (!PVPHelper.CanActive()) return -1;
    if (!29395U.GetSpell().IsReadyWithCanCast()) return -2;
    if (PVPHelper.通用距离检查(15)) return -5;
    if (PVPHelper.通用技能释放Check(29395U, 15) == null) return -6;
    return (PvPSettings.Instance.技能自动选中
         && ((PvPSettings.Instance.最合适目标
           && (target != null)
           && (target != Core.Me)
           && PvPTargetHelper.Check目标免控(target))
          || ((PvPTargetHelper.TargetSelector.Get最近目标() != null)
           && (PvPTargetHelper.TargetSelector.Get最近目标() != Core.Me)
           && PvPTargetHelper.Check目标免控(PvPTargetHelper.TargetSelector.Get最近目标()))))
        || (!PvPSettings.Instance.技能自动选中
         && PvPTargetHelper.Check目标免控(Core.Me.GetCurrTarget()))
               ? -3
               : 0;
  }

  public void Build(Slot slot) {
    PVPHelper.通用技能释放(slot, 29395U, 15);
  }
}
