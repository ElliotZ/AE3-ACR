using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI.Hotkey;

namespace ElliotZ.Rpr.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }
    public static MacroManager macroMan;
    public static MobPullManager mobMan;

    public static readonly List<(string name, string ENname, bool defval, string tooltip)> QtKeys =
    [
        ("爆发药", "Pot", false, ""),
        ("爆发药2分", "Pot2min", true, ""),
        ("起手", "Opener", true, ""),
        ("单魂衣", "SingleShroud", false, "不给爆发留魂衣，爆发只会有1个大丰收送的魂衣"),
        ("神秘环", "ArcaneCircle", true, ""),
        ("大丰收", "PlenHar", true, ""),
        ("灵魂割", "SoulSkill", true, "灵魂切割以及AOE灵魂钐割"),
        ("挥割/爪", "Bloodstalk", true, "隐匿挥割以及派生的缢杀爪/绞决爪，以及AOE束缚挥割"),
        ("暴食", "Gluttony", true, ""),
        ("魂衣", "Enshroud", true, ""),
        ("完人", "Perfectio", true, ""),
        ("真北", "TrueNorth", true, ""),
        ("收获月", "HarvestMoon", true, ""),
        ("勾刃", "Harpe", true, ""),
        ("AOE", "AOE", true, ""),
        ("播魂种", "Soulsow", true, ""),
        ("祭牲", "Sacrificium", true, ""),
        ("倾泻资源", "Dump", false, "会扔收获月，会尽快变身"),
        ("真北优化", "OptiNorth", true, "够红条但是身位不对的时候会攒一攒"),
        ("智能AOE", "SmartAOE", true, "会自动选择AOE目标，包括暴食团契这类技能"),
        ("自动突进", "AutoIngress", false, "只会在跳了之后能打到的时候跳，能用勾刃就不会跳"),
    ];

    public static readonly List<(string name, string ENname, IHotkeyResolver hkr)> HKResolvers =
    [
        ("入境", "Ingress", new IngressHK(IngressHK.CurrDir)),
        ("出境", "Egress", new EgressHK(IngressHK.CurrDir)),
        ("入境<t>", "Ingress<t>", new IngressHK(IngressHK.FaceTarget)),
        ("出境<t>", "Egress<t>", new EgressHK(IngressHK.FaceTarget)),
        ("入境<cam>", "Ingress<cam>", new IngressHK(IngressHK.FaceCam)),
        ("出境<cam>", "Egress<cam>", new EgressHK(IngressHK.FaceCam)),
        ("神秘纹", "Crest", new HotKeyResolver(SpellsDef.ArcaneCrest, SpellTargetType.Self)),
        ("LB", "LB", new HotKeyResolver_LB()),
        ("亲疏", "Armslength", new HotKeyResolver(SpellsDef.ArmsLength, SpellTargetType.Self)),
        ("内丹", "SecondWind", new HotKeyResolver(SpellsDef.SecondWind, SpellTargetType.Self)),
        ("浴血", "BloodBath", new HotKeyResolver(SpellsDef.Bloodbath, SpellTargetType.Self)),
        ("牵制", "Feint", new HotKeyResolver(SpellsDef.Feint)),
        ("真北", "TrueNorth", new HotKeyResolver(SpellsDef.TrueNorth, SpellTargetType.Self)),
        ("播魂种", "Soulsow", new SoulSowHvstMnHK()),
        ("疾跑", "Sprint", new HotKeyResolver_疾跑()),
        ("爆发药", "Pot", new HotKeyResolver_Potion()),
    ];

    public static void SaveQtStates()
    {
        string[] qtArray = Instance.GetQtArray();
        foreach (string name in qtArray)
        {
            bool state = Instance.GetQt(name);
            RprSettings.Instance.QtStates[name] = state;
        }

        RprSettings.Instance.Save();
        LogHelper.Print("QT设置已保存");
    }

    public static void LoadQtStates()
    {
        foreach (KeyValuePair<string, bool> qtState in RprSettings.Instance.QtStates)
        {
            Instance.SetQt(qtState.Key, qtState.Value);
        }

        if (RprSettings.Instance.Debug) LogHelper.Print("QT设置已重载");
    }

    public static void LoadQtStatesNoPot()
    {
        foreach (KeyValuePair<string, bool> qtState in RprSettings.Instance.QtStates)
        {
            if (qtState.Key is not ("爆发药" or
                                    "智能AOE" or
                                    "爆发药2分" or
                                    "自动突进"))
            {
                Instance.SetQt(qtState.Key, qtState.Value);
            }
        }

        if (RprSettings.Instance.Debug) LogHelper.Print("除爆发药和智能AOE以外QT设置已重载");
    }

    public static void Build()
    {
        Instance = new JobViewWindow(RprSettings.Instance.JobViewSave, RprSettings.Instance.Save, "EZRpr");
        Instance.SetUpdateAction(OnUIUpdate);

        macroMan = new MacroManager(Instance, "/EZRpr", QtKeys, HKResolvers, true);
        macroMan.BuildCommandList();
        macroMan.AddQt("爆发", "burst", true, "", delegate (bool isSet)
        {
            Instance.SetQt("神秘环", isSet);
            Instance.SetQt("魂衣", isSet);
        });

        mobMan = new MobPullManager(Instance);
        mobMan.BurstQTs.Add("爆发");

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
        DevTab.Build(Instance);

    }

    public static void OnUIUpdate()
    {
        macroMan.UseToast2 = RprSettings.Instance.ShowToast;
        if (RprSettings.Instance.CommandWindowOpen)
        {
            macroMan.DrawCommandWindow(ref RprSettings.Instance.CommandWindowOpen);
        }
    }
}
