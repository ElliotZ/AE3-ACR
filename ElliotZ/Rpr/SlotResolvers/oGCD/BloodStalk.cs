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
    private int Soul => Core.Resolve<JobApi_Reaper>().SoulGauge;

    public int Check()
    {
        Target = SpellsDef.GrimSwathe.OptimalAOETarget(4, 180, Qt.Instance.GetQt("智能AOE"));

        if (Target is null && 
                Helper.GetActionChange(SpellsDef.BloodStalk).GetSpell().IsReadyWithCanCast() == false) 
        {
            return -99; 
        }
        if (Target is not null && SpellsDef.GrimSwathe.GetSpell(Target!).IsReadyWithCanCast() == false)
        {
            return -99;
        }
        if (Qt.Instance.GetQt("挥割/爪") == false) { return -98; }
        if (Core.Me.HasAura(AurasDef.Enshrouded)) { return -1; }  // not this slot resolver
        
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge == 100 ||
            Core.Me.HasAura(AurasDef.SoulReaver) || 
            Core.Me.HasAura(AurasDef.Executioner)) 
        { 
            return -4; 
        }
        if (Helper.ComboTimer <= GCDHelper.GetGCDDuration() + RprSettings.Instance.AnimLock * 3 &&
        (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign,
                                GCDHelper.GetGCDDuration() + RprSettings.Instance.AnimLock * 3,
                                false))
        {
            return -14;
        }
        if (Core.Me.HasAura(AurasDef.ImmortalSacrifice) ||
                SpellsDef.PlentifulHarvest.RecentlyUsed())
        {
            return -15;  // delay for burst window
        }
        if (Helper.AuraTimerLessThan(AurasDef.ArcaneCircle, 5000) && Core.Me.HasAura(AurasDef.PerfectioParata))
        {
            return -16;
        }

        if (Qt.Instance.GetQt("暴食"))
        {
            if (SpellsDef.Gluttony.IsUnlock() && 
                    SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds < GCDHelper.GetGCDDuration())
            {
                return -21;
            }
            if (Soul == 100 && GCDHelper.GetGCDCooldown() >= RprSettings.Instance.AnimLock) return 1;
            if (SpellsDef.Gluttony.IsUnlock() &&
                    SpellsDef.Gluttony.RdyInGCDs(GcdsToOvercap()) &&
                    !(SpellsDef.Gluttony.RdyInGCDs(2) && SpellsDef.SoulSlice.GetSpell().Charges > 1.7f)) // &&
                    //Soul < 100)
            {
                return -22;  // delay for gluttony gauge cost
            }
        }
        else
        {
            if (Qt.Instance.GetQt("神秘环") &&
                    SpellsDef.ArcaneCircle.IsUnlock() &&
                    !SpellsDef.ArcaneCircle.RdyInGCDs(GcdsToOvercap() + 3))  // &&
                    //Soul < 100)
            {
                return -31;
            }
        }
        if (!Qt.Instance.GetQt("倾泻资源"))  // ignore all if dump qt is set
        {
            if (Qt.Instance.GetQt("神秘环") &&
                    //Soul < 100 &&
                    SpellsDef.ArcaneCircle.IsUnlock() &&
                    SpellsDef.ArcaneCircle.RdyInGCDs(2) &&
                    Core.Resolve<JobApi_Reaper>().ShroudGauge != 40)
            {
                return -17;  // delay for gluttony after burst window
            }
            if (Soul == 100 && GCDHelper.GetGCDCooldown() >= RprSettings.Instance.AnimLock) return 1;
            if (Qt.Instance.GetQt("神秘环") &&
                    //Soul < 100 &&
                    SpellsDef.ArcaneCircle.IsUnlock() &&
                    SpellsDef.ArcaneCircle.RdyInGCDs(Math.Min(6, GcdsToOvercap() + 3)) &&
                    Core.Resolve<JobApi_Reaper>().ShroudGauge != 40)
            {
                return -12;  // delay for gluttony after burst window
            }
            if (!Helper.AuraTimerMoreThan(AurasDef.TrueNorth,
                                  BattleData.Instance.GcdDuration - GCDHelper.GetGCDCooldown()) &&
                    Qt.Instance.GetQt("真北") && Qt.Instance.GetQt("真北优化") &&
                    Core.Me.GetCurrTarget().HasPositional() &&
                    !SpellsDef.TrueNorth.IsMaxChargeReady(1.8f) &&
                    ((Core.Me.HasAura(AurasDef.EnhancedGallows) && !Helper.AtRear) ||
                        (Core.Me.HasAura(AurasDef.EnhancedGibbet) && !Helper.AtFlank)))  // &&
                                                                                         //Soul < 100)
            {
                return -13;  // TN Optimizations perhaps
            }
        }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) return -89;
        return 0;
    }

    private Spell Solve()
    {
        //var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);

        if (Qt.Instance.GetQt("AOE") && Target is not null &&
                SpellsDef.GrimSwathe.GetSpell(Target!).IsReadyWithCanCast()) 
        { 
            return SpellsDef.GrimSwathe.GetSpell(Target!); 
        }
        return Helper.GetActionChange(SpellsDef.BloodStalk).GetSpell();
    }

    private static int GcdsToOvercap()
    {
        int res = (100 - Core.Resolve<JobApi_Reaper>().SoulGauge) / 10;
        if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 
                                            BattleData.Instance.GcdDuration * (res + 3), 
                                            false) ||
            Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 
                                            30000 + BattleData.Instance.GcdDuration * res, 
                                            false))
        {
            res++;
        }
        return res;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve());
    }
}
