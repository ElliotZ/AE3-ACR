using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using ElliotZ.Common;

namespace ElliotZ.Rpr.SlotResolvers.FixedSeq;

public class DblEnshPrep : ISlotSequence
{
    public Action CompletedAction { get; set; }
    
    public int StartCheck()
    {
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public List<Action<Slot>> Sequence { get; } = new()
    {
        Step0,
        Step1,
        Step2,
    };

    private static void Step0(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.Enshroud, SpellTargetType.Self));
        slot.Add(new Spell(SpellsDef.ShadowOfDeath, SpellTargetType.Target));

    }

    private static void Step1(Slot slot)
    {
        if (Core.Me.GetCurrTarget().HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, 40000) && Core.Me.HasAura(AurasDefine.Soulsow))
            slot.Add(new Spell(SpellsDef.HarvestMoon, SpellTargetType.Target));
        else
            slot.Add(new Spell(SpellsDef.ShadowOfDeath, SpellTargetType.Target));
        slot.Add(new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, Spell.CreatePotion()));

    }
    private static void Step2(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.VoidReaping, SpellTargetType.Target));
        slot.Add(new Spell(SpellsDef.ArcaneCircle, SpellTargetType.Self));
    }
}
