using AEAssist;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr;

public static class RprHelper {
  public static int GetGcdDuration => BattleData.Instance.GcdDuration;

  public static uint PrevCombo => Core.Resolve<MemApiSpell>().GetLastComboSpellId();

  //public static int ComboTimer => (int)Core.Resolve<MemApiSpell>().GetComboTimeLeft().TotalMilliseconds;
  public static int Soul => Core.Resolve<JobApi_Reaper>().SoulGauge;
  public static int Shroud => Core.Resolve<JobApi_Reaper>().ShroudGauge;
  public static int BlueOrb => Core.Resolve<JobApi_Reaper>().LemureShroud;
  public static int PurpOrb => Core.Resolve<JobApi_Reaper>().VoidShroud;

  /// <summary>
  /// 自身buff剩余时间是否在x个gcd内
  /// </summary>
  /// <param name="buffId"></param>
  /// <param name="gcd">Number of GCDs</param>
  /// <returns></returns>
  public static bool AuraInGCDs(uint buffId, int gcd) {
    int timeLeft = Helper.GetAuraTimeLeft(buffId);
    if (timeLeft <= 0) return false;
    if (GetGcdDuration <= 0) return false;

    return timeLeft / GetGcdDuration < gcd;
  }

  public static int GcdsToSoulOvercap() {
    int res = (100 - Core.Resolve<JobApi_Reaper>().SoulGauge) / 10;

    if (Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign,
                                    BattleData.Instance.GcdDuration * (res + 3),
                                    false)
     || Helper.TgtAuraTimerLessThan(AurasDef.DeathsDesign,
                                    30000 + BattleData.Instance.GcdDuration * res,
                                    false)) {
      res++;
    }

    return res;
  }

  public static void CasualMode() {
    RprSettings.Instance.NoBurst = true;
    RprSettings.Instance.PullingNoBurst = true;
    RprSettings.Instance.AutoBloodBath = true;
    RprSettings.Instance.AutoCrest = true;
    RprSettings.Instance.AutoSecondWind = true;
    RprSettings.Instance.AutoFeint = true;
    RprSettings.Instance.HandleStopMechs = true;
    Qt.Instance.NewDefault("起手", true);
    Qt.Instance.SetQt("起手", true);
    Qt.Instance.NewDefault("单魂衣", false);
    Qt.Instance.SetQt("单魂衣", false);
    Qt.Instance.NewDefault("神秘环", true);
    Qt.Instance.SetQt("神秘环", true);
    Qt.Instance.NewDefault("大丰收", true);
    Qt.Instance.SetQt("大丰收", true);
    Qt.Instance.NewDefault("灵魂割", true);
    Qt.Instance.SetQt("灵魂割", true);
    Qt.Instance.NewDefault("挥割/爪", true);
    Qt.Instance.SetQt("挥割/爪", true);
    Qt.Instance.NewDefault("暴食", true);
    Qt.Instance.SetQt("暴食", true);
    Qt.Instance.NewDefault("魂衣", true);
    Qt.Instance.SetQt("魂衣", true);
    Qt.Instance.NewDefault("完人", true);
    Qt.Instance.SetQt("完人", true);
    Qt.Instance.NewDefault("真北", true);
    Qt.Instance.SetQt("真北", true);
    Qt.Instance.NewDefault("收获月", true);
    Qt.Instance.SetQt("收获月", true);
    Qt.Instance.NewDefault("勾刃", true);
    Qt.Instance.SetQt("勾刃", true);
    Qt.Instance.NewDefault("AOE", true);
    Qt.Instance.SetQt("AOE", true);
    Qt.Instance.NewDefault("播魂种", true);
    Qt.Instance.SetQt("播魂种", true);
    Qt.Instance.NewDefault("祭牲", true);
    Qt.Instance.SetQt("祭牲", true);
    Qt.Instance.NewDefault("倾泻资源", false);
    Qt.Instance.SetQt("倾泻资源", false);
    Qt.Instance.NewDefault("真北优化", true);
    Qt.Instance.SetQt("真北优化", true);
    Qt.Instance.NewDefault("智能AOE", true);
    Qt.Instance.SetQt("智能AOE", true);
    Qt.Instance.NewDefault("自动突进", false);
    Qt.Instance.SetQt("自动突进", false);
  }

  public static void HardCoreMode() {
    RprSettings.Instance.NoBurst = false;
    RprSettings.Instance.PullingNoBurst = false;
    RprSettings.Instance.AutoBloodBath = false;
    RprSettings.Instance.AutoCrest = false;
    RprSettings.Instance.AutoSecondWind = false;
    RprSettings.Instance.AutoFeint = false;
    RprSettings.Instance.HandleStopMechs = false;
    Qt.Instance.NewDefault("起手", true);
    Qt.Instance.SetQt("起手", true);
    Qt.Instance.NewDefault("单魂衣", false);
    Qt.Instance.SetQt("单魂衣", false);
    Qt.Instance.NewDefault("神秘环", true);
    Qt.Instance.SetQt("神秘环", true);
    Qt.Instance.NewDefault("大丰收", true);
    Qt.Instance.SetQt("大丰收", true);
    Qt.Instance.NewDefault("灵魂割", true);
    Qt.Instance.SetQt("灵魂割", true);
    Qt.Instance.NewDefault("挥割/爪", true);
    Qt.Instance.SetQt("挥割/爪", true);
    Qt.Instance.NewDefault("暴食", true);
    Qt.Instance.SetQt("暴食", true);
    Qt.Instance.NewDefault("魂衣", true);
    Qt.Instance.SetQt("魂衣", true);
    Qt.Instance.NewDefault("完人", true);
    Qt.Instance.SetQt("完人", true);
    Qt.Instance.NewDefault("真北", true);
    Qt.Instance.SetQt("真北", true);
    Qt.Instance.NewDefault("收获月", true);
    Qt.Instance.SetQt("收获月", true);
    Qt.Instance.NewDefault("勾刃", true);
    Qt.Instance.SetQt("勾刃", true);
    Qt.Instance.NewDefault("AOE", false);
    Qt.Instance.SetQt("AOE", false);
    Qt.Instance.NewDefault("播魂种", true);
    Qt.Instance.SetQt("播魂种", true);
    Qt.Instance.NewDefault("祭牲", true);
    Qt.Instance.SetQt("祭牲", true);
    Qt.Instance.NewDefault("倾泻资源", false);
    Qt.Instance.SetQt("倾泻资源", false);
    Qt.Instance.NewDefault("真北优化", true);
    Qt.Instance.SetQt("真北优化", true);
    Qt.Instance.NewDefault("智能AOE", false);
    Qt.Instance.SetQt("智能AOE", false);
    Qt.Instance.NewDefault("自动突进", false);
    Qt.Instance.SetQt("自动突进", false);
  }
}
