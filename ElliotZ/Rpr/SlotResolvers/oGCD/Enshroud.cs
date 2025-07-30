using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using ElliotZ.Rpr.SlotResolvers.FixedSeq;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Enshroud : ISlotResolver
{
    private static bool DeadZoneCheck()
    {
        var accd = SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds;
        var neededShroud = (Core.Me.HasAura(AurasDef.IdealHost) ? 50 : 100) -
                              Core.Resolve<JobApi_Reaper>().ShroudGauge;
        var neededSoul = neededShroud * 5 - Core.Resolve<JobApi_Reaper>().SoulGauge;
        var soulSliceCharge = SpellsDef.SoulSlice.GetSpell().Charges;
        var gluttonyPossible = SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds < 
                               (accd - DblEnshPrep.PreAcEnshTimer - GCDHelper.GetGCDDuration() * 3);
        var deathsDesgnTime = Core.Resolve<MemApiBuff>().GetAuraTimeleft(Core.Me.GetCurrTarget(), 
                                                                         AurasDef.DeathsDesign, true);
        var totalTimeBeforeAC = accd - 
                                GCDHelper.GetGCDDuration() * 2 - 
                                6000 - 
                                GCDHelper.GetGCDCooldown() - 
                                DblEnshPrep.PreAcEnshTimer;
        var totalNumGCDsBeforeAC = (int)Math.Ceiling(totalTimeBeforeAC / GCDHelper.GetGCDDuration() + 0.5);
        if (gluttonyPossible) neededSoul -= 50;
        var NumGCDForShroud = neededShroud / 10;
        var NumGCDForSoul = neededSoul / 10;
        var NumGCDForDD = (int)Math.Ceiling((accd - DblEnshPrep.PreAcEnshTimer - deathsDesgnTime) / 30000);
        var maxPossibleSoulSliceUses = Math.Floor((accd - DblEnshPrep.PreAcEnshTimer) / 30000 + soulSliceCharge);
        NumGCDForSoul -= (int)maxPossibleSoulSliceUses * 4;
        return totalNumGCDsBeforeAC >= NumGCDForSoul + NumGCDForShroud + NumGCDForDD;
    }
    public int Check()
    {
        if (SpellsDef.Enshroud.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("����") == false) { return -98; }
        if (Core.Me.Distance(Core.Me.GetCurrTarget()) > SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -2;  // -2 for not in range
        }

        //if (Qt.Instance.GetQt("������") && Qt.Instance.GetQt("���ػ�") &&
        //    SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 20000) 
        //{ 
        //    return -6;  // burst prep
        //}
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 100)
        {
            if (!Qt.Instance.GetQt("������") && // Qt.Instance.GetQt("���ػ�") &&
                //SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 40000)
                !DeadZoneCheck())
            {
                return -6;
            }
            if (Qt.Instance.GetQt("��ʳ") &&
                    !Core.Me.HasAura(AurasDef.ArcaneCircle) && 
                    SpellsDef.Gluttony.GetSpell().Cooldown.TotalMilliseconds <= 20000 && 
                    SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds >= 55000)
            {
                return -7;
            }
        }
        if (Core.Me.HasAura(AurasDef.SoulReaver) || Core.Me.HasAura(AurasDef.Executioner))
        {
            return -10;  // protect Gib/Gallows
        }
        if (SpellsDef.SoulSlice.GetSpell().Charges > 1.6f && Core.Resolve<JobApi_Reaper>().ShroudGauge < 90)
        {
            return -11;
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