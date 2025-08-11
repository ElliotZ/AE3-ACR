using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class Reprisal : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!Qt.Instance.GetQt("减伤"))
        {
            return -101;
        }

        if (!Qt.Instance.GetQt("雪仇"))
        {
            return -100;
        }

        if (!7535u.GetSpell().IsReadyWithCanCast())
        {
            return -50;
        }

        if (TargetHelper.targetCastingIsBossAOE(Core.Me.GetCurrTarget(), 10000))
        {
            return 2;
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

        List<uint> auras = [3255u, 409u, 810u];
        if (num >= 3)
        {
            if (!16140u.GetSpell().IsReadyWithCanCast() && !7531u.GetSpell().IsReadyWithCanCast() && !16148u.GetSpell().IsReadyWithCanCast())
            {
                if (Core.Me.HasAura(1834u) || Core.Me.HasAnyAura(auras, 2000) || Core.Me.HasAura(1832u))
                {
                    return -8;
                }

                return 5;
            }

            return -5;
        }

        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(7535u.GetSpell());
    }
}
