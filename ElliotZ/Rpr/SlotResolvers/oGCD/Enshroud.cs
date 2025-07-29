using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Enshroud : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Enshroud.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("魂衣") == false) { return -98; }

        //if (Qt.Instance.GetQt("单魂衣") && Qt.Instance.GetQt("神秘环") &&
        //    SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 20000) 
        //{ 
        //    return -6;  // burst prep
        //}
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 100)
        {
            if (!Qt.Instance.GetQt("单魂衣") && // Qt.Instance.GetQt("神秘环") &&
                SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 40000)
            {
                return -6;
            }
            if (Qt.Instance.GetQt("暴食") &&
                    !Core.Me.HasAura(AurasDef.ArcaneCircle) && 
                    SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds <= 20000 && 
                    SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds >= 55000)
            {
                return -6;
            }
        }
        if (Core.Me.HasAura(AurasDef.SoulReaver) || Core.Me.HasAura(AurasDef.Executioner))
        {
            return -10;  // protect Gib/Gallows
        }
        if (Helper.AoeTtkCheck() && TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget())) 
        { 
            return -16;  // delay for next pack
        }

        //if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 50 && !Core.Me.HasAura(AurasDef.IdealHost)) return -1;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Enshroud.GetSpell());
    }
}