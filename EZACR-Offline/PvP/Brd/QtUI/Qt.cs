using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using ElliotZ;
using ElliotZ.ModernJobViewFramework;

namespace EZACR_Offline.PvP.Brd.QtUI;

public static class Qt {
  public static JobViewWindow Instance { get; private set; }
  private static readonly PvPBrdOverlay _lazyOverlay = new();
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
      new("冲刺", "", true, null, ""),
  ];

  private static readonly List<HotKeyInfo> _hkResolvers = [
      new("疾跑", "", new HotKeyResolver_NormalSpell(29057U, SpellTargetType.Self)),
      new("龟壳", "", new HotKeyResolver_NormalSpell(29054U, SpellTargetType.Self)),
      new("热水", "", new HotKeyResolver_NormalSpell(29711U, SpellTargetType.Self)),
      new("LB", "", new HotkeyData.诗人LB()),
      new("后跳", "", new HotkeyData.后射()),
  ];

  public static void Build() {
    Instance = new JobViewWindow(PvPBrdSettings.Instance.JobViewSave,
                                 PvPBrdSettings.Instance.Save,
                                 "EzPvP");
    Instance.SetUpdateAction(OnUIUpdate);

    MacroMan = new MacroManager(Instance,
                                "/EZPvPBrd",
                                _qtKeys,
                                _hkResolvers,
                                true);
    MacroMan.BuildCommandList();

    Instance.AddTab("职业配置", _lazyOverlay.DrawGeneral);
    Instance.AddTab("监控", PVPHelper.监控);
    Instance.AddTab("共通配置", PVPHelper.配置);
  }

  private static void OnUIUpdate() {
    if (PvPBrdSettings.Instance.CommandWindowOpen) {
      MacroMan.DrawCommandWindow(ref PvPBrdSettings.Instance.CommandWindowOpen);
    }
  }
}
