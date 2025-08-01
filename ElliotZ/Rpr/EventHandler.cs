using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using System.Numerics;
using Task = System.Threading.Tasks.Task;

namespace ElliotZ.Rpr;

public class EventHandler : IRotationEventHandler
{
    private MobPullHelper? mobPullHelper;
    //private static long _lastCheckTime = 0L;
    private static bool _burstSettingsAltered = false;

    public void OnResetBattle()
    {
        // initializing MobPullHelper's static data and other data in BattleData
        BattleData.Instance = new BattleData();
        MobPullHelper._lastCheckTime = 0L;
        MobPullHelper._currentPosition = Vector3.Zero;
        MobPullHelper._lastPosition = Vector3.Zero;

        // initialize pull record
        if (AI.Instance.BattleData.CurrBattleTimeInMs >= 0)
        {
            if (Core.Resolve<MemApiDuty>().InMission && 
                Core.Resolve<MemApiDuty>().DutyMembersNumber() == 4 && 
                RprSettings.Instance.PullingNoBurst)
            {
                BattleData.Instance.IsPulling = true;
            }
            //else
            //{
            //    BattleData.Instance.IsPulling = false;
            //}

            //MeleePosHelper.Clear();
            if (RprSettings.Instance.RestoreQtSet) 
            {
                Qt.LoadQtStatesNoPot();
            }
        }
        BattleData.Instance.IsPulling = false;
        MeleePosHelper.Clear();
    }

    public async Task OnNoTarget()
    {
        // maybe add soulsow, idk
        if (BattleData.Instance.IsStopped == false) 
        { 
            BattleData.Instance.NoTarget = true; 
        }
        StopHelper.StopActions(BattleData.Instance.GcdDuration);

        await Task.CompletedTask;
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    public async Task OnPreCombat()
    {
        if (!Core.Resolve<MemApiDuty>().IsOver && Core.Resolve<MemApiDuty>().InMission)
        {
            if (Core.Resolve<MemApiDuty>().DutyMembersNumber() == 8 && RprSettings.Instance.NoBurst)
            {
                LogHelper.Print("检测到你在8人本开了“小怪低血量不交爆发”，为防止出错已自动为你关闭该设置。");
                RprSettings.Instance.NoBurst = false;
                _burstSettingsAltered = true;
            }
        }

        if (Core.Resolve<MemApiDuty>().IsOver && _burstSettingsAltered) 
        {
            LogHelper.Print("改变过“小怪低血量不交爆发”的设置，现在复原。");
            RprSettings.Instance.NoBurst = true;
            _burstSettingsAltered = false;
        }

        // out of combat soulsow
        if (SpellsDef.Soulsow.IsUnlock() && Qt.Instance.GetQt("播魂种") &&
                Core.Me.HasAura(AurasDef.Soulsow) == false &&
                !SpellsDef.HarvestMoon.GetSpell().RecentlyUsed(1500))
        {
            await SpellsDef.Soulsow.GetSpell().Cast();
        }

        StopHelper.StopActions(BattleData.Instance.GcdDuration);
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        //记录复唱时间
        var d = Core.Resolve<MemApiSpell>().GetGCDDuration(true);
        if (d > 0) BattleData.Instance.GcdDuration = d;

        //Single Weave Skills
        AI.Instance.BattleData.CurrGcdAbilityCount = (spell.Id is SpellsDef.VoidReaping 
                                                               or SpellsDef.CrossReaping) ? 1 : 2;

        BattleData.Instance.justCastAC = (spell.Id is SpellsDef.ArcaneCircle);
    }

    public void OnBattleUpdate(int currTime)
    {
        var gcdProgPctg = (int)((GCDHelper.GetGCDCooldown() / (double)BattleData.Instance.GcdDuration) * 100);
        var inTN = Core.Me.HasAura(AurasDef.TrueNorth) &&
                   !RprSettings.Instance.NoPosDrawInTN;
        var GibGallowsReady = Core.Me.HasAura(AurasDef.SoulReaver) || 
                              Core.Me.HasAura(AurasDef.Executioner);
        var GibGallowsJustUsed = 
                Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gibbet).RecentlyUsed(500) ||
                Core.Resolve<MemApiSpell>().CheckActionChange(SpellsDef.Gallows).RecentlyUsed(500);

        // detect if tank is pulling in dungeons
        mobPullHelper = new MobPullHelper();
        if (mobPullHelper.CurrTank is not null && RprSettings.Instance.PullingNoBurst)
        {
            if (AI.Instance.BattleData.CurrBattleTimeInMs - MobPullHelper._lastCheckTime >= 1000)
            {
                mobPullHelper.CheckTankPosition();
                MobPullHelper._lastCheckTime = AI.Instance.BattleData.CurrBattleTimeInMs;
            }

            if (mobPullHelper.IsPulling == false) 
            { 
                mobPullHelper.CheckEnemiesAroundTank();
                if (mobPullHelper.ConcentrationPctg > RprSettings.Instance.ConcentrationThreshold)
                {
                    BattleData.Instance.IsPulling = false;
                }
            }
        }
        // exclude bosses
        if (BattleData.Instance.IsPulling && Core.Me.GetCurrTarget().IsBoss())
        {
            BattleData.Instance.IsPulling = false;
        }

        // logic for holding bursts when mob pack is about to die
        BattleData.Instance.TotalHpPercentage = MobPullHelper.GetTotalHealthPercentageOfNearbyEnemies();
        BattleData.Instance.AverageTTK = MobPullHelper.GetAverageTTKOfNearbyEnemies();

        if (RprSettings.Instance.NoBurst && 
            !Core.Resolve<MemApiDuty>().InBossBattle &&  // exclude boss battles and msq ultima wep
            Core.Resolve<MemApiZoneInfo>().GetCurrTerrId() != 1048 &&
            AI.Instance.BattleData.CurrBattleTimeInMs > 10000 && 
                (BattleData.Instance.TotalHpPercentage < RprSettings.Instance.MinMobHpPercent || 
                 BattleData.Instance.AverageTTK < (RprSettings.Instance.minTTK * 1000)))
        {
            Qt.Instance.SetQt("魂衣", false);
            Qt.Instance.SetQt("神秘环", false);
        }

        // stop action during accel bombs, pyretics and/or when boss is invuln
        if (Helper.AnyAuraTimerLessThan(StopHelper.AccelBomb, 3000) && 
            PlayerOptions.Instance.Stop == false)
        {
            if (Core.Me.GetCurrTarget() is not null && 
                    Qt.Instance.GetQt("收获月") && 
                    Core.Me.HasAura(AurasDef.Soulsow))
            {
                if (AI.Instance.BattleData.NextSlot is null)
                {
                    AI.Instance.BattleData.NextSlot = new Slot();
                }
                AI.Instance.BattleData.NextSlot.Add(SpellsDef.HarvestMoon.GetSpell());
            }
        }

        StopHelper.StopActions(BattleData.Instance.GcdDuration, retarget: true);

        // positional indicator
        if (!inTN && 
                !Core.Me.HasAura(AurasDef.Enshrouded) && 
                Core.Me.GetCurrTarget() is not null && 
                Core.Me.GetCurrTarget().HasPositional())
        {
            if (GibGallowsReady && !GibGallowsJustUsed)
            {
                if (Core.Me.HasAura(AurasDef.EnhancedGallows))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Behind, gcdProgPctg);
                }
                else if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Flank, gcdProgPctg);
                }
                else
                {
                    MeleePosHelper.Clear();
                }
            }
            else if (Core.Resolve<JobApi_Reaper>().SoulGauge >= 50 || SpellsDef.SoulSlice.GetSpell().IsReadyWithCanCast())
            {
                if (Core.Me.HasAura(AurasDef.EnhancedGallows))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Behind, 70);
                }
                else if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Flank, 70);
                }
                else
                {
                    MeleePosHelper.Clear();
                }
            }
            else
            {
                MeleePosHelper.Clear();
            }
        }
        else
        {
            MeleePosHelper.Clear();
        }
    }

    public void OnEnterRotation() //切换到当前ACR
    {
        //LogHelper.Print(
        //    "欢迎使用yoyo舞者ACR，反馈请到：https://discord.com/channels/1191648233454313482/1326201786046087329");
        //Core.Resolve<MemApiChatMessage>()
        //    .Toast2("跟我念：傻逼riku和souma的妈死干净咯！", 1, 5000);


        //检查全局设置
        if (!Helper.GlblSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中勾选【全局能力技不卡GCD】选项");

        //MeleePosHelper2.Init(Qt.Instance, "真北");

        //更新时间轴
        //if (DncSettings.Instance.AutoUpdataTimeLines)
        //    TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);
    }

    public void OnExitRotation() //退出ACR
    {
    }

    public void OnTerritoryChanged()
    {
    }
}