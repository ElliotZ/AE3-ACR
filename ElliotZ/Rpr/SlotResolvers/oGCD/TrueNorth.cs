using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class TrueNorth : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.TrueNorth.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Me.HasAura(AurasDef.TrueNorth)) { return -5; }  // -5 for avoiding spam

        if (Core.Me.GetCurrTarget().HasPositional() &&
                GCDHelper.GetGCDCooldown() < 600 &&
                SpellsDef.Gibbet.GetSpell().IsReadyWithCanCast())
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
