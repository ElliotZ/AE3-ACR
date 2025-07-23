using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.FixedSeq;

public class DblEnshPrep : ISlotSequence
{
    public Action CompletedAction { get; set; }
    
    public int StartCheck()
    {
        if (Core.Me.Level < 80) { return -99; }
        if (SpellsDef.ArcaneCircle.GetSpell().Cooldown.TotalMilliseconds > 5500) { return -6; }
        if (SpellsDef.Enshroud.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (Qt.Instance.GetQt("神秘环") == false) { return -98; }
        if (Core.Resolve<JobApi_Reaper>().ShroudGauge < 50) { return -1; }
        if (Core.Me.HasAura(AurasDef.SoulReaver) || Core.Me.HasAura(AurasDef.Executioner)) { return -10; }
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public List<Action<Slot>> Sequence { get; } = 
    [
        Step0,
        Step1,
        Step2,
    ];

    private static void Step0(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.Enshroud, SpellTargetType.Self));
        slot.Add(new Spell(SpellsDef.ShadowOfDeath, SpellTargetType.Target));
    }

    private static void Step1(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.VoidReaping, SpellTargetType.Target));
        
    }
    private static void Step2(Slot slot)
    {
        if (Core.Me.GetCurrTarget().HasMyAuraWithTimeleft(AurasDefine.DeathsDesign, 40000) && 
            SpellsDef.HarvestMoon.GetSpell().IsReadyWithCanCast())
            slot.Add(new Spell(SpellsDef.HarvestMoon, SpellTargetType.Target));
        else
            slot.Add(new Spell(SpellsDef.ShadowOfDeath, SpellTargetType.Target));
        if (Qt.Instance.GetQt("爆发药"))
        {
            if (BattleData.Instance.numBurstPhases == 0)
            {
                if (ItemHelper.CheckCurrJobPotion())
                {
                    slot.Add(new SlotAction(SlotAction.WaitType.None, 0, Spell.CreatePotion()));
                    slot.Add(new Spell(SpellsDef.ArcaneCircle, SpellTargetType.Self));
                }
                else
                {
                    slot.Add(new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, SpellsDef.ArcaneCircle.GetSpell()));
                }
            }
            else
            {
                slot.Add(new Spell(SpellsDef.ArcaneCircle, SpellTargetType.Self));
                slot.Add(new SlotAction(SlotAction.WaitType.None, 0, Spell.CreatePotion()));
            }
        }
        BattleData.Instance.numBurstPhases++;
    }
}
