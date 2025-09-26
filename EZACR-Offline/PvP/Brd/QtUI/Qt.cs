using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using Dalamud.Bindings.ImGui;
using ElliotZ;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP.Brd.QtUI;

public static class Qt {
  public static JobViewWindow Instance { get; private set; }
  public static MacroManager MacroMan;

  private static readonly List<QtInfo> _qtKeys = [
      new("和弦箭", "", true, null, ""),
      new("光阴神", "", true, null, ""),
      new("沉默", "", true, null, ""),
      new("爆破箭", "", true, null, ""),
      new("绝峰箭", "", true, null, ""),
      new("强劲射击", "", true, null, ""),
      new("喝热水", "", true, null, ""),
      new("职能技能", "", true, null, ""),
      new("自动净化", "", true, null, ""),
      new("龟壳", "", true, null, ""),
      new("冲刺", "", true, null, ""),
  ];

  private static readonly List<HotKeyInfo> _hkResolvers = [
      new("疾跑", "", new HotKeyResolver_NormalSpell(29057U, SpellTargetType.Self)),
      new("龟壳", "", new HotKeyResolver_NormalSpell(29054U, SpellTargetType.Self)),
      new("热水", "", new HotKeyResolver_NormalSpell(29711U, SpellTargetType.Self)),
      new("LB", "", new HotkeyBardLB()),
      new("后射", "", new HotKeyRepellingShot()),
  ];
  
  public static void SaveQtStates() {
    string[] qtArray = Instance.GetQtArray();

    foreach (string name in qtArray) {
      bool state = Instance.GetQt(name);
      PvPBrdSettings.Instance.QtStates[name] = state;
    }

    PvPBrdSettings.Instance.Save();
    LogHelper.Print("QT设置已保存");
  }

  public static void LoadQtStates() {
    foreach (var qtState in PvPBrdSettings.Instance.QtStates) {
      Instance.SetQt(qtState.Key, qtState.Value);
    }

    LogHelper.Print("QT设置已重载");
  }

  public static void Build() {
    Instance = new JobViewWindow(PvPBrdSettings.Instance.JobViewSave,
                                 PvPBrdSettings.Instance.Save,
                                 "EzPvPBrd");
    Instance.SetUpdateAction(OnUIUpdate);

    MacroMan = new MacroManager(Instance,
                                "/EZPvPBrd",
                                _qtKeys,
                                _hkResolvers,
                                true);
    MacroMan.BuildCommandList();

    Instance.AddTab("职业配置", PvPBrdOverlay.DrawGeneral);
    Instance.AddTab("监控", CommonUI.BuildMonitorWindow);
    Instance.AddTab("共通配置", CommonUI.BuildCommonSettings);
    Instance.AddTab("Dev", DevTab);
  }

  private static void OnUIUpdate() {
    if (PvPBrdSettings.Instance.CommandWindowOpen) {
      MacroMan.DrawCommandWindow(ref PvPBrdSettings.Instance.CommandWindowOpen);
    }

    if (PvPSettings.Instance.监控) {
      CommonUI.MonitorWindow(ref PvPSettings.Instance.监控);
    }
  }

  private static void DevTab(JobViewWindow instance) {
    ImGui.Text(BattleData.Instance.ToString());
    CommonUI.BuildPvPDebug(instance);
  }
}
