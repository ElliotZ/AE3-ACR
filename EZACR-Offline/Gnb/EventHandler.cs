using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Target;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Statuses;
using ElliotZ.Common;
using ElliotZ.Rpr;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb;

public class EventHandler : IRotationEventHandler
{
    private long randomSongTime;

    public static bool TimerReset;

    public static bool IsStopped { get; set; }

    public async Task OnPreCombat()
    {
        if (GnbSettings.Instance.UsePotion && GnbSettings.Instance.ACRMode != "Normal")
        {
            Qt.Instance.NewDefault("自动拉怪", newDefault: false);
            Qt.Instance.SetQt("自动拉怪", qtValue: false);
        }

        StopHelper.StopActions(1000);
        await Task.CompletedTask;
    }

    //public bool HasAnyBuff(IBattleChara battleCharacter, List<uint> buffs, int msLeft)
    //{
    //    try
    //    {
    //        if (battleCharacter == null || buffs == null || buffs.Count == 0)
    //        {
    //            return false;
    //        }

    //        for (int i = 0; i < battleCharacter.StatusList.Length; i++)
    //        {
    //            Status status = battleCharacter.StatusList[i];
    //            if (status != null && status.StatusId != 0 && Math.Abs(status.RemainingTime) * 1000f < (float)msLeft && buffs.Contains(status.StatusId))
    //            {
    //                return true;
    //            }
    //        }

    //        return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("HasAnyBuff异常: " + ex.Message);
    //        return false;
    //    }
    //}

    public void OnResetBattle()
    {
        BattleData.Instance = new BattleData();
        TimerReset = false;
        if (GnbSettings.Instance.自动拉怪 && GnbSettings.Instance.ACRMode == "Normal")
        {
            Qt.Instance.NewDefault("自动拉怪", newDefault: true);
            Qt.Instance.SetQt("自动拉怪", qtValue: true);
        }

        Qt.Instance.NewDefault("倾泻爆发", newDefault: false);
        Qt.Instance.SetQt("倾泻爆发", qtValue: false);

        StopHelper.StopActions(1000);
    }

    public async Task OnNoTarget()
    {
        await Task.CompletedTask;
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
    }

    public void OnBattleUpdate(int currTimeInMs)
    {
        IBattleChara? currTarget = Core.Me.GetCurrTarget();
        if (currTarget != null && 
                currTarget.IsBoss() && 
                !Core.Me.GetCurrTarget().IsDummy() && 
                Qt.Instance.GetQt("自动拉怪"))
        {
            Qt.Instance.NewDefault("自动拉怪", newDefault: false);
            Qt.Instance.SetQt("自动拉怪", qtValue: false);
        }

        if (!TimerReset)
        {
            BattleData.Instance.lastFalseTime = DateTime.Now;
            TimerReset = true;
        }

        if (!Core.Me.IsMoving() && Qt.Instance.GetQt("自动拉怪"))
        {
            if ((DateTime.Now - BattleData.Instance.lastFalseTime).TotalSeconds >= (double)GnbSettings.Instance.自动拉怪停止时间)
            {
                LogHelper.Print("检测到停止移动" + GnbSettings.Instance.自动拉怪停止时间 + "秒，自动拉怪已关闭");
                BattleData.Instance.lastFalseTime = DateTime.Now;
                Qt.Instance.SetQt("自动拉怪", qtValue: false);
            }
        }
        else
        {
            BattleData.Instance.lastFalseTime = DateTime.Now;
        }

        // logic for holding bursts when mob pack is about to die
        BattleData.Instance.TotalHpPercentage = MobPullHelper.GetTotalHealthPercentageOfNearbyEnemies();
        BattleData.Instance.AverageTTK = MobPullHelper.GetAverageTTKOfNearbyEnemies();

        if (GnbSettings.Instance.NoBurst &&
            !Core.Resolve<MemApiDuty>().InBossBattle &&  // exclude boss battles and msq ultima wep
            !Core.Me.GetCurrTarget().IsDummy() &&
            Helper.GetTerritoyId != 1048 &&
            AI.Instance.BattleData.CurrBattleTimeInMs > 10000 &&
                (BattleData.Instance.TotalHpPercentage < GnbSettings.Instance.MinMobHpPercent ||
                 BattleData.Instance.AverageTTK < (GnbSettings.Instance.minTTK * 1000)))
        {
            Qt.Instance.SetQt("爆发", false);
            //Qt.Instance.SetQt("神秘环", false);
        }

        if (GnbSettings.Instance.HandleStopMechs) StopHelper.StopActions(1000);
        //if (Helper.AnyAuraTimerLessThan(Buff.停手Buff, 3000) && 
        //        !IsStopped && 
        //        !Map.高难地图.Contains(Core.Resolve<MemApiZoneInfo>().GetCurrTerrId()))
        //{
        //    IsStopped = true;
        //    Core.Me.SetTarget(Core.Me);
        //    LogHelper.Print("检测到需要停手的BUFF，已停止并设置目标为自己。");
        //}

        //if (!Core.Me.HasAnyAura(Buff.停手Buff, 3000) && 
        //        IsStopped && 
        //        TargetMgr.Instance.EnemysIn20.Count > 0 && 
        //        !Map.高难地图.Contains(Core.Resolve<MemApiZoneInfo>().GetCurrTerrId()))
        //{
        //    IsStopped = false;
        //    IBattleChara battleChara = TargetMgr.Instance.EnemysIn20.Values.FirstOrDefault();
        //    if (battleChara != null)
        //    {
        //        Core.Me.SetTarget(battleChara);
        //        LogHelper.Print("需要停手的BUFF已消失，已恢复并设置目标为最近的敌人。");
        //    }
        //    else
        //    {
        //        LogHelper.Print("没有找到最近的敌人。");
        //    }
        //}
    }

    public async void OnEnterRotation()
    {
        LogHelper.Print("KKxb绝枪8.5国际服版本(国服可用)TEST");
        LogHelper.Print("悬浮窗增加自动开盾姿及ST不自动关选项(奇怪的倒计时处理问题 正常不正常交替出现)  出现问题请带上设置去DC使用问题反馈@KKxb");
        LogHelper.Print("本ACR使用须知请查看悬浮窗");
        Core.Resolve<MemApiChatMessage>().Toast2("KKxb绝枪ACR 使用请确认悬浮窗设置说明与QT", 1, 4000);
    }

    public void OnExitRotation()
    {
    }

    public void OnTerritoryChanged()
    {
    }

}
