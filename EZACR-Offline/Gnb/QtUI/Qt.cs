using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using ECommons;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }

    public static readonly (string name, string ENname, bool defval, string tooltip)[] QtKeys =
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

    public static readonly (string name, string ENname, IHotkeyResolver hkr)[] HKResolvers =
    [
        ("超火", "Bolide", new IngressHK(IngressHK.CurrDir)),
        ("亲疏", "Armslength", new HotKeyResolver(SpellsDef.ArmsLength, SpellTargetType.Self)),
        ("极光", "Egress", new EgressHK(IngressHK.CurrDir)),
        ("入境<t>", "Ingress<t>", new IngressHK(IngressHK.FaceTarget)),
        ("出境<t>", "Egress<t>", new EgressHK(IngressHK.FaceTarget)),
        ("入境<cam>", "Ingress<cam>", new IngressHK(IngressHK.FaceCam)),
        ("出境<cam>", "Egress<cam>", new EgressHK(IngressHK.FaceCam)),
        ("神秘纹", "Crest", new HotKeyResolver(SpellsDef.ArcaneCrest, SpellTargetType.Self)),
        ("LB", "LB", new HotKeyResolver_LB()),
        ("内丹", "SecondWind", new HotKeyResolver(SpellsDef.SecondWind, SpellTargetType.Self)),
        ("浴血", "BloodBath", new HotKeyResolver(SpellsDef.Bloodbath, SpellTargetType.Self)),
        ("牵制", "Feint", new HotKeyResolver(SpellsDef.Feint)),
        ("真北", "TrueNorth", new HotKeyResolver(SpellsDef.TrueNorth, SpellTargetType.Self)),
        ("播魂种", "Soulsow", new SoulSowHvstMnHK()),
        ("疾跑", "Sprint", new HotKeyResolver_疾跑()),
        ("爆发药", "Pot", new HotKeyResolver_Potion()),
    ];

    private static readonly List<(string cmdType, string CNCmd, string ENCmd)> cmdList = [];

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
            if (qtState.Key is not ("爆发药" or
                                    "智能AOE" or
                                    "爆发药2分" or
                                    "自动突进"))
            {
                Instance.SetQt(qtState.Key, qtState.Value);
            }
        }

        if (GnbSettings.Instance.Debug) LogHelper.Print("除爆发药和智能AOE以外QT设置已重载");
    }

    public static void Build()
    {
        Instance = new JobViewWindow(GnbSettings.Instance.JobViewSave, GnbSettings.Instance.Save, "EZGnb");
        Instance.SetUpdateAction(OnUIUpdate);
        foreach ((string name, string en, bool defVal, string tooltip) in QtKeys)
        {
            Instance.AddQt(name, defVal, tooltip);
            var cncmd = GnbHelper.TxtCmdHandle + " " + name + "_qt";
            var encmd = GnbHelper.TxtCmdHandle + " " + en.ToLower() + "_qt";
            cmdList.Add(("QT", cncmd, encmd));
        }

        foreach ((string name, string en, IHotkeyResolver hkr) in HKResolvers)
        {
            Instance.AddHotkey(name, hkr);
            var cncmd = GnbHelper.TxtCmdHandle + " " + name + "_hk";
            var encmd = GnbHelper.TxtCmdHandle + " " + en.ToLower() + "_hk";
            cmdList.Add(("Hotkey", cncmd, encmd));
        }

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
        DevTab.Build(Instance);

    }

    public static List<(string cmdType, string CNCmd, string ENCmd)> CmdList()
    {
        return cmdList;
    }

    public static void OnUIUpdate()
    {
        if (GnbSettings.Instance.CommandWindowOpen)
        {
            GnbCmdWindow.Draw();
        }
    }
}
