using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using ElliotZ.Rpr.SlotResolvers.FixedSeq;
using ElliotZ.Rpr.SlotResolvers.GCD;
using ElliotZ.Rpr.SlotResolvers.oGCD;
using ElliotZ.Rpr.Triggers;

namespace ElliotZ.Rpr;

public class RprRotationEntry : IRotationEntry, IDisposable
{
    public string AuthorName { get; set; } = Helper.AuthorName;
    private readonly Jobs _targetJob = Jobs.Reaper;
    private readonly AcrType _acrType = AcrType.Normal;
    private readonly int _minLevel = 1;
    private readonly int _maxLevel = 100;
    private readonly string _description = "RPR试水";

    private List<SlotResolverData> _slotResolvers =
    [
        // GCD
        new(new EnshroudSk(), SlotMode.Gcd),
        new(new GibGall(), SlotMode.Gcd),
        new(new PerfectioHighPrio(), SlotMode.Gcd),
        new(new BuffMaintain(), SlotMode.Gcd),
        new(new GaugeGainCD(), SlotMode.Gcd),
        new(new Perfectio(), SlotMode.Gcd),
        new(new PlentifulHarvest(), SlotMode.Gcd),
        new(new Base(), SlotMode.Gcd),
        new(new Ranged(), SlotMode.Gcd),

        // oGCD
        new(new EnshroudAb(), SlotMode.OffGcd),
        new(new EnshroudHighPrio(), SlotMode.OffGcd),
        new(new ArcaneCircle(), SlotMode.OffGcd),
        new(new TrueNorth(), SlotMode.OffGcd),
        new(new Gluttony(), SlotMode.OffGcd),
        new(new Enshroud(), SlotMode.OffGcd),
        new(new BloodStalk(), SlotMode.OffGcd),
    ];

    public Rotation? Build(string settingFolder)
    {
        RprSettings.Build(settingFolder);
        Qt.Build();
        var rot = new Rotation(_slotResolvers)
        {
            TargetJob = _targetJob,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description,
        };
        rot.AddOpener(level => level < 100 ? null : new Opener100());
        rot.SetRotationEventHandler(new EventHandler());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey());
        rot.AddTriggerCondition(new TriggerCondQt());
        rot.AddCanUseHighPrioritySlotCheck(Helper.HighPrioritySlotCheckFunc);
        rot.AddSlotSequences(new DblEnshPrep());
        return rot;
    }

    public IRotationUI GetRotationUI() { return Qt.Instance; }
    public void OnDrawSetting() { }
    public void Dispose() { }
}
