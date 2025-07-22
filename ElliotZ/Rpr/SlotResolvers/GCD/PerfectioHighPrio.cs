using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class PerfectioHighPrio : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Perfectio.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("完人") == false) { return -98; }
        if (Helper.GetAuraTimeLeft(AurasDef.PerfectioParata) > 2500) { return -8; }  // -8 for Exiting High Prio state
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Perfectio.GetSpell());
    }
}
