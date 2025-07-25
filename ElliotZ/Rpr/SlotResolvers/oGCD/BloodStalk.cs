using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using static AEAssist.CombatRoutine.View.MeleePosHelper;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class BloodStalk : ISlotResolver
{
    private IBattleChara? Target {  get; set; }
    public int Check()
    {
        if (Helper.GetActionChange(SpellsDef.BloodStalk).GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("挥割/爪") == false) { return -98; }
        if (Core.Me.HasAura(AurasDef.Enshrouded)) { return -1; }  // not this slot resolver
        
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge == 100 ||
            Core.Me.HasAura(AurasDef.SoulReaver) || 
            Core.Me.HasAura(AurasDef.Executioner)) 
        { 
            return -4; 
        }
        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, GCDHelper.GetGCDDuration(), false)) 
        { 
            return -14; 
        }
        if (!Core.Me.HasAura(AurasDef.TrueNorth) &&
                Core.Me.GetCurrTarget().HasPositional() &&
                !SpellsDef.TrueNorth.IsMaxChargeReady(1.8f) &&
                ((Core.Me.HasAura(AurasDef.EnhancedGallows) && !Helper.AtRear) ||
                 (Core.Me.HasAura(AurasDef.EnhancedGibbet) && !Helper.AtFlank)) &&
                Core.Resolve<JobApi_Reaper>().SoulGauge < 100)
        {
            return -13;  // TN Optimizations perhaps
        }
        if (SpellsDef.Gluttony.IsUnlock() && 
                SpellsDef.Gluttony.CoolDownInGCDs(3) && 
                Core.Resolve<JobApi_Reaper>().SoulGauge < 100)
        {
            return -12;  // delay for gluttony gauge cost
        }
        if (Qt.Instance.GetQt("神秘环") && 
                SpellsDef.ArcaneCircle.IsUnlock() &&
                SpellsDef.ArcaneCircle.CoolDownInGCDs(5) && 
                Core.Resolve<JobApi_Reaper>().ShroudGauge != 40)
        {
            return -12;  // delay for gluttony after burst window
        }
        if (Helper.ComboTimer <= GCDHelper.GetGCDDuration() + 2000 &&
                (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        if (Core.Me.HasAura(AurasDef.ImmortalSacrifice) || 
                SpellsDef.PlentifulHarvest.RecentlyUsed()) 
        { 
            return -12;  // delay for burst window
        }
        if (Helper.AuraTimerLessThan(AurasDef.ArcaneCircle, 5000) && Core.Me.HasAura(AurasDef.PerfectioParata))
        {
            return -12;
        }

        return 0;
    }

    private Spell Solve()
    {
        //var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        Target = SpellsDef.GrimSwathe.OptimalAOETarget(4, 180);

        if (Qt.Instance.GetQt("AOE") && Target is not null &&
                SpellsDef.GrimSwathe.GetSpell(Target!).IsReadyWithCanCast()) 
        { 
            return SpellsDef.GrimSwathe.GetSpell(Target!); 
        }
        return Helper.GetActionChange(SpellsDef.BloodStalk).GetSpell();
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve());
    }
}
