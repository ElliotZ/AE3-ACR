using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Gluttony : ISlotResolver
{
    private static uint currGluttony => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gluttony);

    public int Check()
    {
        if (currGluttony.GetSpell().IsReadyWithCanCast() == false)
        {
            return -99;
        }
        if (Core.Me.HasAura(AurasDef.Executioner) ||
                Core.Me.HasAura(AurasDef.SoulReaver))
        {
            return -4;  // -4 for Overcapped Resources
        }
        // add QT
        // might need to check for death's design
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(currGluttony.GetSpell());
    }
}
