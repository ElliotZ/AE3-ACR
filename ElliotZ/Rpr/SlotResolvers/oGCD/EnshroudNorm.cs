using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class EnshroudNorm : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Enshroud.isUnlock() == false) return -99;
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 50 && !Core.Me.HasAura(AurasDef.IdealHost)) return -1;
        return 0;
    }

}