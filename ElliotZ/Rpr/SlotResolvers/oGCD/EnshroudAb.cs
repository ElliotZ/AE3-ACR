﻿using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class EnshroudAb : ISlotResolver
{
    private int purpOrb => Core.Resolve<JobApi_Reaper>().VoidShroud;
    //private int enemyCount => TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
    private IBattleChara? Target { get; set; }

    public int Check()
    {
        Target = SpellsDef.LemuresScythe.OptimalAOETarget(3, 180, Qt.Instance.GetQt("智能AOE"));

        if (Core.Me.HasAura(AurasDef.Enshrouded) == false) { return -3; }  // -3 for Unmet Prereq Conditions
        if (Target is null && 
                Core.Me.Distance(Core.Me.GetCurrTarget()) > SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -2;  // -2 for not in range
        }
        if (Target is not null && SpellsDef.LemuresScythe.GetSpell(Target!).IsReadyWithCanCast())
        {
            return -2;
        }
        //if (SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 5000 &&
        //        Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign, 30000))
        //{
        //    return -6;  // -6 for delaying for burst prep
        //}
        if (purpOrb < 2) { return -3; }
        //if (GCDHelper.GetGCDCooldown() < 800) return -7;  // -7 for avoiding clipping
        return 0;
    }

    private Spell Solve()
    {
        //var enemyCount = TargetHelper.GetEnemyCountInsideSector(Core.Me, Core.Me.GetCurrTarget(), 8, 180);
        //Target = SpellsDef.LemuresScythe.OptimalAOETarget(3, 180);

        if (Qt.Instance.GetQt("AOE") && 
                Target is not null && 
                SpellsDef.LemuresScythe.GetSpell(Target!).IsReadyWithCanCast()) 
        { 
            return SpellsDef.LemuresScythe.GetSpell(Target!); 
        }
        return SpellsDef.LemuresSlice.GetSpell();
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve());
    }
}
