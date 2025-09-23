namespace EZACR_Offline.PvP;

public interface IPvPBattleData {
  public int HPDelta { get; set; }
  public int LastHp { get; set; }
  public int HPDeltaTime { get; set; }
  public int TotalHPDelta { get; set; }
}
