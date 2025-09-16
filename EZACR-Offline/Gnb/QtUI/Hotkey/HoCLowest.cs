using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ;

namespace EZACR_Offline.Gnb.QtUI.Hotkey;

public class HoCLowest() : HotKeyResolver(SpellsDef.HeartOfCorundum, useHighPrioritySlot: false) {
  private static IBattleChara? LowestHpPartyMemberWithoutBuffs(uint buffId) {
    if (PartyHelper.CastableAlliesWithin30.Count == 0) return Core.Me;
    return PartyHelper.CastableAlliesWithin30.Where(r => (r.CurrentHp != 0) && !r.HasAura(buffId))
                      .MinBy(r => r.CurrentHpPercent());
  }

  public override void Run() {
    uint targetSpellId = _spellId.AdaptiveId();
    Spell spell = targetSpellId.GetSpell(LowestHpPartyMemberWithoutBuffs(AurasDef.Holmgang)!);
    double cooldown = spell.Cooldown.TotalMilliseconds;

    if (_waitCoolDown && (cooldown > 0)) {
      _ = Run1(spell, (int)cooldown);
    } else {
      _ = Run1(spell);
    }
  }
}
