using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class DangerZone : ISlotResolver
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

        if (Core.Me.Level > 79)
        {
            return -3;
        }

        if (Core.Me.Level < 18)
        {
            return -4;
        }

        if (!16144u.GetSpell().IsReadyWithCanCast())
        {
            return -88;
        }

        if (Core.Me.HasAnyAura(GnbHelper.ContBuffs))
        {
            return -99;
        }

        if (16138u.CoolDownInGCDs(2) && !Qt.Instance.GetQt("无视无情"))
        {
            return -5;
        }

        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -1;
        }

        if (Qt.Instance.GetQt("倾泻爆发"))
        {
            return 2;
        }

        if (!Qt.Instance.GetQt("爆发"))
        {
            return -6;
        }

        if (!Qt.Instance.GetQt("爆破领域"))
        {
            return -7;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16144u.GetSpell());
    }
}
