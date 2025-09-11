using AEAssist.CombatRoutine;
using AEAssist.Helper;
using ElliotZ.Common;

namespace EZACR_Offline.Gnb.QtUI.Hotkey;

public class OneKeyMits()
    : HotKeyResolver(SpellsDef.Rampart, SpellTargetType.Self, waitCoolDown: false) {
  public override int Check() {
    if (SpellsDef.Rampart.GetSpell().IsReadyWithCanCast()) {
      _spellId = SpellsDef.HeartOfCorundum;
      return 0;
    }

    if (SpellsDef.HeartOfCorundum.GetSpell().IsReadyWithCanCast()) {
      _spellId = SpellsDef.GreatNebula;
      return 0;
    }

    if (SpellsDef.GreatNebula.GetSpell().IsReadyWithCanCast()) {
      _spellId = SpellsDef.Camouflage;
      return 0;
    }

    if (SpellsDef.Camouflage.GetSpell().IsReadyWithCanCast()) {
      _spellId = SpellsDef.Superbolide;
      return 0;
    }

    if (SpellsDef.Superbolide.GetSpell().IsReadyWithCanCast()) {
      _spellId = SpellsDef.Rampart;
      return 0;
    }

    return -1;
  }
}
