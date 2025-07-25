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
using Task = System.Threading.Tasks.Task;

namespace ElliotZ.Rpr;

public class EventHandler : IRotationEventHandler
{
    //private static int 舞步步骤 => Core.Resolve<JobApi_Dancer>().CompleteSteps;
    //private static bool 大舞ing => Core.Me.HasAura(Data.Buffs.正在大舞);
    //private static bool 小舞ing => Core.Me.HasAura(Data.Buffs.正在小舞);

    /// <summary>
    /// 重置战斗
    /// </summary>
    public void OnResetBattle()
    {
        BattleData.Instance = new BattleData();
    }

    /// <summary>
    /// 进战且无目标时
    /// </summary>
    /// <returns></returns>
    public async Task OnNoTarget()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 施法成功判定可以滑步时
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="spell"></param>
    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    /// <summary>
    /// 脱战时
    /// </summary>
    /// <returns></returns>
    public async Task OnPreCombat()
    {
        if (SpellsDef.Soulsow.IsUnlock() && Core.Me.HasAura(AurasDef.Soulsow) == false)
        {
            await SpellsDef.Soulsow.GetSpell().Cast();
        }
    }

    /// <summary>
    /// 某个技能使用之后的处理,比如诗人在刷Dot之后记录这次是否是强化buff的Dot 
    /// 如果是读条技能，则是服务器判定它释放成功的时间点，比上面的要慢一点
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="spell"></param>
    public void AfterSpell(Slot slot, Spell spell)
    {
        //记录复唱时间
        var d = Core.Resolve<MemApiSpell>().GetGCDDuration(true);
        if (d > 0) BattleData.Instance.GcdDuration = d;

        //Single Weave Skills
        if (spell.Id is SpellsDef.VoidReaping
                     or SpellsDef.CrossReaping)
        {
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
        }
        else { AI.Instance.BattleData.CurrGcdAbilityCount = 2; }
    }

    public void OnBattleUpdate(int currTime) //战斗中逐帧检测
    {
        var gcdProgPctg = (int)((GCDHelper.GetGCDCooldown() / (double)BattleData.Instance.GcdDuration) * 100);
        var inTN = Core.Me.HasAura(AurasDef.TrueNorth);
        var GibGallowsReady = Core.Me.HasAura(AurasDef.SoulReaver) || Core.Me.HasAura(AurasDef.Executioner);

        if (!inTN && 
                !Core.Me.HasAura(AurasDef.Enshrouded) && 
                Core.Me.GetCurrTarget() is not null && 
                Core.Me.GetCurrTarget().HasPositional())
        {
            if (GibGallowsReady)
            {
                if (Core.Me.HasAura(AurasDef.EnhancedGallows))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Behind, gcdProgPctg);
                    //LogHelper.Print("ready for enhanced gallows");
                }
                else if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Flank, gcdProgPctg);
                    //LogHelper.Print("ready for enhanced gibbet");
                }
                else
                {
                    MeleePosHelper.Clear();
                    //LogHelper.Print("cleared");
                }
            }
            else if (Core.Resolve<JobApi_Reaper>().SoulGauge >= 50 || SpellsDef.SoulSlice.GetSpell().IsReadyWithCanCast())
            {
                if (Core.Me.HasAura(AurasDef.EnhancedGallows))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Behind, 1);
                    //LogHelper.Print("s>=50 or ss ready and enhanced gallows");
                }
                else if (Core.Me.HasAura(AurasDef.EnhancedGibbet))
                {
                    MeleePosHelper.Draw(MeleePosHelper.Pos.Flank, 1);
                    //LogHelper.Print("s>=50 or ss ready and enhanced gibbet");
                }
                else
                {
                    MeleePosHelper.Clear();
                    //LogHelper.Print("cleared");
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