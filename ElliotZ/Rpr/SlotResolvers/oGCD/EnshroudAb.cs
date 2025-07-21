using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class EnshroudAb : ISlotResolver
{
    public int Check()
    {
        var purpOrb = Core.Resolve<JobApi_Reaper>().VoidShroud;

        if (Core.Me.HasAura(AurasDef.Enshrouded) == false) { return -3; }  // -3 for Unmet Prereq Conditions
        //if (SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5000 &&
        //        Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000))
        //{
        //    return -6;  // -6 for delaying for burst prep
        //}
        if (purpOrb < 2) { return -3; }
        //if (GCDHelper.GetGCDCooldown() < 800) return -7;  // -7 for avoiding clipping
        return 0;
    }

    private uint Solve()
    {
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);

        if (enemyCount >= 3) { return SpellsDef.LemuresScythe; }
        return SpellsDef.LemuresSlice;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
