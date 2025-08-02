using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.oGCD;

public class Sacrificum : ISlotResolver
{
    private IBattleChara? Target { get; set; }
    public int Check()
    {
        Target = SpellsDef.Sacrificium.OptimalAOETarget(1, Qt.Instance.GetQt("智能AOE"), 5);
        if (Target is null || SpellsDef.Sacrificium.GetSpell().IsReadyWithCanCast() == false)
        {
            return -99;
        }

        if (Qt.Instance.GetQt("神秘环") &&
                (!Core.Me.HasAura(AurasDef.ArcaneCircle) &&
                 SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds <= 10000) ||
                 BattleData.Instance.justCastAC)
        {
            return -6;  // -6 for delaying for burst prep
        }
        
        // add QT
        // might need to check for death's design
        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(SpellsDef.Sacrificium.GetSpell(Target));
    }
}
