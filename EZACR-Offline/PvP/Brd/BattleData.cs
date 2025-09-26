using AEAssist;

namespace EZACR_Offline.PvP.Brd;

public class BattleData : IPvPBattleData {
  public static BattleData Instance = new();

  public int HPDelta { get; set; } = 0;
  public int TotalHPDelta { get; set; } = 0;
  public int LastHp { get; set; } = (int)Core.Me.CurrentHp;
  public int HPDeltaTime { get; set; } = 0;

  public static void Reset() {
    Instance = new BattleData();
  }

  public override string ToString() {
    return $"BattleData: HPDelta={HPDelta} TotalHPDelta={TotalHPDelta} LastHp={LastHp} HpDeltaTime={HPDeltaTime}";
  }
}
