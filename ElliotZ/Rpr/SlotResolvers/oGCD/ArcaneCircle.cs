using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class ArcaneCircle : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.ArcaneCircle.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.ArcaneCircle.GetSpell());
    }
}
