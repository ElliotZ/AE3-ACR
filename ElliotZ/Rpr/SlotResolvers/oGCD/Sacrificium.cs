using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Sacrificum : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Sacrificium.GetSpell().IsReadyWithCanCast() == false)
        {
            return -99;
        }

        if (Qt.Instance.GetQt("神秘环") &&
                !Core.Me.HasAura(AurasDef.ArcaneCircle) &&
                 SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 10000)
        {
            return -6;  // -6 for delaying for burst prep
        }
        
        // add QT
        // might need to check for death's design
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Sacrificium.GetSpell());
    }
}
