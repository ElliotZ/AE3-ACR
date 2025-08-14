using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class ArcaneCircle : ISlotResolver
{
    public int Check()
    {
        if (SpellsDef.ArcaneCircle.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("神秘环") == false) { return -98; }

        if (Helper.AoeTtkCheck() && TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget()))
        {
            return -16;  // delay for next pack
        }
        if (Qt.mobMan.Holding) return -3;

        if (AI.Instance.BattleData.CurrBattleTimeInMs < 5000 &&
                SpellsDef.SoulScythe.GetSpell().Charges == 2)
        {
            return -11;
        }
        if (GCDHelper.GetGCDCooldown() < RprSettings.Instance.AnimLock) return -89;
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.ArcaneCircle.GetSpell());
    }
}
