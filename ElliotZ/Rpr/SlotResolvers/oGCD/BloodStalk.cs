using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class BloodStalk : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.BloodStalk.IsUnlock() == false) return -99;
        if (Core.Resolve<JobApi_Reaper>().SoulGauge < 50) return -1;
        // add QT
        // may be more conditions
        return 0;
    }

    private uint Solve()
    {
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);

        if (enemyCount >= 4 && SpellsDef.GrimSwathe.IsUnlock()) return SpellsDef.GrimSwathe;
        return SpellsDef.BloodStalk;
    }

    public void Build(Slot slot)
    {
        slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(Solve()).GetSpell());
    }
}
