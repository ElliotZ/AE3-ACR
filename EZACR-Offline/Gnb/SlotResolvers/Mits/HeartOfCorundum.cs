using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class HeartOfCorundum : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public static Spell GetSpell()
    {
        if (Core.Me.Level < 82)
        {
            return 16161u.GetSpell();
        }

        return 25758u.GetSpell();
    }

    public int Check()
    {
        if (Core.Me.Level >= 82 && !25758u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        if (Core.Me.Level < 82 && !16161u.GetSpell().IsReadyWithCanCast())
        {
            return -12;
        }

        List<uint> auras = [3255u, 409u, 810u];
        if (!Qt.Instance.GetQt("减伤"))
        {
            return -2;
        }

        if (!Qt.Instance.GetQt("自动刚玉"))
        {
            return -5;
        }

        int num = 0;
        foreach (KeyValuePair<uint, IBattleChara> item in TargetMgr.Instance.EnemysIn20)
        {
            IBattleChara value = item.Value;
            if (value.CanAttack() && Core.Me.Distance(value) <= 5f && value.TargetObjectId == Core.Me.GameObjectId)
            {
                if (Core.Me.CurrentHpPercent() <= 0.5f)
                {
                    num++;
                }
                else if (!TTKHelper.IsTargetTTK(value, 10, ignoreBossCheck: false))
                {
                    num++;
                }
            }
        }

        if (TargetHelper.targetCastingIsDeathSentence(Core.Me.GetCurrTarget()) && Core.Me.GetCurrTarget().TargetObjectId == Core.Me.GameObjectId)
        {
            return 4;
        }

        if (num >= 3)
        {
            if (Core.Me.HasAnyAura(auras, 2000))
            {
                return -9;
            }

            return 0;
        }

        if (Core.Me.CurrentHpPercent() < GnbSettings.Instance.刚玉之心阈值 && Core.Me.HasAnyAura(auras, 2000))
        {
            return -1;
        }

        if (Core.Me.CurrentHpPercent() > GnbSettings.Instance.刚玉之心阈值)
        {
            return -8;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        Spell spell = GetSpell();
        if (spell != null)
        {
            slot.Add(spell);
        }
    }
}
