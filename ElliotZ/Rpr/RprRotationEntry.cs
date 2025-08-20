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
    public static string SettingsFolderPath = "";

    private readonly List<SlotResolverData> _slotResolvers =
    [
        // GCD
        new(new EnshroudSk(), SlotMode.Gcd),
        new(new GibGall(), SlotMode.Gcd),
        new(new PerfectioHighPrio(), SlotMode.Gcd),
        new(new HarvestMoonHighPrio(), SlotMode.Gcd),
        new(new SoulSow(), SlotMode.Gcd),
        new(new BuffMaintain(), SlotMode.Gcd),
        new(new GaugeGainCD(), SlotMode.Gcd),
        new(new Perfectio(), SlotMode.Gcd),
        new(new PlentifulHarvest(), SlotMode.Gcd),
        new(new Base(), SlotMode.Gcd),
        new(new HarvestMoon(), SlotMode.Gcd),
        new(new Harpe(), SlotMode.Gcd),

        // oGCD
        new(new EnshroudAb(), SlotMode.OffGcd),
        new(new Sacrificum(), SlotMode.OffGcd),
        new(new EnshroudHighPrio(), SlotMode.OffGcd),
        new(new ArcaneCircle(), SlotMode.OffGcd),
        new(new TrueNorth(), SlotMode.OffGcd),
        new(new Gluttony(), SlotMode.OffGcd),
        new(new Enshroud(), SlotMode.OffGcd),
        new(new BloodStalk(), SlotMode.OffGcd),
        new(new AutoCrest(), SlotMode.OffGcd),
        new(new AutoFeint(), SlotMode.OffGcd),
        new(new AutoBloodBath(), SlotMode.OffGcd),
        new(new AutoSecondWind(), SlotMode.OffGcd),

        // Low Prio Always
        new(new Ingress(), SlotMode.Always),
    ];

    public Rotation? Build(string settingFolder)
    {
        SettingsFolderPath = settingFolder;
        RprSettings.Build(SettingsFolderPath);
        GlobalSetting.Build(SettingsFolderPath, "EZRpr", false);
        Qt.Build();
        var rot = new Rotation(_slotResolvers)
        {
            TargetJob = _targetJob,
            AcrType = _acrType,
            MinLevel = _minLevel,
            MaxLevel = _maxLevel,
            Description = _description,
        };
        rot.AddOpener(level => level < 88 ? new OpenerCountDownOnly() : new Opener100());
        rot.SetRotationEventHandler(new EventHandler());
        rot.AddTriggerAction(new TriggerActionQt(), new TriggerActionHotkey());
        rot.AddTriggerCondition(new TriggerCondQt());
        rot.AddCanUseHighPrioritySlotCheck(Helper.HighPrioritySlotCheckFunc);
        rot.AddSlotSequences(new DblEnshPrep());
        return rot;
    }

    public string CheckFirstAvailableSkillGCD()
    {
        SlotResolverData? slotResolverData =
            _slotResolvers.FirstOrDefault((SlotResolverData srd) =>
                                              srd.SlotMode == SlotMode.Gcd &&
                                              srd.SlotResolver.Check() >= 0);
        return (slotResolverData != null) ?
                   slotResolverData.SlotResolver.GetType().Name : "无技能";
    }

    public string CheckFirstAvailableSkilloffGCD()
    {
        SlotResolverData? slotResolverData =
            _slotResolvers.FirstOrDefault((SlotResolverData srd) =>
                                              srd.SlotMode == SlotMode.OffGcd &&
                                              srd.SlotResolver.Check() >= 0);
        return (slotResolverData != null) ?
                   slotResolverData.SlotResolver.GetType().Name : "无技能";
    }

    public IRotationUI GetRotationUI() { return Qt.Instance; }
    public void OnDrawSetting() { }
    public void Dispose() { }
}
