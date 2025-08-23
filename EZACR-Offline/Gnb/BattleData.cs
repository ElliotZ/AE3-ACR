namespace EZACR_Offline.Gnb;

public class BattleData
{
    public DateTime LastFalseTime = default(DateTime);
    public float TotalHpPercentage = 0f;
    public float AverageTTK = 0f;

    public static BattleData Instance = new BattleData();
}
