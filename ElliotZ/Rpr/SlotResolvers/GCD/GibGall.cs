using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GibGall : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Gibbet.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        if (enemyCount >= 3) return SpellsDef.Guillotine;
        if (Core.Me.HasAura(AurasDef.EnhancedGallows)) { return SpellsDef.Gallows; }
        if (Core.Me.HasAura(AurasDef.EnhancedGibbet)) { return SpellsDef.Gibbet; }
        if (Helper.AtRear)
        {
            return SpellsDef.Gallows;
        }
        else
        {
            return SpellsDef.Gibbet;
        }
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
