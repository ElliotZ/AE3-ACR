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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb;

public class GnbRotationEntry : IRotationEntry, IDisposable
{
    public List<SlotResolverData> SlotResolvers =
    [
        // GCD
        new SlotResolverData(new DoubleDown(), SlotMode.Gcd),
        new SlotResolverData(new SonicBreak(), SlotMode.Gcd),
        new SlotResolverData(new GnashingFang(), SlotMode.Gcd),
        new SlotResolverData(new LightningShot(), SlotMode.Gcd),
        new SlotResolverData(new LionHeart(), SlotMode.Gcd),
        new SlotResolverData(new BurstStrike(), SlotMode.Gcd),
        new SlotResolverData(new FatedCircle(), SlotMode.Gcd),
        new SlotResolverData(new Base(), SlotMode.Gcd),

        // oGCD
        new SlotResolverData(new SuperBolide(), SlotMode.OffGcd),
        new SlotResolverData(new Trajectory(), SlotMode.OffGcd),
        new SlotResolverData(new NoMercy(), SlotMode.OffGcd),
        new SlotResolverData(new Bloodfest(), SlotMode.OffGcd),
        new SlotResolverData(new Continuation(), SlotMode.OffGcd),
        new SlotResolverData(new BlastingZone(), SlotMode.OffGcd),
        new SlotResolverData(new DangerZone(), SlotMode.OffGcd),
        new SlotResolverData(new BowShock(), SlotMode.OffGcd),
        new SlotResolverData(new RoyalGuard(), SlotMode.OffGcd),
        new SlotResolverData(new Provoke(), SlotMode.OffGcd),
        new SlotResolverData(new HeartOfCorundum(), SlotMode.OffGcd),
        new SlotResolverData(new Nebula(), SlotMode.OffGcd),
        new SlotResolverData(new Camouflage(), SlotMode.OffGcd),
        new SlotResolverData(new Rampart(), SlotMode.OffGcd),
        new SlotResolverData(new Aurora(), SlotMode.OffGcd),
        new SlotResolverData(new Armslength(), SlotMode.OffGcd),
        new SlotResolverData(new Reprisal(), SlotMode.OffGcd)
    ];

    //private IOpener Opener_GNB_2gcd_100 = new Opener_GNB_2gcd_100();
    //private IOpener Opener_GNB_2gcd_100_Change = new Opener_GNB_2gcd_100_Change();
    //private IOpener Opener_GNB_1gcd_100_Change = new Opener_GNB_1gcd_100_Change();
    //private IOpener Opener_GNB_3gcd_100_Change = new Opener_GNB_3gcd_100_Change();
    //private IOpener Opener_GNB_2gcd_80绝亚 = new Opener_GNB_2gcd_80绝亚();
    //private IOpener Opener_GNB_1gcd_90绝欧 = new Opener_GNB_1gcd_90绝欧();
    //private IOpener Opener_GNB_5gcd_70神兵 = new Opener_GNB_5gcd_70神兵();
    //private IOpener Opener_GNB_1gcd_100_25 = new Opener_GNB_1gcd_100_25();
    //private IOpener Opener_GNB_2gcd_70巴哈 = new Opener_GNB_2gcd_70巴哈();

    //public string OverlayTitle { get; } = "EZGnb";
    public string AuthorName { get; set; } = Helper.AuthorName;

    public Rotation Build(string settingFolder)
    {
        GnbSettings.Build(settingFolder);
        Qt.Build();
        Rotation rotation = new(SlotResolvers)
        {
            TargetJob = Jobs.Gunbreaker,
            AcrType = AcrType.Both,
            MinLevel = 1,
            MaxLevel = 100,
            Description = "全等级适配 可日随可高难 具体看悬浮窗。"
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

    public IRotationUI GetRotationUI() { return Qt.Instance; }
    public void OnDrawSetting() { }

    private IOpener? GetOpener(uint level)
    {
        return GnbSettings.Instance.opener switch
        {
            1 => new Opener100_2gcd(),
            2 => new Opener100_2gcd_mk2(),
            3 => new Opener100_1gcd_mk2(),
            4 => new Opener100_3gcd_mk2(),
            5 => new Opener80_2gcd_TEA(),
            6 => new Opener90_1gcd_OPU(),
            7 => new Opener70_5gcd_UWU(),
            8 => new Opener100_1gcd_0sks(),
            9 => new Opener70_2gcd_UCOB(),
            _ => null
        };
    }
    public void Dispose() { }

}
