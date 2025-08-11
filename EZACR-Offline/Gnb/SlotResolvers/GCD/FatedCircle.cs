using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.JobApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class FatedCircle : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.Gcd;

    public int Check()
    {
        //if (Core.Me.HasAnyAura(Buff.无法发动技能类))
        //{
        //    return -150;
        //}

        //if (Core.Me.HasAnyAura(Buff.无法造成伤害))
        //{
        //    return -151;
        //}

        //if (Core.Me.GetCurrTarget().HasAnyAura(Buff.敌人无敌BUFF))
        //{
        //    return -152;
        //}
        if (Core.Me.Level < 72)
        {
            return -2;
        }
        if (!16163u.GetSpell().IsReadyWithCanCast())
        {
            return -5;
        }

        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 8, 8);
        if (nearbyEnemyCount < 2)
        {
            return -4;
        }
        if (Core.Me.Level < 88)
        {
            if (Core.Resolve<JobApi_GunBreaker>().Ammo < 2)
            {
                return -1;
            }
        }
        else if (Core.Resolve<JobApi_GunBreaker>().Ammo < 3)
        {
            return -8;
        }
        if (!Qt.Instance.GetQt("AOE"))
        {
            return -3;
        }
        if (Qt.Instance.GetQt("倾泻输出") && Core.Resolve<JobApi_GunBreaker>().Ammo > 0)
        {
            return 2;
        }
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16163u.GetSpell());
    }

}
