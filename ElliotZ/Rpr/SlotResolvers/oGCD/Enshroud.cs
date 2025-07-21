using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Enshroud : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Enshroud.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        //if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 50 && !Core.Me.HasAura(AurasDef.IdealHost)) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Enshroud.GetSpell());
    }
}