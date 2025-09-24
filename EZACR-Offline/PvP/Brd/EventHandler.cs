using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd;

public class EventHandler : IRotationEventHandler {
  public void OnTerritoryChanged() { }

  public void OnSpellCastSuccess(Slot slot, Spell spell) { }

  public void OnResetBattle() {
    BattleData.Reset();
  }

  public async Task OnPreCombat() {
    PvPTargetHelper.自动选中(PvPBrdSettings.Instance.TargetingDistance, 
                         PvPBrdSettings.Instance.TargetingHpThreshold);

    if (PvPSettings.Instance.无目标坐骑) {
      MountHandler.无目标坐骑();
    }

    await Task.CompletedTask;
  }

  public async Task OnNoTarget() {
//    Slot slot = new Slot();
    PvPTargetHelper.自动选中(PvPBrdSettings.Instance.TargetingDistance, 
                         PvPBrdSettings.Instance.TargetingHpThreshold);

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
    PvPHelper.CommonBattleUpdate(currTime, BattleData.Instance);
    PvPTargetHelper.自动选中(PvPBrdSettings.Instance.TargetingDistance, 
                         PvPBrdSettings.Instance.TargetingHpThreshold);
  }

  public void OnEnterRotation() {
    PvPHelper.AcrInit();
    Qt.MacroMan.Init();
    Share.Pull = true;
    
    //force mappy
    
  }

  public void OnExitRotation() {
    Qt.MacroMan.Exit();
    Share.Pull = false;
  }
}
