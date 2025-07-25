using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.FixedSeq;

public class Opener100 : IOpener
{
    public int StartCheck()
    {
        if (Qt.Instance.GetQt("起手") == false) { return -98; }
        if (Qt.Instance.GetQt("神秘环") == false) { return -98; }
        if (Core.Me.Level < 88) { return -99; }  // might not need this
        if (SpellsDef.SoulSlice.IsMaxChargeReady(0.0f)== false) { return -99; }
        if (SpellsDef.ArcaneCircle.GetSpell().IsReadyWithCanCast() == false) { return -99; }
        if (SpellsDef.Gluttony.CoolDownInGCDs(3) == false) { return -6; }
        if (TargetHelper.GetNearbyEnemyCount(5) > 2) { return -13; }  // opener is basically only meant for single target
        return 0;
    }

    public int StopCheck(int index)
    {
        return -1;
    }

    public void InitCountDown(CountDownHandler cdh)
    {
        Qt.Reset();

        const int startTime = 15000;
        if (!Core.Me.HasAura(AurasDef.Soulsow))
        {
            cdh.AddAction(startTime, SpellsDef.Soulsow);
        }
        cdh.AddAction(RprSettings.Instance.PrepullCastTimeHarpe, 
                      () => SpellsDef.Harpe.GetSpell(SpellTargetType.Target));
    }

    public List<Action<Slot>> Sequence { get; } = [Step0, Step1];

    private static void Step0(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.ShadowOfDeath, SpellTargetType.Target));
        if (Qt.Instance.GetQt("爆发药") && !Qt.Instance.GetQt("爆发药2分") && !RprSettings.Instance.TripleWeavePot)
        {
            slot.Add(new SlotAction(SlotAction.WaitType.WaitInMs,
                                    GCDHelper.GetGCDDuration() - RprSettings.Instance.AnimLock, 
                                    Spell.CreatePotion()));
        }
    }

    private static void Step1(Slot slot)
    {
        slot.Add(new Spell(SpellsDef.SoulSlice, SpellTargetType.Target));
        if (RprSettings.Instance.TripleWeavePot && Qt.Instance.GetQt("爆发药") && !Qt.Instance.GetQt("爆发药2分"))
        {
            slot.Add(new SlotAction(SlotAction.WaitType.WaitInMs,
                        GCDHelper.GetGCDDuration() - RprSettings.Instance.AnimLock * 3,
                        SpellsDef.ArcaneCircle.GetSpell()));
            slot.Add(Spell.CreatePotion());
            slot.Add(new Spell(SpellsDef.Gluttony, SpellTargetType.Target));
        }
        else 
        {
            slot.Add(new SlotAction(SlotAction.WaitType.WaitInMs,
                                    GCDHelper.GetGCDDuration() - 2000,
                                    SpellsDef.ArcaneCircle.GetSpell()));
            slot.Add(new Spell(SpellsDef.Gluttony, SpellTargetType.Target));
            //LogHelper.Error("why does this not work");
        }
    }

    public uint Level { get; } = 88;

    public Action CompletedAction { get; set; }
}
