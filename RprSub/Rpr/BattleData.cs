namespace ElliotZ.Rpr;

public class BattleData {
  public static BattleData Instance = new();
  
  /// <summary>
  /// 用于记录gcd复唱时间
  /// </summary>
  public int GcdDuration{ get; set; } = 2500;
  public int NumBurstPhases{ get; set; } = 0;
  
  private static bool _isChanged;
  public int HoldCommunio { get; set; } = 0;

  public static void RebuildSettings() {
    if (!_isChanged) return;

    _isChanged = false;
    GlobalSetting.Build(RprRotationEntry.SettingsFolderPath, true);
    RprSettings.Build(RprRotationEntry.SettingsFolderPath);
  }
}
