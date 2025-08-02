using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Statuses;
using Lumina.Excel.Sheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Common;

public static class StopHelper
{
    public const uint AccelerationBomb4144 = 4144u;
    public const uint AccelerationBomb3802 = 3802u;
    public const uint AccelerationBomb3793 = 3793u;
    public const uint AccelerationBomb1384 = 1384u;
    public const uint AccelerationBomb2657 = 2657u;
    public const uint AccelerationBomb1072 = 1072u;
    public const uint Pyretic960 = 960u;
    public const uint Pyretic639 = 639u;
    public const uint Pyretic3522 = 3522u;
    public const uint Pyretic1599 = 1599u;
    public const uint Pyretic1133 = 1133u;
    public const uint Pyretic1049 = 1049u;
    public const uint Invincibility325 = 325u;
    public const uint Invincibility529 = 529u;
    public const uint Invincibility656 = 656u;
    public const uint Invincibility671 = 671u;
    public const uint Invincibility775 = 775u;
    public const uint Invincibility776 = 776u;
    public const uint Invincibility969 = 969u;
    public const uint Invincibility981 = 981u;
    public const uint Invincibility1570 = 1570u;
    public const uint Invincibility1697 = 1697u;
    public const uint Invincibility1829 = 1829u;
    public const uint HeartoftheMountain = 328u;
    public const uint TrueHallowedGround = 2287u;
    public const uint MightoftheVortex = 3009u;
    public const uint MightofCrags = 3010u;
    public const uint MightoftheInferno = 3011u;
    public const uint VortexBarrier = 3012u;

    public const uint NoPos = 3808u;

    private static bool _manualOverride = false;
    private static bool _lastStopVal = false;

    public static readonly List<uint> AccelBomb = [4144, 3802, 3793, 1384, 2657, 1072];

    public static readonly List<uint> Pyretic = [960, 639, 3522, 1599, 1133, 1049];

    public static readonly List<uint> Invulns =
    [
        325, // 无敌：一切攻击都无法造成伤害
        529, // 无敌：一切攻击都无法造成伤害
        656, // 无敌：一切攻击都无法造成伤害
        671, // 无敌：一切攻击都无法造成伤害
        775, // 无敌：一切攻击都无法造成伤害
        776, // 无敌：一切攻击都无法造成伤害
        895, // 无敌：所有攻击均无效化
        969, // 无敌：一切攻击都无法造成伤害
        981, // 无敌：一切攻击都无法造成伤害
        1570, // 无敌：一切攻击都无法造成伤害
        1697, // 无敌：一切攻击都无法造成伤害
        1444,//魔导结界,塔结界在运转，一切攻击都无法造成伤害
        1829, // 无敌：一切攻击都无法造成伤害
        328, // 土神的心石
        2287, // 纯正神圣领域
        2670, // 冥界行
        3012, // 风神障壁
        3039, // 不死救赎
        3255, // 出死入生
        394, // 无敌：所有攻击均无效化
        //1125, // 特定方向无敌：令特定方向的攻击无效化
        1567, // 召唤兽的加护：受到了召唤兽的加护，处于暂时无敌的状态
    ];

    public static readonly HashSet<string> WhiteList = [ "18014449513685488", 
                                                         "18014449511049086", 
                                                         "19014409515763009", 
                                                         "19014419509512110" ];

    public static bool Debug = false;

    public static void StopActions(int time, bool retarget = false)
    {
        var check = StopCheck(time);
        if (check is 1 or 2)
        {
            PlayerOptions.Instance.Stop = true;
            _manualOverride = false;
            Core.Resolve<MemApiSpell>().CancelCast();

            if (check is 1) { Core.Me.SetTarget(Core.Me); }
        }
        else
        {
            if (PlayerOptions.Instance.Stop != _lastStopVal)
            {
                _manualOverride = true;
            }
            if (!_manualOverride) 
            {
                PlayerOptions.Instance.Stop = false; 
                if (retarget && TargetMgr.Instance.EnemysIn20.Count > 0 &&
                    //!TargetMgr.Instance.EnemysIn20.Values.First().HasAnyAura(Invulns) &&
                        //(Core.Me.GetCurrTarget() is null || 
                        Core.Me.GetCurrTarget() is not null &&
                        Core.Me.GetCurrTarget()!.GameObjectId == Core.Me.GameObjectId)//)
                {
                    if (Debug) LogHelper.Print("Setting Target");
                    Core.Me.SetTarget(TargetMgr.Instance.EnemysIn20.Values.First());
                }
            }
        }
        _lastStopVal = PlayerOptions.Instance.Stop;
    }

    /// <summary>
    /// checks for whether a stop is needed
    /// </summary>
    /// <param name="time">only relevant for accel bombs</param>
    /// <returns>positive values if stop is needed</returns>
    public static int StopCheck(int time)
    {
        if (Helper.AnyAuraTimerLessThan(AccelBomb, time)) { return 1; }
        if (Core.Me.HasAnyAura(Pyretic)) { return 1; }
        if (Core.Me.GetCurrTarget() is not null && 
                !(Core.Me.GetCurrTarget()!.GameObjectId == Core.Me.GameObjectId) && 
                Core.Me.GetCurrTarget().HasAnyAura(Invulns)) 
        {
            return 2; 
        }
        return -1;
    }
}
