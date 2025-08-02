using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class HarvestMoonHighPrio : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.HarvestMoon.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("收获月") == false) { return -98; }
        if (!Qt.Instance.GetQt("倾泻资源")) { return -1; }
        if (Core.Me.HasAura(AurasDef.SoulReaver) || Core.Me.HasAura(AurasDef.Executioner))
        {
            return -10;  // -10 for protecting SoulReaver/Executioner
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.HarvestMoon.GetSpell());
    }
}
