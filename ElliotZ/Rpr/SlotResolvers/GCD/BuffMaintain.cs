using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class BuffMaintain : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.ShadowOfDeath.GetSpell().IsReadyWithCanCast() == false) { return -99; }  // -99 for not usable
        if (SpellsDef.WhorlOfDeath.RecentlyUsed(10000)) { return -5; }  // -5 for Avoiding Spam
        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 100, false)) { return 1; }
        return -1;  // -1 for general unmatch
    }

    //private static int AOEAuraScore()
    //{
    //    var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
    //}

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        if (SpellsDef.WhorlOfDeath.GetSpell().IsReadyWithCanCast() && enemyCount >= 3)
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
