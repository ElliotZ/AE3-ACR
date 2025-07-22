using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class EnshroudSk : ISlotResolver
{
    public int Check()
    {
        if (Core.Me.HasAura(AurasDef.Enshrouded) == false)
        {
            return -3;  // -3 for Unmet Prereq Conditions
        }
        if (Qt.Instance.GetQt("单魂衣") && Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 10000, false))
        {
            return -6;
        }
        if (//SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5500 &&
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000, false))
        {
            return -6;  // -6 for delaying for burst prep
        }
        return 0;
    }

    private uint Solve()
    {
        var blueOrb = Core.Resolve<JobApi_Reaper>().LemureShroud;
        //var purpOrb = Core.Resolve<JobApi_Reaper>().VoidShroud;
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        if (SpellsDef.Communio.GetSpell().IsReadyWithCanCast() && blueOrb == 1)
        {
            return SpellsDef.Communio;
        }
        if (Qt.Instance.GetQt("AOE"))
        {
            if (enemyCount >= 4 || 
                (enemyCount >= 3 && !(Core.Me.HasAura(AurasDef.EnhancedCrossReaping) ||
                                      Core.Me.HasAura(AurasDef.EnhancedVoidReaping))
                )
               )
            {
                return SpellsDef.GrimReaping;
            }
        }
        if (Core.Me.HasAura(AurasDef.EnhancedCrossReaping))
        {
            return SpellsDef.CrossReaping;
        }
        //if (SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5000 &&
        //        Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000))  // TODO: Add Burst QT
        //{
        //    return SpellsDef.ShadowOfDeath;
        //}
        return SpellsDef.VoidReaping;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
