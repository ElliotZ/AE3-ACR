using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.JobApi;

namespace EZACR_Offline.Gnb.Triggers;

public class TriggerAction_AmmoGTE : ITriggerCond, ITriggerBase {
  [LabelName("子弹数量大于等于")] public int Red { get; set; }

  public string DisplayName { get; } = "GNB/检测量谱-子弹";

  public string Remark { get; set; }

  public bool Draw() {
    return false;
  }

  public static int 绝枪量谱_子弹数() {
    return Core.Resolve<JobApi_GunBreaker>().Ammo;
  }

  public bool Handle(ITriggerCondParams triggerCondParams) {
    return 绝枪量谱_子弹数() >= Red;
  }
}
