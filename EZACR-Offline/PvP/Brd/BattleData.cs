namespace EZACR_Offline.PvP.Brd;

public class BattleData {
  public static BattleData Instance = new();

  public static void Reset() {
    Instance = new BattleData();
  }
}
