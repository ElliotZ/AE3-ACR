using AEAssist;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Gui.Toast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Common;

public class MobPullHelper
{
    public readonly IBattleChara? CurrTank = CheckTank();
    private int VisibleEnemiesIn25 = 0;
    private int VisibleEnemiesIn5 = 0;
    public float ConcentrationPctg = 0f;
    public static Vector3 _currentPosition = Vector3.Zero;
    public static Vector3 _lastPosition = Vector3.Zero;
    public static long _lastCheckTime = 0L;
    public bool IsPulling = false;

    public static IBattleChara? CheckTank()
    {
        if (Core.Resolve<MemApiDuty>().DutyMembersNumber() == 4)
        {
            return PartyHelper.CastableTanks.FirstOrDefault();
        }
        return null;
    }

    public void CheckEnemiesAroundTank()
    {
        if (CurrTank == null)
        {
            return;
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

        ConcentrationPctg = ((VisibleEnemiesIn25 > 0) ? 
                             (VisibleEnemiesIn5 / (float)VisibleEnemiesIn25) : 0f);
    }

    public void CheckTankPosition()
    {
        if (CurrTank != null)
        {
            _currentPosition = CurrTank.Position;
            float num = Vector3.Distance(_currentPosition, _lastPosition);
            IsPulling = !(num < 1.5f);
            _lastPosition = _currentPosition;
        }
    }

    public static float GetTotalHealthPercentageOfNearbyEnemies()
    {
        Dictionary<uint, IBattleChara> enemysIn = TargetMgr.Instance.EnemysIn25;
        float totalMobCurrHp = 0f;
        float totalMobMaxHp = 0f;
        int MobCount = 0;
        foreach (KeyValuePair<uint, IBattleChara> item in enemysIn)
        {
            //IBattleChara value = item.Value;
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

    public static float GetAverageTTKOfNearbyEnemies()
    {
        Dictionary<uint, IBattleChara> enemysIn = TargetMgr.Instance.EnemysIn25;
        List<float> TTKList = new List<float>();
        int MobCount = 0;
        foreach (KeyValuePair<uint, IBattleChara> item in enemysIn)
        {
            IBattleChara value = item.Value;
            if (!TargetHelper.IsBoss(value) && TargetMgr.Instance.TargetStats.TryGetValue(value.EntityId, out var value2) && value2.DeathPrediction > 0)
            {
                TTKList.Add(value2.DeathPrediction);
                MobCount++;
            }
        }

        if (MobCount == 0)
        {
            return 0f;
        }

        if (TTKList.Count > 4)
        {
            TTKList.Sort();
            TTKList.RemoveAt(0);
            TTKList.RemoveAt(TTKList.Count - 1);
        }

        return TTKList.Average();
    }
}
