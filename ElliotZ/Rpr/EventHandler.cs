using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;
using System.Numerics;
using System.Reflection;
using Task = System.Threading.Tasks.Task;

namespace ElliotZ.Rpr;

public class EventHandler : IRotationEventHandler
{
    private MobPullHelper? mobPullHelper;
    //private static long _lastCheckTime = 0L;
    private static bool _burstSettingsAltered = false;
    private Dictionary<string, string> _qtKeyDict;
    private Dictionary<string, IHotkeyResolver> _hotkeyDict;
    private static readonly List<string> QtToastBuffer = new List<string>();
    private static bool _qtToastScheduled;

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
            else
            {
                BattleData.Instance.IsPulling = false;
            }

            //MeleePosHelper.Clear();
            if (RprSettings.Instance.RestoreQtSet) 
            {
                Qt.LoadQtStatesNoPot();
            }
        }
        //BattleData.Instance.IsPulling = false;
        MeleePosHelper.Clear();
    }

    public async Task OnNoTarget()
    {
        // maybe add soulsow, idk
        if (BattleData.Instance.IsStopped == false) 
        { 
            BattleData.Instance.NoTarget = true; 
        }
        StopHelper.StopActions(1000);

        if (RprSettings.Instance.Debug) LogHelper.Print("no target");

        await Task.CompletedTask;
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    public async Task OnPreCombat()
    {
        // out of combat soulsow
        if (SpellsDef.Soulsow.GetSpell().IsReadyWithCanCast() && Qt.Instance.GetQt("播魂种"))
        {
            await SpellsDef.Soulsow.GetSpell().Cast();
        }

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

        StopHelper.StopActions(1000);
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
        //stop casting soulsow if just entered combat
        if (currTime < 3000 && Core.Me.CastActionId == SpellsDef.Soulsow)
        {
            Core.Resolve<MemApiSpell>().CancelCast();
        }

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
            !Core.Me.GetCurrTarget().IsDummy() &&
            Helper.GetTerritoyId != 1048 &&
            AI.Instance.BattleData.CurrBattleTimeInMs > 10000 && 
                (BattleData.Instance.TotalHpPercentage < RprSettings.Instance.MinMobHpPercent || 
                 BattleData.Instance.AverageTTK < (RprSettings.Instance.minTTK * 1000)))
        {
            Qt.Instance.SetQt("魂衣", false);
            Qt.Instance.SetQt("神秘环", false);
        }

        //if (Helper.AnyAuraTimerLessThan(StopHelper.AccelBomb, 3200) && 
        //    //Core.Me.HasAnyAura(StopHelper.AccelBomb, BattleData.Instance.GcdDuration) &&
        //    PlayerOptions.Instance.Stop == false)
        //{
        //    if (Core.Me.GetCurrTarget() is not null && 
        //            Qt.Instance.GetQt("收获月") && 
        //            SpellsDef.HarvestMoon.GetSpell().IsReadyWithCanCast())
        //    {
        //        if (AI.Instance.BattleData.NextSlot is null)
        //        {
        //            AI.Instance.BattleData.NextSlot = new Slot();
        //        }
        //        AI.Instance.BattleData.NextSlot.Add(SpellsDef.HarvestMoon.GetSpell());
        //    }
        //}

        // stop action during accel bombs, pyretics and/or when boss is invuln
        StopHelper.StopActions(1000);

        // positional indicator
        var gcdProgPctg = (int)((GCDHelper.GetGCDCooldown() / (double)BattleData.Instance.GcdDuration) * 100);
        var inTN = Core.Me.HasAura(AurasDef.TrueNorth) &&
                   !RprSettings.Instance.NoPosDrawInTN;
        var GibGallowsReady = Core.Me.HasAura(AurasDef.SoulReaver) ||
                              Core.Me.HasAura(AurasDef.Executioner);
        var GibGallowsJustUsed =
                Helper.GetActionChange(SpellsDef.Gibbet).RecentlyUsed(500) ||
                Helper.GetActionChange(SpellsDef.Gallows).RecentlyUsed(500);
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
                else { MeleePosHelper.Clear(); }
            }
            else if (Core.Resolve<JobApi_Reaper>().SoulGauge >= 50 || 
                        SpellsDef.SoulSlice.GetSpell().IsReadyWithCanCast())
            {
                if (Core.Me.HasAura(AurasDef.EnhancedGallows))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Behind, 70);
                }
                else if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Flank, 70);
                }
                else { MeleePosHelper.Clear(); }
            }
            else { MeleePosHelper.Clear(); }
        }
        else { MeleePosHelper.Clear(); }
    }

    public void OnEnterRotation() //切换到当前ACR
    {
        //LogHelper.Print(
        //    "欢迎使用yoyo舞者ACR，反馈请到：https://discord.com/channels/1191648233454313482/1326201786046087329");
        Helper.SendTips("欢迎使用EZRpr，使用前请把左上角悬浮窗拉大查看README。");
        LogHelper.Print("如有问题和反馈可以在DC找我。");

        //检查全局设置
        if (Helper.GlblSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中取消勾选【全局能力技不卡GCD】选项");

        try
        {
            ECHelper.Commands.RemoveHandler(RprHelper.TxtCmdHandle);
        }
        catch (Exception) { }

        ECHelper.Commands.AddHandler(RprHelper.TxtCmdHandle, new Dalamud.Game.Command.CommandInfo(RprCommandHandler));
        _qtKeyDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach ((string name, string enName, bool defVal, string tooltip) in Qt.QtKeys) 
        { 
            _qtKeyDict.TryAdd(name, name);
            _qtKeyDict.TryAdd(enName.ToLower(), name);
        }
        _hotkeyDict = new Dictionary<string, IHotkeyResolver>(StringComparer.OrdinalIgnoreCase);
        foreach ((string name, string enName, IHotkeyResolver hkr) in Qt.HKResolvers)
        {
            _hotkeyDict.TryAdd(enName.ToLower(), hkr);
            _hotkeyDict.TryAdd(name, hkr);
        }
    }

    private void RprCommandHandler(string command, string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            LogHelper.PrintError(RprHelper.TxtCmdHandle[1..] + " 命令无效，请提供参数");
            return;
        }

        string processed = args.Trim().ToLower();
        if (processed.EndsWith("_qt"))
        {
            if (_qtKeyDict.ContainsKey(processed[..^3]))
            {
                ToggleQtSetting(_qtKeyDict.GetValueOrDefault(processed[..^3]));
            }
            else
            {
                 LogHelper.PrintError("未知QT参数：" +  args);
            }
            return;
        }

        if (processed.EndsWith("_hk"))
        {
            if (_hotkeyDict.ContainsKey(processed[..^3]))
            {
                ExecuteHotkey(_hotkeyDict.GetValueOrDefault(processed[..^3]));
            }
            else
            {
                 LogHelper.PrintError("未知Hotkey参数：" + args);
            }
            return;
        }

        if (processed == "hello")
        {
            LogHelper.Print("Hello World!");
        }
        else
        {
            LogHelper.PrintError("未知参数：" + args);
        }
    }

    private static void ExecuteHotkey(IHotkeyResolver? hkr)
    {
        if (hkr is null)
        {
            LogHelper.PrintError("HotkeyResolver未初始化");
        }
        else if (hkr.Check() >= 0)
        {
            hkr.Run();
        }
        else
        {
            LogHelper.Print("无法执行Hotkey，可能条件不满足或技能不可用。");
        }
    }

    private static void ToggleQtSetting(string? qtName)
    {
        if (!string.IsNullOrEmpty(qtName))
        {
            if (Qt.Instance.ReverseQt(qtName))
            {
                var SuccessNote = $"QT\"{qtName}\"已设置为 {Qt.Instance.GetQt(qtName)}。";
                LogHelper.Print(SuccessNote);
                if (RprSettings.Instance.ShowToast)
                {
                    QtToastBuffer.Add(SuccessNote);
                    if (!_qtToastScheduled)
                    {
                        _qtToastScheduled = true;
                        Task.Delay(50).ContinueWith(delegate 
                        {
                            string msg = string.Join("\n", QtToastBuffer);
                            Helper.SendTips(msg, 1, 1000);
                            QtToastBuffer.Clear();
                            _qtToastScheduled = false;
                        });
                    }
                }
            }
            else
            {
                LogHelper.PrintError("Failed to Toggle QT");
            }
        }
        else
        {
            LogHelper.PrintError("Empty QT name");
        }
    }

    public void OnExitRotation() //退出ACR
    {
        ECHelper.Commands.RemoveHandler(RprHelper.TxtCmdHandle);
    }

    public void OnTerritoryChanged()
    {
    }
}