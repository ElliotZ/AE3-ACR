using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using EZACR_Offline.PvP.Brd.QtUI;
using EZACR_Offline.PvP.Brd.SlotResolvers;

namespace EZACR_Offline.PvP.Brd;

public class RotationEntry : IRotationEntry {
  private readonly PvPBrdSettingUI _settingUI = new();
  
  public List<SlotResolverData> SlotResolvers = new() {
      new SlotResolverData(new 龟壳(), SlotMode.Always),
      new SlotResolverData(new 净化(), SlotMode.Always),
      new SlotResolverData(new 药(), SlotMode.Always),
      new SlotResolverData(new 锐眼(), SlotMode.Always),
      new SlotResolverData(new 光阴神(), SlotMode.Always),
      new SlotResolverData(new 速度之星(), SlotMode.Always),
      new SlotResolverData(new 勇气(), SlotMode.Always),
      new SlotResolverData(new 和弦箭(), SlotMode.Always),
      new SlotResolverData(new 英雄的返场余音(), SlotMode.Always),
      new SlotResolverData(new 默者的夜曲(), SlotMode.Always),
      new SlotResolverData(new 爆破箭(), SlotMode.Always),
      new SlotResolverData(new 绝峰箭(), SlotMode.Always),
      new SlotResolverData(new 完美音调(), SlotMode.Always),
      new SlotResolverData(new 强劲射击(), SlotMode.Always),
      new SlotResolverData(new 冲刺(), SlotMode.Always),
  };

  public void Dispose() { }

  public IRotationUI GetRotationUI() {
    return Qt.Instance;
  }

  public void OnDrawSetting() {
    _settingUI.Draw();
  }

  public string OverlayTitle { get; } = "巴德";

  public void DrawOverlay() { }

  public string AuthorName { get; set; } = "Linto PvP EZmix";

  public Rotation Build(string settingFolder) {
    PvPBrdSettings.Build(settingFolder);
    PvPSettings.Build(settingFolder);
    Qt.Build();
    var rotation = new Rotation(SlotResolvers) {
        TargetJob = Jobs.Bard,
        AcrType = AcrType.PVP,
        MinLevel = 1,
        MaxLevel = 100,
        Description = "[1级码及以上使用]不定时更新,有问题DC频道反馈\n[7.1适配]",
    };
    rotation.SetRotationEventHandler(new EventHandler());
    return rotation;
  }
}
