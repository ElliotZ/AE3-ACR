using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using ElliotZ.Common;
using EZACR_Offline.Gnb.QtUI;
using EZACR_Offline.Gnb.SlotResolvers.FixedSeq;
using EZACR_Offline.Gnb.SlotResolvers.GCD;
using EZACR_Offline.Gnb.SlotResolvers.Mits;
using EZACR_Offline.Gnb.SlotResolvers.oGCD;
using EZACR_Offline.Gnb.Triggers;

namespace EZACR_Offline.Gnb;

public class GnbRotationEntry : IRotationEntry {
  public List<SlotResolverData> SlotResolvers = [
      // GCD
      new(new DoubleDown(), SlotMode.Gcd),
      new(new SonicBreak(), SlotMode.Gcd),
      new(new GnashingFang(), SlotMode.Gcd),
      new(new LightningShot(), SlotMode.Gcd),
      new(new LionHeart(), SlotMode.Gcd),
      new(new BurstStrike(), SlotMode.Gcd),
      new(new FatedCircle(), SlotMode.Gcd),
      new(new Base(), SlotMode.Gcd),

      // oGCD
      new(new SuperBolide(), SlotMode.OffGcd),
      //new(new Trajectory(), SlotMode.OffGcd),
      new(new NoMercy(), SlotMode.OffGcd),
      new(new Bloodfest(), SlotMode.OffGcd),
      new(new Continuation(), SlotMode.OffGcd),
      new(new BlastingZone(), SlotMode.OffGcd),
      new(new DangerZone(), SlotMode.OffGcd),
      new(new BowShock(), SlotMode.OffGcd),
      new(new RoyalGuard(), SlotMode.OffGcd),
      new(new Provoke(), SlotMode.OffGcd),
      new(new HeartOfCorundum(), SlotMode.OffGcd),
      new(new Nebula(), SlotMode.OffGcd),
      new(new Camouflage(), SlotMode.OffGcd),
      new(new Rampart(), SlotMode.OffGcd),
      new(new Aurora(), SlotMode.OffGcd),
      new(new Armslength(), SlotMode.OffGcd),
      new(new Reprisal(), SlotMode.OffGcd),
  ];

  //public string OverlayTitle { get; } = "EZGnb";
  public string AuthorName { get; set; } = Helper.AuthorName;

  public Rotation Build(string settingFolder) {
    GnbSettings.Build(settingFolder);
    Qt.Build();
    Rotation rotation = new(SlotResolvers) {
        TargetJob = Jobs.Gunbreaker,
        AcrType = AcrType.Both,
        MinLevel = 1,
        MaxLevel = 100,
        Description = "全等级适配 可日随可高难 具体看悬浮窗。",
    };
    rotation.SetRotationEventHandler(new EventHandler());
    rotation.AddTriggerAction(new TriggerAction_QT());
    rotation.AddTriggerAction(new TriggerAction_新QT());
    rotation.AddTriggerCondition(new TriggerAction_Ammo());
    rotation.AddTriggerCondition(new TriggerAction_AmmoLTE());
    rotation.AddTriggerCondition(new TriggerAction_AmmoGTE());
    rotation.AddTriggerCondition(new TriggerAction_AmmoEqual());
    rotation.AddTriggerAction(new TriggerAction_OpenerSelection());
    rotation.AddOpener(GetOpener);
    return rotation;
  }

  public IRotationUI GetRotationUI() {
    return Qt.Instance;
  }

  public void OnDrawSetting() { }

  private static IOpener? GetOpener(uint level) {
    return GnbSettings.Instance.opener switch {
        1 => new Opener100_2gcd(),
        2 => new Opener100_2gcd_mk2(),
        3 => new Opener100_1gcd_mk2(),
        4 => new Opener100_3gcd_mk2(),
        5 => new Opener80_2gcd_TEA(),
        6 => new Opener90_1gcd_OPU(),
        7 => new Opener70_5gcd_UWU(),
        8 => new Opener100_1gcd_0sks(),
        9 => new Opener70_2gcd_UCOB(),
        _ => null,
    };
  }

  public void Dispose() {
    Qt.Instance.Dispose();
  }
}
