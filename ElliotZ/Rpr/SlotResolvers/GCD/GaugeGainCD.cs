using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GaugeGainCD : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.SoulSlice.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Resolve<JobApi_Reaper>().SoulGauge > 50) { return -4; }  // -4 for Overcapped Resources
        return 0;
    }

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        if (SpellsDef.SoulScythe.GetSpell().IsReadyWithCanCast() && enemyCount >= 3)
        {
            return SpellsDef.SoulScythe;
        }
        return SpellsDef.SoulSlice;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
