using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Common.Hotkey;
using EZACR_Offline.Gnb.QtUI.Hotkey;

namespace EZACR_Offline.Gnb.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }
    public static MacroManager macroMan;
    public static MobPullManager mobMan;

    public static readonly List<(string name, string ENname, bool defval, string tooltip)> QtKeys =
    [
        ("使用基础Gcd", "BaseGCD", true, ""),
        ("AOE", "AOE", true, ""),
        ("爆发", "Burst", true, ""),
        ("狮心连", "LionheartCombo", true, ""),
        ("子弹连", "CartCombo", true, ""),
        ("倍攻", "DblDown", true, ""),
        ("血壤", "Bloodfest", true, ""),
        ("爆发击", "BurstStrike", true, "包括命运环"),
        ("爆发药", "Pot", false, ""),
        ("音速破", "SonicBreak", true, ""),
        ("爆破领域", "BlastingZone", true, ""),
        ("极光", "Aurora", true, ""),
        ("减伤", "Mits", true, ""),
        ("雪仇", "Reprisal", true, ""),
        ("闪雷弹", "LightningShot", true, ""),
        ("突进起手", "GapCloseOpen", true, ""),
        ("弓形冲", "BowShock", true, ""),
        ("自动刚玉", "AutoHoC", false, ""),
        ("自动超火", "AutoBolide", false, ""),
        ("无情后半", "LateWeaveNM", false, "是否无情全后半GCD开"),
        ("自动拉怪", "AutoPull", false, "自动拉怪期间不打爆发"),
        ("倾泻资源", "Dump", false, "资源无保留,全力输出无视爆发QT"),
        ("爆发药卸豆子", "DumpCartUnderPot", false, "爆发药卸豆子/卸多了会乱轴"),
        ("无情", "NoMercy", true, ""),
        ("TP开怪", "TPPull", false, "暂未实装.是否使用TP开怪 覆盖突进"),
        ("无视无情", "IgnoreNM", false, "用于无视无情状态打爆发"),
        ("强制变命运", "ForceFatedCircle", false, "用于爆发击强制变命运之环"),
        ("强制爆发击", "ForceBurstStrike", false, "强制打爆发击 开启有豆子就打"),
        ("优先音速破", "SonicBreakPrio", false, "优先打音速破，对1G2.5起手生效"),
    ];

    public static readonly List<(string name, string ENname, IHotkeyResolver hkr)> HKResolvers =
    [
        ("超火", "Bolide", new HotKeyResolver(SpellsDef.Superbolide, SpellTargetType.Self, false)),
        ("亲疏", "Armslength", new HotKeyResolver(SpellsDef.ArmsLength, SpellTargetType.Self)),
        ("极光<me>", "Nebula<me>", new HotKeyResolver(SpellsDef.Aurora, SpellTargetType.Self)),
        ("刚玉<me>", "HoC<me>", new HotKeyResolver(SpellsDef.HeartOfCorundum, SpellTargetType.Self)),
        ("雪仇", "Reprisal", new HotKeyResolver(SpellsDef.Reprisal, SpellTargetType.Self)),
        ("光之心", "HoL", new HotKeyResolver(SpellsDef.HeartofLight, SpellTargetType.Self)),
        ("极光<2>", "Nebula<2>", new HotKeyResolver(SpellsDef.Aurora, SpellTargetType.Pm2)),
        ("刚玉<2>", "HoC<2>", new HotKeyResolver(SpellsDef.HeartOfCorundum, SpellTargetType.Pm2)),
        ("挑衅", "Voke", new HotKeyResolver(SpellsDef.Provoke, SpellTargetType.Target, false)),
        ("退避<2>", "Shirk<2>", new HotKeyResolver(SpellsDef.Shirk, SpellTargetType.Pm2, false)),
        ("LB", "LB", new HotKeyResolver_LB()),
        ("插言", "Interject", new HotKeyResolver(SpellsDef.Interject, SpellTargetType.Target, false)),
        ("下踢", "LowBlow", new HotKeyResolver(SpellsDef.LowBlow, SpellTargetType.Target)),
        ("选自己", "TargetSelf", new SelectSelf()),
        ("清理HPQ", "ClearHPQ", new ToiletFlusher()),
        ("刚玉血量最低", "HoCLowest", new HoCLowest()),
        ("疾跑", "Sprint", new HotKeyResolver_疾跑()),
        ("爆发药", "Pot", new HotKeyResolver_Potion()),
        ("一键减伤", "OneKeyMits", new OneKeyMits()),
    ];

    public static void SaveQtStates()
    {
        string[] qtArray = Instance.GetQtArray();
        foreach (string name in qtArray)
        {
            bool state = Instance.GetQt(name);
            GnbSettings.Instance.QtStates[name] = state;
        }

        GnbSettings.Instance.Save();
        LogHelper.Print("QT设置已保存");
    }

    public static void LoadQtStates()
    {
        foreach (KeyValuePair<string, bool> qtState in GnbSettings.Instance.QtStates)
        {
            Instance.SetQt(qtState.Key, qtState.Value);
        }

        if (GnbSettings.Instance.Debug) LogHelper.Print("QT设置已重载");
    }

    public static void LoadQtStatesNoPot()
    {
        foreach (KeyValuePair<string, bool> qtState in GnbSettings.Instance.QtStates)
        {
            if (qtState.Key is not "爆发药")
            {
                Instance.SetQt(qtState.Key, qtState.Value);
            }
        }

        if (GnbSettings.Instance.Debug) LogHelper.Print("除爆发药以外QT设置已重载");
    }

    public static void Build()
    {
        Instance = new JobViewWindow(GnbSettings.Instance.JobViewSave, GnbSettings.Instance.Save, "EZGnb");
        Instance.SetUpdateAction(OnUIUpdate);
        macroMan = new MacroManager(Instance, "/EZGnb", QtKeys, HKResolvers, true);
        mobMan = new MobPullManager(Instance);
        mobMan.BurstQTs.Add("爆发");

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
        DevTab.Build(Instance);

    }

    public static void OnUIUpdate()
    {
        //macroMan.UseToast2 = false;
        if (GnbSettings.Instance.CommandWindowOpen)
        {
            macroMan.DrawCommandWindow(ref GnbSettings.Instance.CommandWindowOpen);
        }
    }
}
