using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class TrueNorth : ISlotResolver
{
    private static uint currGibbet => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gibbet);
    private static uint currGallows => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gallows);
    public int Check()
    {
        if (SpellsDef.TrueNorth.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("真北") == false) { return -98; }

        if (Core.Me.HasAura(AurasDef.TrueNorth)) { return -5; }  // -5 for avoiding spam

        if (Core.Me.GetCurrTarget().HasPositional() &&
                GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock &&
                currGibbet.GetSpell().IsReadyWithCanCast())
        {
            if (Core.Me.HasAura(AurasDef.EnhancedGallows) && !Helper.AtRear) { return 0; }
            if (Core.Me.HasAura(AurasDef.EnhancedGibbet) && !Helper.AtFlank) { return 0; }
        }
        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.TrueNorth.GetSpell());
    }
}
