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

    public static readonly List<uint> Invulns = [ 352, 529, 656, 671, 
                                                    775, 776, 696, 981, 
                                                    1570, 1697, 1829, 328, 
                                                    2287, 3009, 3010, 3011, 
                                                    3012 ];

    public static readonly HashSet<string> WhiteList = [ "18014449513685488", 
                                                         "18014449511049086", 
                                                         "19014409515763009", 
                                                         "19014419509512110" ];

    public static void StopActions(int time)
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
                if (TargetMgr.Instance.EnemysIn20.Count > 0 && Core.Me.GetCurrTarget() == Core.Me)
                {
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
        if (Core.Me.GetCurrTarget() is not null && Core.Me.GetCurrTarget().HasAnyAura(Invulns)) 
        {
            return 2; 
        }
        return -1;
    }
}
