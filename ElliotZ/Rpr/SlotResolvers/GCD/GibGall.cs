using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GibGall : ISlotResolver
{
    private IBattleChara? Target {  get; set; }
    public int Check()
    {
        if (SpellsDef.Gluttony.GetSpell().RecentlyUsed()) { return 9; }  // 9 for server acq ignore
        if (Core.Me.HasAura(AurasDef.Enshrouded)) { return -14; }
        if (Helper.GetActionChange(SpellsDef.Gibbet).GetSpell().IsReadyWithCanCast() == false) { return -99; }
        return 0;
    }

    private Spell Solve()
    {
        //var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        Target = SpellsDef.Guillotine.OptimalAOETarget(3, 180);

        if (Qt.Instance.GetQt("AOE") && (Target is not null)) 
        { 
            return Helper.GetActionChange(SpellsDef.Guillotine).GetSpell(Target!); 
        }
        if (Core.Me.HasAura(AurasDef.EnhancedGallows)) 
        { 
            return Helper.GetActionChange(SpellsDef.Gallows).GetSpell(); 
        }
        if (Core.Me.HasAura(AurasDef.EnhancedGibbet)) 
        { 
            return Helper.GetActionChange(SpellsDef.Gibbet).GetSpell(); 
        }
        if (Helper.AtRear)
        {
            return Helper.GetActionChange(SpellsDef.Gallows).GetSpell();
        }
        else
        {
            return Helper.GetActionChange(SpellsDef.Gibbet).GetSpell();
        }
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve());
    }
}
