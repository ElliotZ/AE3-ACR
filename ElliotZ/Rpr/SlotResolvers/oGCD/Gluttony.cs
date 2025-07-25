using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using static AEAssist.CombatRoutine.View.MeleePosHelper;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Gluttony : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.Gluttony.GetSpell().IsReadyWithCanCast() == false)
        {
            return -99;
        }
        if (Qt.Instance.GetQt("暴食") == false) { return -98; }

        if (Core.Me.HasAura(AurasDef.Executioner) ||
                Core.Me.HasAura(AurasDef.SoulReaver) ||
                Core.Resolve<JobApi_Reaper>().ShroudGauge > 80)
        {
            return -4;  // -4 for Overcapped Resources
        }
        if (!Qt.Instance.GetQt("单魂衣") && Core.Me.HasAura(AurasDef.ArcaneCircle))
        {
            return -12;  // delay for burst window
        }
        if (Helper.ComboTimer < 2 * GCDHelper.GetGCDDuration() + GCDHelper.GetGCDCooldown() &&
                (RprHelper.PrevCombo == SpellsDef.Slice || RprHelper.PrevCombo == SpellsDef.WaxingSlice))
        {
            return -9;  // -9 for combo protection
        }
        // add QT
        // might need to check for death's design
        return 0;
    }

    public void Build(Slot slot)
    {
        MeleePosHelper2.Clear();
        if (Core.Me.HasAura(AurasDef.EnhancedGallows))
        {
            MeleePosHelper2.DrawMeleePosOffset(Pos.Behind,
                                               BattleData.Instance.GcdDuration,
                                               Helper.GetActionChange(SpellsDef.Gallows));
        }
        if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
        {
            MeleePosHelper2.DrawMeleePosOffset(Pos.Flank,
                                               BattleData.Instance.GcdDuration,
                                               Helper.GetActionChange(SpellsDef.Gibbet));
        }
        slot.Add(SpellsDef.Gluttony.GetSpell());
    }
}
