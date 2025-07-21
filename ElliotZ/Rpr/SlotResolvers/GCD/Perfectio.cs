using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class Perfectio : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Perfectio.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Perfectio.GetSpell());
    }
}
