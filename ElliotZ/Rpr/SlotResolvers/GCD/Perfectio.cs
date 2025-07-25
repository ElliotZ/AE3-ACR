using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class Perfectio : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Perfectio.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        //if (Core.Me.HasAura(AurasDef.PerfectioParata) == false) { return -99; }
        if (Qt.Instance.GetQt("完人") == false) { return -98; }
        if (Helper.ComboTimer < GCDHelper.GetGCDDuration() + 200 &&
        (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Perfectio.GetSpell());
    }
}
