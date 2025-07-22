using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class BuffMaintain : ISlotResolver
{
    private static int GluttonyCD => (int)Math.Floor(SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds);
    public int Check()
    {
        if (SpellsDef.ShadowOfDeath.GetSpell().IsReadyWithCanCast() == false) { return -99; }  // -99 for not usable

        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, GCDHelper.GetGCDDuration(), false)) 
        {
            return 1;  // 1 for buff maintain within a GCD
        }
        if (Qt.Instance.GetQt("暴食") && GluttonyCD < 10000 && 
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, GluttonyCD + 5000) &&
                Helper.TgtAuraTimerMoreThan(AurasDef.DeathsDesign, GluttonyCD))
        {
            return 2;  // 2 for pre gluttony, earlier use because Gib/Gallows must be covered
        }
        if (Qt.Instance.GetQt("单魂衣") && 
                Core.Me.HasAura(AurasDef.Enshrouded) && 
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 10000))
        {
            return 3;
        }
        if (Core.Me.HasAura(AurasDef.Enshrouded) && 
                //SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5000 &&
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000)) 
        { 
            return 3;  // 3 for burst prep
        }

        if (SpellsDef.WhorlOfDeath.RecentlyUsed(10000)) { return -5; }  // -5 for Avoiding Spam
        //if (Core.Resolve<JobApi_Reaper>().ShroudGauge >= 50 &&)
        return -1;  // -1 for general unmatch
    }

    // VERY BIG TODO
    //private static int AOEAuraScore()
    //{
    //    var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
    //}

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        if (Qt.Instance.GetQt("AOE") && SpellsDef.WhorlOfDeath.GetSpell().IsReadyWithCanCast() && enemyCount >= 3)
        {
            return SpellsDef.WhorlOfDeath;
        }
        return SpellsDef.ShadowOfDeath;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
