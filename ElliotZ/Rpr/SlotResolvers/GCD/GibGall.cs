using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GibGall : ISlotResolver
{
    private static uint currGibbet => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gibbet);
    private static uint currGallows => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gallows);
    private static uint currGuillotine => Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Guillotine);
    public int Check()
    {
        if (currGibbet.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        if (Qt.Instance.GetQt("AOE") && enemyCount >= 3) { return currGuillotine; }
        if (Core.Me.HasAura(AurasDef.EnhancedGallows)) { return currGallows; }
        if (Core.Me.HasAura(AurasDef.EnhancedGibbet)) { return currGibbet; }
        if (Helper.AtRear)
        {
            return currGallows;
        }
        else
        {
            return currGibbet;
        }
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
