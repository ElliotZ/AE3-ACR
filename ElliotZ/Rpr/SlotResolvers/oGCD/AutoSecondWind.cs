using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class AutoSecondWind : ISlotResolver
{
    public int Check()
    {
        var SecondWindThreshold = Core.Me.MaxHp * RprSettings.Instance.BloodBathPercent;

        if (RprSettings.Instance.AutoSecondWind == false) { return -1; }
        if (SpellsDef.SecondWind.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Me.CurrentHp > SecondWindThreshold) { return -4; }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) { return -89; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.SecondWind.GetSpell());
    }
}

