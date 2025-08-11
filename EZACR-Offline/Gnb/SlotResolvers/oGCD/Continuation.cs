using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class Continuation : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public static bool CheckSpell()
    {
        return Core.Me.HasAnyAura(GnbHelper.ContBuffs);
    }

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

        if (!CheckSpell())
        {
            return -5;
        }

        if (!Qt.Instance.GetQt("无情后半") && !Qt.Instance.GetQt("无视无情") && 16138u.CoolDownInGCDs(1) && GCDHelper.GetGCDCooldown() > 900)
        {
            return -4;
        }

        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > (float)(SettingMgr.GetSetting<GeneralSettings>().AttackRange + 2))
        {
            return -1;
        }

        if (Qt.Instance.GetQt("倾泻爆发"))
        {
            return 2;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(Core.Resolve<MemApiSpell>().CheckActionChange(16155u).GetSpell());
    }
}
