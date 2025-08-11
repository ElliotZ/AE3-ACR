using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class Camouflage : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!16140u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        List<uint> auras = [3255u, 409u, 810u, 1836u];
        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 992)
        {
            return -66;
        }

        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 1168)
        {
            return -67;
        }

        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 922)
        {
            return -68;
        }

        if (!Qt.Instance.GetQt("减伤"))
        {
            return -5;
        }

        if (MoveHelper.IsMoving())
        {
            return -6;
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

        if (num >= 3)
        {
            if (!16148u.GetSpell().IsReadyWithCanCast() || 16148u.RecentlyUsed())
            {
                if (Core.Me.HasAura(1834u) || Core.Me.HasAnyAura(auras, 2000))
                {
                    return -8;
                }

                if (16152u.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("自动超火") && num >= 4)
                {
                    return -7;
                }

                return 0;
            }

            return -9;
        }

        if (Qt.Instance.GetQt("自动拉怪"))
        {
            return -11;
        }

        if ((ulong)Core.Me.GetCurrTarget().MaxHp > (ulong)((long)Core.Me.MaxHp * 12L) && Core.Me.Level > 50)
        {
            return -12;
        }

        if (!Qt.Instance.GetQt("自动拉怪") && (!16148u.GetSpell().IsReadyWithCanCast() || 16148u.RecentlyUsed()))
        {
            if (Core.Me.CurrentHpPercent() > GnbSettings.Instance.伪装阈值)
            {
                return -13;
            }

            if (Core.Me.CurrentHpPercent() <= GnbSettings.Instance.伪装阈值 && !Core.Me.HasAnyAura(auras, 2000))
            {
                return 3;
            }

            if (Core.Me.CurrentHpPercent() <= GnbSettings.Instance.伪装阈值 && (Core.Me.HasAura(1834u, 1000) || Core.Me.HasAura(1191u, 1000)))
            {
                return 3;
            }
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16140u.GetSpell());
    }
}
