using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class GibGall : ISlotResolver {
  private IBattleChara? _target { get; set; }

  public int Check() {
    _target = SpellsDef.Guillotine.OptimalAOETarget(3, 
                                                   180, 
                                                   Qt.Instance.GetQt("智能AOE"));

    if (SpellsDef.Gluttony.GetSpell().RecentlyUsed()) return 9; // 9 for server acq ignore

    if (Core.Me.HasAura(AurasDef.Enshrouded)) return -14;

    if (_target is null
     && Helper.GetActionChange(SpellsDef.Gibbet)
              .GetSpell()
              .IsReadyWithCanCast() is false) {
      return -99;
    }

    if (_target is not null 
     && SpellsDef.Guillotine.GetSpell(_target).IsReadyWithCanCast() is false) {
      return -99;
    }

    return 0;
  }

  private Spell Solve() {
    //var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);

    if (Qt.Instance.GetQt("AOE") && _target is not null) {
      return Helper.GetActionChange(SpellsDef.Guillotine).GetSpell(_target);
    }

    if (Core.Me.HasAura(AurasDef.EnhancedGallows)) {
      return Helper.GetActionChange(SpellsDef.Gallows).GetSpell();
    }

    if (Core.Me.HasAura(AurasDef.EnhancedGibbet)) {
      return Helper.GetActionChange(SpellsDef.Gibbet).GetSpell();
    }

    return Helper.AtRear
               ? Helper.GetActionChange(SpellsDef.Gallows).GetSpell()
               : Helper.GetActionChange(SpellsDef.Gibbet).GetSpell();
  }

  public void Build(Slot slot) {
    slot.Add(Solve());
  }
}
