using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class AutoFeint : ISlotResolver
{
    public int Check()
    {
        if (RprSettings.Instance.AutoFeint == false) { return -1; }
        if (SpellsDef.Feint.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Core.Me.GetCurrTarget() is null ||
                !TargetHelper.targetCastingIsDeathSentenceWithTime(Core.Me.GetCurrTarget(), 2500)) { return -3; }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) { return -89; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Feint.GetSpell());
    }
}
