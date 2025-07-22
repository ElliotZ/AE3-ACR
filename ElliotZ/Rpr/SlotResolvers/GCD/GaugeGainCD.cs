using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GaugeGainCD : ISlotResolver
{
    //private static uint PrevCombo => Core.Resolve<MemApiSpell>().GetLastComboSpellId();

    public int Check()
    {
        if (SpellsDef.SoulSlice.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("灵魂割") == false) { return -98;  }  // -98 for QT toggled off
        if (Core.Resolve<JobApi_Reaper>().SoulGauge > 50) { return -4; }  // -4 for Overcapped Resources
        if (RprHelper.ComboTimer <= GCDHelper.GetGCDDuration() && 
                (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        return 0;
    }

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);
        if (Qt.Instance.GetQt("AOE") && SpellsDef.SoulScythe.GetSpell().IsReadyWithCanCast() && enemyCount >= 3)
        {
            return SpellsDef.SoulScythe;
        }
        return SpellsDef.SoulSlice;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
