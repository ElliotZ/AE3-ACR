using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class AutoCrest : ISlotResolver
{
    public int Check()
    {
        var CrestThreshold = Core.Me.MaxHp * RprSettings.Instance.CrestPercent;

        if (RprSettings.Instance.AutoCrest == false) { return -1; }
        if (SpellsDef.ArcaneCrest.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Me.CurrentHp > CrestThreshold) { return -4; }
        if (Core.Me.GetCurrTarget() is null ||
                !TargetHelper.targetCastingIsBossAOE(Core.Me.GetCurrTarget()!, 2000))
        {
            return -3;
        }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) { return -89; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.ArcaneCrest.GetSpell());
    }
}
