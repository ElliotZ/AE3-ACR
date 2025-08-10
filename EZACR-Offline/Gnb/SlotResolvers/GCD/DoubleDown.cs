using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class DoubleDown : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.Gcd;


    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);
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

        if (Core.Me.Level < 90) { return -5;  } 
        if (Qt.Instance.GetQt("自动拉怪")) { return -1; } 
        if (!25760u.GetSpell().IsReadyWithCanCast()) { return -3; } 
        if (Core.Resolve<JobApi_GunBreaker>().Ammo < 1) { return -2; } 
        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 5f) { return -4; }

        //if (!战斗爽Helper.战斗爽())
        //{
        //    return -99;
        //}

        if (16138u.CoolDownInGCDs(2) && !Qt.Instance.GetQt("无视无情")) { return -6; }
        if (16153u.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("优先音速破")) { return -7; }
        if (Qt.Instance.GetQt("倾泻爆发") && Core.Resolve<JobApi_GunBreaker>().Ammo >= 1) { return 10; }
        if (!Qt.Instance.GetQt("爆发")) { return -10; }
        if (!Qt.Instance.GetQt("倍攻")) { return -8; }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(25760u.GetSpell());
    }
}
