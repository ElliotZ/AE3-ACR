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

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class Ranged : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Harpe.IsUnlock() == false) return -99;
        // Add QT
        return 0;
    }

    private static uint Solve()
    {
        if (Core.Me.HasAura(AurasDef.Soulsow)) return SpellsDef.HarvestMoon;
        return SpellsDef.Harpe;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
