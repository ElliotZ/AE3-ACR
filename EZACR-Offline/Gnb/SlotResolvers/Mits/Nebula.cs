using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class Nebula : ISlotResolver {
  public SlotMode SlotMode { get; } = SlotMode.OffGcd;

  public static Spell GetSpell() {
    if (Core.Me.Level >= 94) return 36935u.GetSpell();

    return 16148u.GetSpell();
  }

  public int Check() {
    if (!16148u.GetSpell().IsReadyWithCanCast()) return -3;

    if (MoveHelper.IsMoving()) return -6;

    List<uint> auras = [3255u, 409u, 810u, 1836u];
    if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 992) return -66;

    if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 1168) return -67;

    if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 922) return -68;

    if (!Qt.Instance.GetQt("减伤")) return -5;

    int num = 0;

    foreach (var item in TargetMgr.Instance.EnemysIn20) {
      IBattleChara value = item.Value;

      if (value.CanAttack()
       && (Core.Me.Distance(value) <= 5f)
       && (value.TargetObjectId == Core.Me.GameObjectId)) {
        if (Core.Me.CurrentHpPercent() <= 0.5f) {
          num++;
        } else if (!TTKHelper.IsTargetTTK(value, 10, false)) num++;
      }
    }

    if (num >= 3) {
      if (7531u.RecentlyUsed() || 16140u.RecentlyUsed()) return -8;

      if (Core.Me.HasAura(1191u) || Core.Me.HasAura(1832u) || Core.Me.HasAnyAura(auras, 2000)) {
        return -1;
      }

      return 0;
    }

    if (Qt.Instance.GetQt("自动拉怪")) return -7;

    if (Helper.TargetIsBossOrDummy && (Core.Me.Level > 50)) return -10;

    if (!Qt.Instance.GetQt("自动拉怪")) {
      if ((Core.Me.CurrentHpPercent() < GnbSettings.Instance.星云阈值)
       && !Core.Me.HasAnyAura(auras, 3000)) {
        return 3;
      }

      if ((Core.Me.CurrentHpPercent() < GnbSettings.Instance.星云阈值)
       && (!Core.Me.HasAura(1832u, 1000) || !Core.Me.HasAura(1191u, 1000))) {
        return 4;
      }

      if (Core.Me.CurrentHpPercent() > GnbSettings.Instance.星云阈值) return -8;
    }

    return 0;
  }

  public void Build(Slot slot) {
    Spell spell = GetSpell();
    if (spell != null) slot.Add(spell);
  }
}
