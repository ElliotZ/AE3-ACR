using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using EZACR_Offline.PvP.Brd.QtUI;
using EZACR_Offline.PvP.Brd.SlotResolvers;
using JobViewWindow = ElliotZ.ModernJobViewFramework.JobViewWindow;

namespace EZACR_Offline.PvP.Brd;

public class RotationEntry : IRotationEntry {
  private PvPBrdSettingUI settingUI = new();
  //public static JobViewWindow QtInstance = null!;
  private PvPBrdOverlay _lazyOverlay = new();
  public List<SlotResolverData> SlotResolvers = new() {
      new SlotResolverData(new 净化(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 药(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 光阴神(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 速度之星(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 勇气(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 九天连箭(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 英雄的返场余音(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 默者的夜曲(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 爆破箭(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 绝峰箭(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 完美音调(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 强劲射击(), SlotMode.Always),
      new SlotResolverData((ISlotResolver)new 冲刺(), SlotMode.Always),
  };

  public void Dispose() { }

  public IRotationUI GetRotationUI() {
    return Qt.Instance;
  }

  public void OnDrawSetting() {
    settingUI.Draw();
  }

  public string OverlayTitle { get; } = "巴德";

  public void DrawOverlay() { }

  public string AuthorName { get; set; } = "Linto PvP";

  public Rotation Build(string settingFolder) {
    PvPBrdSettings.Build(settingFolder);
    PvPSettings.Build(settingFolder);
    //BuildQt();
    Qt.Build();
    var rotation = new Rotation(SlotResolvers) {
        TargetJob = Jobs.Bard,
        AcrType = AcrType.PVP,
        MinLevel = 1,
        MaxLevel = 100,
        Description = "[1级码及以上使用]不定时更新,有问题DC频道反馈\n[7.1适配]",
    };
    rotation.SetRotationEventHandler(new EventHandler());
    //rotation.AddOpener(new Func<uint, IOpener>(this.GetOpener));
    return rotation;
  }

/*
  public void BuildQt()
  {
    QtInstance = new JobViewWindow(PvPBrdSettings.Instance.JobViewSave,
                                                    PvPBrdSettings.Instance.Save,
                                                    OverlayTitle);
    QtInstance.AddTab("职业配置", _lazyOverlay.DrawGeneral);
    QtInstance.AddTab("监控", PVPHelper.监控);
    QtInstance.AddTab("共通配置", PVPHelper.配置);

    QtInstance.AddHotkey("疾跑", new HotKeyResolver_NormalSpell(29057U, SpellTargetType.Self));
    QtInstance.AddHotkey("龟壳", new HotKeyResolver_NormalSpell(29054U, SpellTargetType.Self));
    QtInstance.AddHotkey("热水", new HotKeyResolver_NormalSpell(29711U, SpellTargetType.Self));
    QtInstance.AddHotkey("LB", new HotkeyData.诗人LB());
    QtInstance.AddHotkey("后跳", new HotkeyData.后射());
  }
*/

  //private IOpener? GetOpener(uint level) => null;
}
