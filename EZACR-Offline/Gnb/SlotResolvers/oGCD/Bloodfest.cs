using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.JobApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class Bloodfest : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

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

        if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0)
        {
            return -1;
        }

        if (!Qt.Instance.GetQt("爆发"))
        {
            return -2;
        }

        if (!Qt.Instance.GetQt("血壤"))
        {
            return -7;
        }

        if (!16164u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        if (Qt.Instance.GetQt("自动拉怪"))
        {
            return -15;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16164u.GetSpell());
    }
}
