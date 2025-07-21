using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class PlentifulHarvest : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.PlentifulHarvest.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.PlentifulHarvest.GetSpell());
    }
}
