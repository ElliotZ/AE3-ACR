using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class EnshroudHighPrio : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Enshroud.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        //if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 50 && !Core.Me.HasAura(AurasDef.IdealHost)) return -1;
        if (Helper.GetAuraTimeLeft(AurasDef.IdealHost) > 1500) { return -8; }  // -8 for exiting high prio state
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Enshroud.GetSpell());
    }
}