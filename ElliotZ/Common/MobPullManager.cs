using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Utility;
using System.Numerics;

namespace ElliotZ.Common;

public class MobPullManager(JobViewWindow qtInstance, string holdQT = "")
{
    private readonly JobViewWindow instance = qtInstance;
    private IBattleChara? CurrTank = null;

    private Vector3 _currentPosition = Vector3.Zero;
    private Vector3 _lastPosition = Vector3.Zero;
    private long _lastCheckTime = 0L;
    private bool TankMoving = false;

    private string _holdQtName = holdQT;
    /// <summary>
    /// 主控QT的名字，默认空
    /// </summary>
    public string HoldQtName { get => _holdQtName; set => _holdQtName = value; }

    private bool _holding = false;
    /// <summary>
    /// 如果不使用主控QT可以通过Holding判断是否施放技能
    /// </summary>
    public bool Holding 
    { 
        get
        { 
            if (HoldQtName.IsNullOrEmpty()) 
                return _holding; 
            else 
                return instance.GetQt(HoldQtName); 
        }
        set 
        {
            if (HoldQtName.IsNullOrEmpty())
                _holding = value;
            else
                instance.SetQt(HoldQtName, !value);
        } 
    }

    /// <summary>
    /// 需要自行添加需要控制的QT
    /// </summary>
    public readonly List<string> BurstQTs = [];

    /// <summary>
    /// 当前场景是否适用留爆发功能，以及是否存在Tank的判定
    /// </summary>
    /// <returns></returns>
    public bool CheckTank()
    {
        if (Core.Resolve<MemApiDuty>().InMission && 
            Core.Resolve<MemApiDuty>().DutyMembersNumber() == 4)
        {
            CurrTank = PartyHelper.CastableTanks.FirstOrDefault();
        }
        else CurrTank = null;

        return CurrTank is not null;
    }

    public static uint GetTerritoyId => Core.Resolve<MemApiMap>().GetCurrTerrId();

    /// <summary>
    /// 重置状态，需要在OnPreCombat和OnResetBattle中调用
    /// </summary>
    public void 重置() => Reset();
    public void Reset()
    {
        _lastCheckTime = 0L;
        _currentPosition = Vector3.Zero;
        _lastPosition = Vector3.Zero;
        if (CheckTank()) Holding = true;
        else Holding = false;
    }

    /// <summary>
    /// 在坦克拉怪过程中留爆发的控制逻辑
    /// </summary>
    /// <param name="currTime">当前战斗持续时间，可以直接把OnBattleUpdate的currTime参数填入</param>
    /// <param name="ConcentrationThreshold">设定的小怪集中度</param>
    public void 拉怪中留爆发(int 当前时间, float 集中度) => HoldBurstIfPulling(当前时间, 集中度);
    public void HoldBurstIfPulling(int currTime, float ConcentrationThreshold)
    {
        if (CurrTank is not null)
        {
            if (currTime - _lastCheckTime >= 1000)
            {
                CheckTankPosition();
                _lastCheckTime = currTime;
            }

            if (TankMoving == false)
            {
                if (CheckEnemiesAroundTank() > ConcentrationThreshold)
                {
                    SetAllQTs(true);  // also sets HoldingQT to false if possible
                }
            }
            if (Holding && Core.Me.GetCurrTarget() is not null && Core.Me.GetCurrTarget()!.IsBoss())
            {
                SetAllQTs(true);
            }
        }
    }

    /// <summary>
    /// 在一波小怪接近死亡时留爆发的控制逻辑
    /// </summary>
    /// <param name="currTime">当前战斗持续时间，可以直接把OnBattleUpdate的currTime参数填入</param>
    /// <param name="mobHPThreshold">设定的小怪血量阈值</param>
    /// <param name="minTTK">设定的小怪平均死亡时间阈值，用ms计算</param>
    public void 小怪死亡留爆发(int 当前时间, float 血量阈值, int 死亡时间阈值) => HoldBurstIfMobsDying(当前时间, 血量阈值, 死亡时间阈值);
    public void HoldBurstIfMobsDying(int currTime, float mobHPThreshold, int minTTK)
    {
        // exclude boss battles, msq ultima wep, and 8 man duties in general
        if (Core.Resolve<MemApiDuty>().InMission &&
               Core.Resolve<MemApiDuty>().DutyMembersNumber() != 8 &&
               !Core.Resolve<MemApiDuty>().InBossBattle &&  
               //!Core.Me.GetCurrTarget().IsDummy() &&
               GetTerritoyId != 1048 &&
               currTime > 10000 &&
               (GetTotalHealthPercentageOfNearbyEnemies() < mobHPThreshold ||
                GetAverageTTKOfNearbyEnemies() < minTTK))
        {
            SetAllQTs(false);
        }
    }

    /// <summary>
    /// 设置所有BurstQTs里面记录的QT。如果设置了总控holdQT则只会设置总控QT。
    /// </summary>
    /// <param name="val"></param>
    private void SetAllQTs(bool val)
    {
        if (HoldQtName.IsNullOrEmpty())
        {
            foreach (var item in BurstQTs) { instance.SetQt(item, val); }
        }
        Holding = !val;
    }

    /// <summary>
    /// 判断Tank周围的敌人密度是否大于设定阈值
    /// </summary>
    private float CheckEnemiesAroundTank()
    {
        int VisibleEnemiesIn25 = 0;
        int VisibleEnemiesIn5 = 0;
        if (CurrTank == null)
        {
            return 0f;
        }

        Dictionary<uint, IBattleChara> enemys = TargetMgr.Instance.Enemys;
        foreach (KeyValuePair<uint, IBattleChara> item in enemys)
        {
            if (CurrTank.Distance(item.Value) <= 25f)
            {
                VisibleEnemiesIn25++;
            }

            if (CurrTank.Distance(item.Value) <= 5f)
            {
                VisibleEnemiesIn5++;
            }
        }

        return ((VisibleEnemiesIn25 > 0) ?
                             (VisibleEnemiesIn5 / (float)VisibleEnemiesIn25) : 0f);
    }

    /// <summary>
    /// 维护TankMoving状态，如果1秒内队伍tank位移超过1.5米则TankMoving为True
    /// </summary>
    private void CheckTankPosition()
    {
        if (CurrTank != null)
        {
            _currentPosition = CurrTank.Position;
            float num = Vector3.Distance(_currentPosition, _lastPosition);
            TankMoving = !(num < 1.5f);
            _lastPosition = _currentPosition;
        }
    }

    /// <summary>
    /// 求25米内敌人的总HP百分比
    /// </summary>
    /// <returns></returns>
    public static float 附近敌人总血量比例() => GetTotalHealthPercentageOfNearbyEnemies();
    public static float GetTotalHealthPercentageOfNearbyEnemies()
    {
        Dictionary<uint, IBattleChara> enemysIn = TargetMgr.Instance.EnemysIn25;
        float totalMobCurrHp = 0f;
        float totalMobMaxHp = 0f;
        int MobCount = 0;
        foreach (KeyValuePair<uint, IBattleChara> item in enemysIn)
        {
            if (item.Value is not null && !item.Value.IsBoss())
            {
                totalMobCurrHp += item.Value.CurrentHp;
                totalMobMaxHp += item.Value.MaxHp;
                MobCount++;
            }
        }

        if (MobCount == 0)
        {
            return 0f;
        }

        return totalMobCurrHp / totalMobMaxHp;
    }

    /// <summary>
    /// 求25米内敌人的平均死亡时间
    /// </summary>
    /// <returns></returns>
    public static float 附近敌人平均死亡时间() => GetAverageTTKOfNearbyEnemies();
    public static float GetAverageTTKOfNearbyEnemies()
    {
        Dictionary<uint, IBattleChara> enemysIn = TargetMgr.Instance.EnemysIn25;
        List<float> TTKList = [];
        int MobCount = 0;
        // 遍历25米内敌人，根据敌人的EntityID把所有大于0的DeathPrediction加起来，跳过boss
        foreach (KeyValuePair<uint, IBattleChara> item in enemysIn)
        {
            IBattleChara value = item.Value;
            if (!TargetHelper.IsBoss(value) && 
                TargetMgr.Instance.TargetStats.TryGetValue(value.EntityId, out var value2) && 
                value2.DeathPrediction > 0)
            {
                TTKList.Add(value2.DeathPrediction);
                MobCount++;
            }
        }

        if (MobCount == 0)
        {
            return 0f;
        }

        // 如果TTKList总数大于5则掐头去尾取平均
        if (TTKList.Count > 4)
        {
            TTKList.Sort();
            TTKList.RemoveAt(0);
            TTKList.RemoveAt(TTKList.Count - 1);
        }

        return TTKList.Average();
    }
}
