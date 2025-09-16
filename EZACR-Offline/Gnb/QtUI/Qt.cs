using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using ElliotZ;
using ElliotZ.Hotkey;
using EZACR_Offline.Gnb.QtUI.Hotkey;
using JobViewWindow = ElliotZ.ModernJobViewFramework.JobViewWindow;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace EZACR_Offline.Gnb.QtUI;

public static class Qt {
  public static JobViewWindow Instance { get; private set; }
  public static MacroManager MacroMan;
  public static MobPullManager MobMan;

  private static readonly List<QtInfo> _qtKeys = [
      new("使用基础Gcd", "BaseGCD", true, null, ""),
      new("AOE", "AOE", true, null, ""),
      new("爆发", "Burst", true, null, ""),
      new("狮心连", "LionheartCombo", true, null, ""),
      new("子弹连", "CartCombo", true, null, ""),
      new("倍攻", "DblDown", true, null, ""),
      new("血壤", "Bloodfest", true, null, ""),
      new("爆发击", "BurstStrike", true, null, "包括命运环"),
      new("爆发药", "Pot", false, null, ""),
      new("音速破", "SonicBreak", true, null, ""),
      new("爆破领域", "BlastingZone", true, null, ""),
      new("极光", "Aurora", true, null, ""),
      new("减伤", "Mits", true, null, ""),
      new("雪仇", "Reprisal", true, null, ""),
      new("闪雷弹", "LightningShot", true, null, ""),
      new("突进起手", "GapCloseOpen", true, null, ""),
      new("弓形冲", "BowShock", true, null, ""),
      new("自动刚玉", "AutoHoC", false, null, ""),
      new("自动超火", "AutoBolide", false, null, ""),
      new("无情后半", "LateWeaveNM", false, null, "是否无情全后半GCD开"),
      new("自动拉怪", "AutoPull", false, null, "自动拉怪期间不打爆发"),
      new("倾泻资源", "Dump", false, null, "资源无保留,全力输出无视爆发QT"),
      new("爆发药卸豆子", "DumpCartUnderPot", false, null, "爆发药卸豆子/卸多了会乱轴"),
      new("无情", "NoMercy", true, null, ""),
      new("TP开怪", "TPPull", false, null, "暂未实装.是否使用TP开怪 覆盖突进"),
      new("无视无情", "IgnoreNM", false, null, "用于无视无情状态打爆发"),
      new("强制变命运", "ForceFatedCircle", false, null, "用于爆发击强制变命运之环"),
      new("强制爆发击", "ForceBurstStrike", false, null, "强制打爆发击 开启有豆子就打"),
      new("优先音速破", "SonicBreakPrio", false, null, "优先打音速破，对1G2.5起手生效"),
  ];

  private static readonly List<HotKeyInfo> _hkResolvers = [
      new("超火", "Bolide", new HotKeyResolver(SpellsDef.Superbolide, SpellTargetType.Self, false)),
      new("亲疏", "Armslength", new HotKeyResolver(SpellsDef.ArmsLength, SpellTargetType.Self)),
      new("极光<me>", "Nebula<me>", new HotKeyResolver(SpellsDef.Aurora, SpellTargetType.Self)),
      new("刚玉<me>", "HoC<me>", new HotKeyResolver(SpellsDef.HeartOfCorundum, SpellTargetType.Self)),
      new("雪仇", "Reprisal", new HotKeyResolver(SpellsDef.Reprisal, SpellTargetType.Self)),
      new("光之心", "HoL", new HotKeyResolver(SpellsDef.HeartofLight, SpellTargetType.Self)),
      new("极光<2>", "Nebula<2>", new HotKeyResolver(SpellsDef.Aurora, SpellTargetType.Pm2)),
      new("刚玉<2>", "HoC<2>", new HotKeyResolver(SpellsDef.HeartOfCorundum, SpellTargetType.Pm2)),
      new("挑衅", "Voke", new HotKeyResolver(SpellsDef.Provoke, SpellTargetType.Target, false)),
      new("退避<2>", "Shirk<2>", new HotKeyResolver(SpellsDef.Shirk, SpellTargetType.Pm2, false)),
      new("LB", "LB", new HotKeyResolver_LB()),
      new("插言", "Interject", new HotKeyResolver(SpellsDef.Interject, SpellTargetType.Target, false)),
      new("下踢", "LowBlow", new HotKeyResolver(SpellsDef.LowBlow)),
      new("选自己", "TargetSelf", new SelectSelf()),
      new("清理HPQ", "ClearHPQ", new ToiletFlusher()),
      new("刚玉血量最低", "HoCLowest", new HoCLowest()),
      new("疾跑", "Sprint", new HotKeyResolver_疾跑()),
      new("爆发药", "Pot", new HotKeyResolver_Potion()),
      new("一键减伤", "OneKeyMits", new OneKeyMits()),
  ];

  public static void SaveQtStates() {
    string[] qtArray = Instance.GetQtArray();

    foreach (string name in qtArray) {
      bool state = Instance.GetQt(name);
      GnbSettings.Instance.QtStates[name] = state;
    }

    GnbSettings.Instance.Save();
    LogHelper.Print("QT设置已保存");
  }

  public static void LoadQtStates() {
    foreach (var qtState in GnbSettings.Instance.QtStates) {
      Instance.SetQt(qtState.Key, qtState.Value);
    }

    if (GnbSettings.Instance.Debug) LogHelper.Print("QT设置已重载");
  }

  public static void LoadQtStatesNoPot() {
    foreach (var qtState in
             GnbSettings.Instance.QtStates.Where(qtState => qtState.Key is not "爆发药")) {
      Instance.SetQt(qtState.Key, qtState.Value);
    }

    if (GnbSettings.Instance.Debug) LogHelper.Print("除爆发药以外QT设置已重载");
  }

  public static void Build() {
    Instance = new JobViewWindow(GnbSettings.Instance.JobViewSave,
                                 GnbSettings.Instance.Save,
                                 "EZGnb");
    Instance.SetUpdateAction(OnUIUpdate);
    MacroMan = new MacroManager(Instance, "/EZGnb", _qtKeys, _hkResolvers, true);
    MobMan = new MobPullManager(Instance);
    MobMan.BurstQTs.Add("爆发");

    //其余tab窗口
    ReadmeTab.Build(Instance);
    SettingTab.Build(Instance);
    DevTab.Build(Instance);
  }

  private static void OnUIUpdate() {
    //macroMan.UseToast2 = false;
    if (GnbSettings.Instance.CommandWindowOpen) {
      MacroMan.DrawCommandWindow(ref GnbSettings.Instance.CommandWindowOpen);
    }
  }
}
