using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class LionHeart : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.Gcd;
    //private bool CheckSpell()
    //{
    //    if (Core.Resolve<MemApiSpell>().CheckActionChange(36937u.GetSpell().Id).IsReady())
    //    {
    //        return true;
    //    }

    //    return false;
    //}

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

        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -19;
        }

        if (Qt.Instance.GetQt("自动拉怪"))
        {
            return -1;
        }

        if (!36937u.GetSpell().IsReadyWithCanCast())
        {
            return -10;
        }

        if (!Qt.Instance.GetQt("爆发"))
        {
            return -30;
        }

        if (!Qt.Instance.GetQt("狮心连"))
        {
            return -40;
        }

        if (!Core.Me.HasAura(3840u) && Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 36939)
        {
            return -4;
        }

        if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 36937 || Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 36938)
        {
            return 5;
        }

        if (Core.Me.HasMyAuraWithTimeleft(3840u, 3000))
        {
            return 39;
        }

        if (16138u.CoolDownInGCDs(2) && !Qt.Instance.GetQt("无视无情") && Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 36937 && Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 36938)
        {
            return -25;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(36937u).GetSpell());
    }

}
