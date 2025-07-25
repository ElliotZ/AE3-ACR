using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class EnshroudSk : ISlotResolver
{
    private int blueOrb => Core.Resolve<JobApi_Reaper>().LemureShroud;
    private IBattleChara? Target { get; set; }

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
        if (!Core.Me.HasAura(AurasDef.PerfectioOculta) &&
                Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000, false))
        {
            return -6;  // -6 for delaying for burst prep
        }
        return 0;
    }

    private Spell Solve()
    {
        //var purpOrb = Core.Resolve<JobApi_Reaper>().VoidShroud;
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        var enhancedReapingCheck = (Core.Me.HasAura(AurasDef.EnhancedCrossReaping) ||
                                      Core.Me.HasAura(AurasDef.EnhancedVoidReaping)) ? 3 : 4;
        Target = SpellsDef.GrimReaping.OptimalAOETarget(enhancedReapingCheck, 180);

        if (SpellsDef.Communio.GetSpell().IsReadyWithCanCast() && blueOrb < 2)
        {
            return SpellsDef.Communio.GetSpell();
        }
        if (Qt.Instance.GetQt("AOE"))
        {
            if (Target is not null)
            {
                return SpellsDef.GrimReaping.GetSpell(Target!);
            }
        }
        if (Core.Me.HasAura(AurasDef.EnhancedCrossReaping))
        {
            return SpellsDef.CrossReaping.GetSpell();
        }
        //if (SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5000 &&
        //        Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000))  // TODO: Add Burst QT
        //{
        //    return SpellsDef.ShadowOfDeath;
        //}
        return SpellsDef.VoidReaping.GetSpell();
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve());
    }
}
