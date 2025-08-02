using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class AutoBloodBath : ISlotResolver
{
    public int Check()
    {
        var BloodBathThreshold = Core.Me.MaxHp * RprSettings.Instance.BloodBathPercent;

        if (RprSettings.Instance.AutoBloodBath == false) { return -1; }
        if (SpellsDef.Bloodbath.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Me.CurrentHp > BloodBathThreshold) { return -4; }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) { return -89; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Bloodbath.GetSpell());
    }
}
