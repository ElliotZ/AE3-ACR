using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class PlentifulHarvest : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.PlentifulHarvest.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("大丰收") == false) { return -98; }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.PlentifulHarvest.GetSpell());
    }
}
