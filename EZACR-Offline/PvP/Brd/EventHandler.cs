using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;

namespace EZACR_Offline.PvP.Brd;

public class EventHandler : IRotationEventHandler {
  public void OnTerritoryChanged() { }

  public void OnSpellCastSuccess(Slot slot, Spell spell) { }

  public void OnResetBattle() {
    BattleData.Reset();
  }

  public async Task OnPreCombat() {
    PvPTargetHelper.自动选中();

    if (PvPSettings.Instance.无目标坐骑) {
      MountHandler.无目标坐骑();
    }

    await Task.CompletedTask;
  }

  public async Task OnNoTarget() {
//    Slot slot = new Slot();
    PvPTargetHelper.自动选中();

    if (PvPSettings.Instance.无目标坐骑) {
      MountHandler.无目标坐骑();
    }

    await Task.CompletedTask;
//    slot = (Slot) null;
  }

  public void AfterSpell(Slot slot, Spell spell) {
//    uint id = spell.Id;
  }

  public void OnBattleUpdate(int currTime) {
    PVPHelper.战斗状态();
    PvPTargetHelper.自动选中();
  }

  public void OnEnterRotation() {
    PVPHelper.进入ACR();
    Share.Pull = true;
  }

  public void OnExitRotation() {
    Share.Pull = false;
  }
}
