using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 龟壳 : ISlotResolver{
  private const uint _技能龟壳 = 29054;

  public int Check() {
    var battleCharaList = PvPTargetHelper.Get看着目标的人(Group.敌人, Core.Me);

    if (_技能龟壳.GetSpell().IsReadyWithCanCast() is false) {
      return -99;
    }

    if (Qt.Instance.GetQt("龟壳") is false) return -98;

    if (battleCharaList.Count >= PvPSettings.Instance.警报数量 && Danger()) {
      return 0;
    }

    return -1;
  }

  private static bool Danger() {
    float dangerHpThreshold =
        PvPHelper.RestrictedTerritoryIds.Contains(ElliotZ.Helper.GetTerritoryId) 
            ? 0.2f 
            : 0.3f;
    return Core.Me.CurrentHpPercent() <= dangerHpThreshold 
        || BattleData.Instance.HPDelta <= -12000 
        || BattleData.Instance.TotalHPDelta <= 35000;
  }

  public void Build(Slot slot) {
    slot.Add(new Spell(_技能龟壳, Core.Me) {DontUseGcdOpt = true});
  }
}
