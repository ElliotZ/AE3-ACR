using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class BloodStalk : ISlotResolver
{
    private static uint currBloodStalk => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.BloodStalk);
    public int Check()
    {
        if (currBloodStalk.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("挥割/爪") == false) { return -98; }
        if (Core.Me.HasAura(AurasDef.Enshrouded)) { return -1; }  // not this slot resolver
        
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge == 100 ||
            Core.Me.HasAura(AurasDef.SoulReaver) || 
            Core.Me.HasAura(AurasDef.Executioner)) 
        { 
            return -4; 
        }
        // maybe check for death's design, but probably not needed for now
        if (SpellsDef.Gluttony.CoolDownInGCDs(3) && Core.Resolve<JobApi_Reaper>().SoulGauge < 100)
        {
            return -12;  // delay for gluttony gauge cost
        }
        if (Qt.Instance.GetQt("神秘环") && 
                SpellsDef.ArcaneCircle.CoolDownInGCDs(5) && 
                Core.Resolve<JobApi_Reaper>().ShroudGauge != 40)
        {
            return -12;  // delay for gluttony after burst window
        }
        if (RprHelper.ComboTimer <= GCDHelper.GetGCDDuration() + 600 &&
                (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        if (Core.Me.HasAura(AurasDef.ImmortalSacrifice) || 
                SpellsDef.PlentifulHarvest.RecentlyUsed()) 
        { 
            return -12;  // delay for burst window
        }
        if (!Core.Me.HasAura(AurasDef.TrueNorth) &&
                Core.Me.GetCurrTarget().HasPositional() &&
                ((Core.Me.HasAura(AurasDef.EnhancedGallows) && !Helper.AtRear) || 
                  (Core.Me.HasAura(AurasDef.EnhancedGibbet) && !Helper.AtFlank)  ) &&
                Core.Resolve<JobApi_Reaper>().SoulGauge < 100)
        {
            return -13;  // TN Optimizations perhaps
        }
        return 0;
    }

    private uint Solve()
    {
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);

        if (Qt.Instance.GetQt("AOE") && enemyCount >= 4 && SpellsDef.GrimSwathe.GetSpell().IsReadyWithCanCast()) 
        { 
            return SpellsDef.GrimSwathe; 
        }
        return currBloodStalk;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
