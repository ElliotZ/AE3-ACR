using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class BuffMaintain : ISlotResolver
{
    private static int GluttonyCD => (int)Math.Floor(SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds);
    public int Check()
    {
        if (SpellsDef.ShadowOfDeath.GetSpell().IsReadyWithCanCast() == false) { return -99; }  // -99 for not usable

        if (SpellsDef.WhorlOfDeath.RecentlyUsed(5000)) { return -5; }  // -5 for Avoiding Spam

        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, GCDHelper.GetGCDDuration(), false)) 
        {
            return 1;  // 1 for buff maintain within a GCD
        }
        if (Qt.Instance.GetQt("暴食") && GluttonyCD < 10000 && 
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, GluttonyCD + 7500) &&
                Helper.TgtAuraTimerMoreThan(AurasDef.DeathsDesign, GluttonyCD + 2500))
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
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000, false)) 
        { 
            return 3;  // 3 for burst prep
        }
        if (Core.Resolve<JobApi_Reaper>().SoulGauge == 100 &&
                !SpellsDef.Perfectio.GetSpell().IsReadyWithCanCast() &&
                !SpellsDef.PlentifulHarvest.GetSpell().IsReadyWithCanCast() &&
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000, false))
        {
            return 5;
        }
        //if (Qt.Instance.GetQt("神秘环") && 
        //        SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds < 11000 && 
        //        Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 11000 + GCDHelper.GetGCDDuration(), false))
        //{
        //    return 3;
        //}
        if (SpellsDef.WhorlOfDeath.IsUnlock() && AOEAuraCheck()) { return 4; };
        //if (Core.Resolve<JobApi_Reaper>().ShroudGauge >= 50 &&)
        return -1;  // -1 for general unmatch
    }

    /// <summary>
    /// Checks for Deaths Design on all enemies 
    /// </summary>
    /// <returns>true if less than half enemies around have the debuff, false otherwise</returns>
    private static bool AOEAuraCheck()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        var enemylist = TargetMgr.Instance.EnemysIn12;
        var noDebuffEnemyCount = enemylist.Count(v =>
                Core.Me.Distance(v.Value, DistanceMode.IgnoreTargetHitbox | DistanceMode.IgnoreHeight) < 5 &&
                Core.Resolve<MemApiBuff>().GetAuraTimeleft(v.Value, AurasDef.DeathsDesign, true) <= BattleData.Instance.GcdDuration);
        if (RprSettings.Instance.Debug) {
            LogHelper.PrintError("BuffMaintain.AOEAuraCheck() Internals");
            LogHelper.PrintError(noDebuffEnemyCount.ToString() + "/" + enemyCount.ToString() + "=" + (noDebuffEnemyCount / (double)enemyCount).ToString());
        }
        return (noDebuffEnemyCount / (double)enemyCount) > 0.5;
    }

    public static uint Solve()
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
