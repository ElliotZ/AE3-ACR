using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.PvP.Brd.QtUI;

namespace EZACR_Offline.PvP.Brd.SlotResolvers;

public class 光阴神 : ISlotResolver {
  public int Check() {
    bool 光阴队友 = PvPBrdSettings.Instance.光阴队友;
    if (!Qt.Instance.GetQt("光阴神")) return -3;
    if (!PvPHelper.CanActive()) return -1;
    if (!29400U.GetSpell().IsReadyWithCanCast()) return -2;

    if (光阴队友) {
      if ((!PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].HasCanDispel()
        && !Core.Me.HasCanDispel())
       || (PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].HasCanDispel()
        && (PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].DistanceToPlayer() > 30.0))) {
        return -9;
      }

      IBattleChara? mate = PartyHelper.CastableParty.FirstOrDefault(chara => chara.HasCanDispel());
      if (mate == null) return -9;
      if (mate.DistanceToPlayer() > 30.0) return -6;
    } else if (!Core.Me.HasCanDispel()) {
      return -10;
    }

    return 1;
  }

  public void Build(Slot slot) {
    bool 光阴队友 = PvPBrdSettings.Instance.光阴队友;
    bool 光阴播报 = PvPBrdSettings.Instance.光阴播报;
    IBattleChara? target;

    if (光阴队友) {
      target = !PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].HasCanDispel()
            || !PartyHelper.CastableParty.Contains(PartyHelper.Party[PvPBrdSettings.Instance.光阴对象])
                   ? PartyHelper.CastableParty.FirstOrDefault(chara => chara.HasCanDispel())
                   : PartyHelper.Party[PvPBrdSettings.Instance.光阴对象];

      if (target == null) {
        return;
      }
    } else {
      target = Core.Me;
    }

    slot.Add(new Spell(29400U, target) {DontUseGcdOpt = true});

    if (!光阴播报) {
      return;
    }

    LogHelper.Print($"光阴目标:{target.Name}");
  }
}
