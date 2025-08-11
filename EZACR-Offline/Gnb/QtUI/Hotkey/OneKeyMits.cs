using AEAssist.CombatRoutine;
using AEAssist.Helper;
using ElliotZ.Common;

namespace EZACR_Offline.Gnb.QtUI.Hotkey;

public class OneKeyMits() : HotKeyResolver(SpellsDef.Rampart, SpellTargetType.Self, waitCoolDown: false)
{
    public override int Check()
    {
        if (SpellsDef.Rampart.GetSpell().IsReadyWithCanCast())
        {
            SpellId = SpellsDef.HeartOfCorundum;
            return 0;
        }

        if (SpellsDef.HeartOfCorundum.GetSpell().IsReadyWithCanCast())
        {
            SpellId = SpellsDef.GreatNebula;
            return 0;
        }

        if (SpellsDef.GreatNebula.GetSpell().IsReadyWithCanCast())
        {
            SpellId = SpellsDef.Camouflage;
            return 0;
        }

        if (SpellsDef.Camouflage.GetSpell().IsReadyWithCanCast())
        {
            SpellId = SpellsDef.Superbolide;
            return 0;
        }

        if (SpellsDef.Superbolide.GetSpell().IsReadyWithCanCast())
        {
            SpellId = SpellsDef.Rampart;
            return 0;
        }

        return -1;
    }
}
