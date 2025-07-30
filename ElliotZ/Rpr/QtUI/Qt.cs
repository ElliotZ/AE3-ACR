using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI.Hotkey;

namespace ElliotZ.Rpr.QtUI;

public static class Qt
{
    public static JobViewWindow Instance { get; set; }

    /// <summary>
    /// 除了爆发药以外都复原
    /// </summary>
    public static void Reset()
    {
        Instance.SetQt("起手", true);
        Instance.SetQt("单魂衣", false);
        Instance.SetQt("神秘环", true);
        Instance.SetQt("大丰收", true);
        Instance.SetQt("灵魂割", true);
        Instance.SetQt("挥割/爪", true);
        Instance.SetQt("暴食", true);
        Instance.SetQt("魂衣", true);
        Instance.SetQt("完人", true);
        Instance.SetQt("真北", true);
        Instance.SetQt("收获月", true);
        Instance.SetQt("勾刃", true);
        Instance.SetQt("AOE", true);
        Instance.SetQt("播魂种", true);
        Instance.SetQt("祭牲", true);
        Instance.SetQt("倾泻资源", false);
        Instance.SetQt("真北优化", true);
        Instance.SetQt("智能AOE", true);
        Instance.SetQt("自动突进", false);
    }

    public static void Build()
    {
        Instance = new JobViewWindow(RprSettings.Instance.JobViewSave, RprSettings.Instance.Save, "EZRpr");
        Instance.AddQt("爆发药", false);
        Instance.AddQt("爆发药2分", true);
        Instance.AddQt("起手", true);
        Instance.AddQt("单魂衣", false, "不给爆发留魂衣，爆发只会有1个大丰收送的魂衣");
        Instance.AddQt("神秘环", true);
        Instance.AddQt("大丰收", true);
        Instance.AddQt("灵魂割", true, "灵魂切割以及AOE灵魂钐割");
        Instance.AddQt("挥割/爪", true, "隐匿挥割以及派生的缢杀爪/绞决爪，以及AOE束缚挥割");
        Instance.AddQt("暴食", true);
        Instance.AddQt("魂衣", true);
        Instance.AddQt("完人", true);
        Instance.AddQt("真北", true);
        Instance.AddQt("收获月", true);
        Instance.AddQt("勾刃", true);
        Instance.AddQt("AOE", true);
        Instance.AddQt("播魂种", true);
        Instance.AddQt("祭牲", true);
        Instance.AddQt("倾泻资源", false);
        Instance.AddQt("真北优化", true);
        Instance.AddQt("智能AOE", true);
        Instance.AddQt("自动突进", false);

        Instance.AddHotkey("入境", new IngressHK(IngressHK.CurrDir));
        Instance.AddHotkey("出境", new EgressHK(IngressHK.CurrDir));
        Instance.AddHotkey("入境<t>", new IngressHK(IngressHK.FaceTarget));
        Instance.AddHotkey("出境<t>", new EgressHK(IngressHK.FaceTarget));
        Instance.AddHotkey("入境<cam>", new IngressHK(IngressHK.FaceCam));
        Instance.AddHotkey("出境<cam>", new EgressHK(IngressHK.FaceCam));
        Instance.AddHotkey("神秘纹", new HotKeyResolver(SpellsDef.ArcaneCrest, SpellTargetType.Self));
        Instance.AddHotkey("LB", new HotKeyResolver_LB());
        Instance.AddHotkey("亲疏", new HotKeyResolver(SpellsDef.ArmsLength, SpellTargetType.Self));
        Instance.AddHotkey("内丹", new HotKeyResolver(SpellsDef.SecondWind, SpellTargetType.Self));
        Instance.AddHotkey("浴血", new HotKeyResolver(SpellsDef.Bloodbath, SpellTargetType.Self));
        Instance.AddHotkey("牵制", new HotKeyResolver(SpellsDef.Feint, SpellTargetType.Target));
        Instance.AddHotkey("真北", new HotKeyResolver(SpellsDef.TrueNorth, SpellTargetType.Self));
        Instance.AddHotkey("播魂种", new SoulSowHvstMnHK());
        Instance.AddHotkey("疾跑", new HotKeyResolver_疾跑());
        Instance.AddHotkey("爆发药", new HotKeyResolver_Potion());

        //其余tab窗口
        ReadmeTab.Build(Instance);
        SettingTab.Build(Instance);
        DevTab.Build(Instance);

    }
}
