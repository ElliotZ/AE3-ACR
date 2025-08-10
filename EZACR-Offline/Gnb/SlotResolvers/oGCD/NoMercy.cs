using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class NoMercy : ISlotResolver
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

        if (!16138u.GetSpell().IsReadyWithCanCast())
        {
            return -5;
        }

        if (Qt.Instance.GetQt("自动拉怪"))
        {
            return -1;
        }

        //if (!战斗爽Helper.战斗爽() && GunbreakerSettings.Instance.ACRMode == "Normal")
        //{
        //    return -2;
        //}

        //if (TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), 10, ignoreBossCheck: true) && GnbSettings.Instance.ACRMode == "Normal")
        //{
        //    return -2;
        //}

        if (Qt.Instance.GetQt("倾泻爆发"))
        {
            return 3;
        }

        if (!Qt.Instance.GetQt("爆发"))
        {
            return -3;
        }

        if (!Qt.Instance.GetQt("无情"))
        {
            return -42;
        }

        if (Qt.Instance.GetQt("无情后半"))
        {
            if (GCDHelper.GetGCDCooldown() > 700)
            {
                return -8;
            }

            if (Qt.Instance.GetQt("倾泻爆发"))
            {
                return 3;
            }

            if (!Qt.Instance.GetQt("爆发"))
            {
                return -3;
            }

            if (!Qt.Instance.GetQt("无情"))
            {
                return -42;
            }

            if (16138u.GetSpell().IsReadyWithCanCast() && 25760u.GetSpell().IsReadyWithCanCast())
            {
                return 8;
            }

            return 9;
        }

        if (GCDHelper.GetGCDCooldown() < 300)
        {
            return -9;
        }

        if (Qt.Instance.GetQt("倾泻爆发"))
        {
            return 3;
        }

        if (!Qt.Instance.GetQt("爆发"))
        {
            return -3;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16138u.GetSpell());
    }

}
