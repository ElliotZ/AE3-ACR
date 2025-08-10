using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.QtUI.Hotkey;

public class HoCLowest() : HotKeyResolver(SpellsDef.HeartOfCorundum, useHighPrioritySlot: false)
{
    private static IBattleChara? LowestHpPartyMemberWithoutBuffs(uint buffId)
    {
        if (PartyHelper.CastableAlliesWithin30.Count == 0)
        {
            return Core.Me;
        }
        return PartyHelper.CastableAlliesWithin30.Where(r => 
                                                          r.CurrentHp != 0 && 
                                                         !r.HasAura(buffId)).MinBy(r => 
                                                                                     r.CurrentHpPercent());
    }

    public override void Run()
    {
        var targetSpellId = Helper.GetActionChange(SpellId);
        var spell = targetSpellId.GetSpell(LowestHpPartyMemberWithoutBuffs(AurasDef.Holmgang));
        var cooldown = spell.Cooldown.TotalMilliseconds;

        if (WaitCoolDown && cooldown > 0)
        {
            _ = Run1(spell, (int)cooldown);
        }
        else
        {
            _ = Run1(spell);
        }
    }
}
