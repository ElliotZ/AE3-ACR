using System.Numerics;

namespace ElliotZ.Rpr;

public class BattleData
{
    public static BattleData Instance = new();

    /// <summary>
    /// 用于记录gcd复唱时间
    /// </summary>
    public int GcdDuration = 2500;

    public int numBurstPhases = 0;

    /// <summary>
    /// 上一个技能是否为神秘环
    /// </summary>
    public bool justCastAC = false;

    public float TotalHpPercentage = 0f;
    public float AverageTTK = 0f;
    //public bool IsStopped = false;
    //public bool NoTarget = false;
    //public bool IsInvuln = false;
    //public int VisibleEnemiesIn25 = 0;
    //public int VisibleEnemiesIn5 = 0;
    public bool IsPulling = false;
    //public Vector3 currPos = Vector3.Zero;
    //public Vector3 lastPos = Vector3.Zero;
}
