using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.CombatRoutine.View.JobView.HotkeyResolver;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using ImGuiNET;
using PatchouliTC.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Rdm;

public class RedMageRotationEntry
{
    public string AuthorName { get; set; } = "Anmi";



    // 逻辑从上到下判断，通用队列是无论如何都会判断的 
    // gcd则在可以使用gcd时判断
    // offGcd则在不可以使用gcd 且没达到gcd内插入能力技上限时判断
    // pvp环境下 全都强制认为是通用队列
    private List<SlotResolverData> SlotResolvers = new()
    {
        // 通用队列 不管是不是gcd 都会判断的逻辑
        //new(new XXXXXXXX(),SlotMode.Always),
        
        // gcd队列
        new(new RedMageGCD_小停(),SlotMode.Gcd),
        new(new RedMageGCD_后魔3连(),SlotMode.Gcd),
        new(new RedMageGCD_前魔3连(),SlotMode.Gcd),
        new(new RedMageGCD_赤复活(),SlotMode.Gcd),
        new(new RedMageGCD_Base(),SlotMode.Gcd),
        new(new RedMageGCD_90以下(),SlotMode.Gcd),
        
        // offGcd队列

        new (new RedMage_Ability_Base(),SlotMode.OffGcd),

    };


    public Rotation Build(string settingFolder)
    {
        // 初始化设置
        RedMageSettings.Build(settingFolder);
        // 初始化QT （依赖了设置的数据）
        BuildQT();

        var style = new QtStyle(RedMageSettings.Instance.JobViewSave);


        var rot = new Rotation(SlotResolvers)
        {
            TargetJob = Jobs.RedMage,
            AcrType = AcrType.Both,
            MinLevel = 0,
            MaxLevel = 100,
            Description = "RedMage",
        };
        //添加停手方案
        rot.AddCanPauseACRCheck(Helper.停手);
        rot.AddOpener(GetOpener);
        // 添加各种事件回调
        rot.SetRotationEventHandler(new RedMageRotationEventHandler());
        // 添加QT开关的时间轴行为
        rot.AddTriggerAction(new TriggerAction_QT(), new 赤魔时间轴新QT设置(), new TriggerAction_QuickQT(), new TriggerAction_新QT2());
        rot.AddTriggerCondition(new TriggerAction_白魔元(), new TriggerAction_黑魔元(), new TriggerAction_魔元集());

        return rot;
    }
    IOpener? GetOpener(uint level)
    {
        if (level < 70)
            return null;
        if (level == 70)
            return new RedMage_Opener70();
        if (level == 80)
            return new RedMage_Opener70();
        if (level >= 90 && level < 100)
            return new RedMage_Opener90();
        return new RedMage_Opener100();
    }
    // 声明当前要使用的UI的实例 示例里使用QT
    public static JobViewWindow QT { get; private set; }

    // 如果你不想用QT 可以自行创建一个实现IRotationUI接口的类
    public IRotationUI GetRotationUI()
    {
        return QT;
    }

    // 构造函数里初始化QT
    public void BuildQT()
    {

        // JobViewSave是AE底层提供的QT设置存档类 在你自己的设置里定义即可
        // 第二个参数是你设置文件的Save类 第三个参数是QT窗口标题
        QT = new JobViewWindow(RedMageSettings.Instance.JobViewSave, RedMageSettings.Instance.Save, "Anmi");
        RedMageRotationEntry.QT.SetUpdateAction(OnUIUpdate); // 设置QT中的Update回调 不需要就不设置

        //添加QT分页 第一个参数是分页标题 第二个是分页里的内容
        RedMageRotationEntry.QT.AddTab("通用", DrawQtGeneral);
        RedMageRotationEntry.QT.AddTab("Dev", DrawQtDev);

        // 添加QT开关 第二个参数是默认值 (开or关) 第三个参数是鼠标悬浮时的tips

        RedMageRotationEntry.QT.AddQt(QTKey.爆发, true, "控制魔元化和鼓励");
        RedMageRotationEntry.QT.AddQt(QTKey.最终爆发, false, "最终爆发");
        RedMageRotationEntry.QT.AddQt(QTKey.输出即刻, true, "即刻用于输出");
        RedMageRotationEntry.QT.AddQt(QTKey.起手序列, false, QTKey.起手序列);
        RedMageRotationEntry.QT.AddQt(QTKey.AOE, true, "AOE");
        RedMageRotationEntry.QT.AddQt(QTKey.魔元化, true, "");
        RedMageRotationEntry.QT.AddQt(QTKey.鼓励, true, "");
        RedMageRotationEntry.QT.AddQt(QTKey.拉人, RedMageSettings.Instance.拉人, "拉人2.0，在通用面板自定义复活时间");
        RedMageRotationEntry.QT.AddQt(QTKey.自动昏乱, RedMageSettings.Instance.自动昏乱, "自动昏乱、抗死");
        RedMageRotationEntry.QT.AddQt(QTKey.交剑, true, "好了就打，在近战范围内");
        RedMageRotationEntry.QT.AddQt(QTKey.短兵, true, "好了就打，在近战范围内");
        RedMageRotationEntry.QT.AddQt(QTKey.自奶, false, "自己奶一小口");
        RedMageRotationEntry.QT.AddQt(QTKey.保留促进, false, "保留促进");
        RedMageRotationEntry.QT.AddQt(QTKey.魔六, true, "控制魔六,关闭这个QT将不打魔1也就不会打后面的23456");
        RedMageRotationEntry.QT.AddQt(QTKey.锅炉圣人, false, "极速拉起周围25米内死亡的玩家（不论是否在战斗状态）");
        RedMageRotationEntry.QT.AddQt(QTKey.短交留一层, RedMageSettings.Instance.留一层, "短交留一层");
        RedMageRotationEntry.QT.AddQt(QTKey.老年圣人, false, "默认拉起周围25米内死亡3秒的玩家（更像人）");
        RedMageRotationEntry.QT.AddQt(QTKey.飞刺六分, true, "飞刺六分，轴用");
        RedMageRotationEntry.QT.AddQt(QTKey.强制魔元化, false, "强制魔元化，ACR自用，测3");
        RedMageRotationEntry.QT.AddQt(QTKey.小停一下, false, "小停一下");
        RedMageRotationEntry.QT.AddQt(QTKey.自动停手, true, "轴控最好关闭，日随用：变成青蛙、身上有热病、加速度炸弹的时候停手");
        RedMageRotationEntry.QT.AddQt(QTKey.范围拉人, RedMageSettings.Instance.范围拉人, "无视小队，拉取周边死亡玩家");
        RedMageRotationEntry.QT.AddQt(QTKey.对齐GCD, false, "单数次魔六后接促进以对齐GCD");
        RedMageRotationEntry.QT.AddQt(QTKey.移动即刻, false, "轴控QT，使用时最好关闭输出即刻QT");

        RedMageRotationEntry.QT.AddHotkey("LB", new HotKeyResolver_法系LB());
        RedMageRotationEntry.QT.AddHotkey("爆发药", new HotKeyResolver_Potion());
        RedMageRotationEntry.QT.AddHotkey("沉稳咏唱", new HotKeyResolver_NormalSpell(SpellsDefine.Surecast, SpellTargetType.Self, false));
        RedMageRotationEntry.QT.AddHotkey("昏乱", new HotKeyResolver_NormalSpell(SpellsDefine.Addle, SpellTargetType.Target, false));
        RedMageRotationEntry.QT.AddHotkey("抗死", new HotKeyResolver_NormalSpell(SpellsDefine.MagickBarrier, SpellTargetType.Target, false));
        RedMageRotationEntry.QT.AddHotkey("疾跑", new HotKeyResolver_疾跑());
        RedMageRotationEntry.QT.AddHotkey("魔回刺,自适应魔划圆斩", new 魔回刺hotkey());
        RedMageRotationEntry.QT.AddHotkey("即刻/赤治疗复活，会优先复活当前目标", new 即刻赤复活hotkey());

        if (RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Count == 0)
        {
            // 列表为空
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Clear();
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.最终爆发);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.输出即刻);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.魔元化);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.鼓励);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.交剑);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.短兵);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.魔六);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.保留促进);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.短交留一层);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.锅炉圣人);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.飞刺六分);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.强制魔元化);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.小停一下);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.对齐GCD);
            RedMageSettings.Instance.JobViewSave.QtUnVisibleList.Add(QTKey.自动停手);
            RedMageSettings.Instance.Save();
        }

        // 添加快捷按钮 (带技能图标)
        /*
        RedMageRotationEntry.QT.AddHotkey("战斗之声",
            new HotKeyResolver_NormalSpell(SpellsDefine.BattleVoice, SpellTargetType.Self));
        RedMageRotationEntry.QT.AddHotkey("失血",
            new HotKeyResolver_NormalSpell(SpellsDefine.Bloodletter, SpellTargetType.Target));
        RedMageRotationEntry.QT.AddHotkey("爆发药", new HotKeyResolver_Potion());
        RedMageRotationEntry.QT.AddHotkey("极限技", new HotKeyResolver_LB());
        */
        /*
        // 这是一个自定义的快捷按钮 一般用不到
        // 图片路径是相对路径 基于AEAssist(C|E)NVersion/AEAssist
        // 如果想用AE自带的图片资源 路径示例: Resources/AE2Logo.png
        SageRotationEntry.QT.AddHotkey("极限技", new HotkeyResolver_General("#自定义图片路径", () =>
        {
            // 点击这个图片会触发什么行为
            LogHelper.Print("你好");
        }));
        */
    }

    // 设置界面
    public void OnDrawSetting()
    {
        RedMageSettingUI.Instance.Draw();
    }

    public void OnUIUpdate()
    {

    }

    public void DrawQtGeneral(JobViewWindow jobViewWindow)
    {
        ImGui.Checkbox("诊断模式", ref RedMageSettings.Instance.test);
        int previous复活时间 = RedMageSettings.Instance.复活时间; // 保存当前值
        ImGui.SliderInt("复活延迟", ref RedMageSettings.Instance.复活时间, 0, 5, "%d秒");
        var (本日数, 总计数) = Helper.导随日记();
        ImGui.Text("本日导随数量：" + 本日数); ImGui.SameLine(); // 使文字和滑块在同一行显示
        ImGui.Text("总计导随数量：" + 总计数);

        /*
        var 当前划圆斩 = Core.Resolve<MemApiSpell>().CheckActionChange(技能.划圆斩);
        if (ImGui.Button("test"))
            LogHelper.Print($"{当前划圆斩}");
        var 目标 = Helper.获取死亡玩家(25);
        var 目标2 = "";
        if (目标 != null)
        { 目标2 = 目标.Name.ToString(); }
        ImGui.Text("周围死亡的玩家第一名：" + 目标2);
        var 爆发药CD = Anmi.Samurai.Triggers.TriggerAction_爆发药CD.爆发药CD();
        var 当前坐标 = Core.Me.Position;
        // 创建目标点，X 坐标加上 200，Y 和 Z 坐标保持不变
        Vector3 targetPoint = new Vector3(当前坐标.X + 20, 当前坐标.Y, 当前坐标.Z);
        if (ImGui.Button("前往坐标"))
        {
            //LogHelper.Print($"目标坐标：X={当前坐标.X:F2}, Y={当前坐标.Y:F2}, Z={当前坐标.Z:F2}");
            //Core.Resolve<MemApiMove>().MoveToTarget(targetPoint);
            ChatHelper.SendMessage("/vnav moveto -86.740 -40.047 312.165");
        }
        // 将下一个按钮放在同一行
        ImGui.SameLine();

        if (ImGui.Button("停止"))
        {
            //Core.Resolve<MemApiMove>().CancelMove();
            ChatHelper.SendMessage("/vnav stop");
        }
        ImGui.Text("爆发药CD：" + 爆发药CD);
        */
        //LogHelper.Print(爆发药CD.ToString());
        // 检查复活时间是否发生了变化
        if (RedMageSettings.Instance.复活时间 != previous复活时间)
        {
            // 保存新的设置值
            RedMageSettings.Instance.Save();
        }
        if (ImGui.CollapsingHeader("赤菩萨日记"))
        {
            ImGui.Text("导随日记：(与ACT插件日随记录器的联动)");
            // 将属性值赋给局部变量
            string filePath = RedMageSettings.Instance.导随记录文件;

            ImGui.InputText("数据.csv 文件路径", ref filePath, 256);
            // 如果用户修改了路径，保存配置文件
            if (ImGui.IsItemDeactivatedAfterEdit())
            {
                // 将局部变量的值赋回属性
                RedMageSettings.Instance.导随记录文件 = filePath;
                RedMageSettings.Instance.Save();
            }
            if (ImGui.Button("打开导随日记"))
            {
                try
                {
                    // 直接在按钮点击事件中打开文件夹并选中文件
                    Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                }
                catch (Exception ex)
                {
                    ImGui.Text("无法打开文件夹：" + ex.Message);
                }
            }

            ImGui.Text("---------------------");
            ImGui.Text("救人日记：");
            ImGui.SameLine(); // 使文字和滑块在同一行显示

            var (本日救人数, 总计救人数) = Helper.赤菩萨日记();
            ImGui.Text("本日救人数量：" + 本日救人数); ImGui.SameLine();
            ImGui.Text("总计救人数量：" + 总计救人数);


            if (ImGui.Button("打开救人日记"))
            {
                try
                {
                    // 直接在按钮点击事件中打开文件夹并选中文件
                    Process.Start("explorer.exe", $"/select,\"{RedMageSettings.LogPath}\"");
                }
                catch (Exception ex)
                {
                    ImGui.Text("无法打开文件夹：" + ex.Message);
                }
            }
        }
        if (ImGui.CollapsingHeader("DEBUG"))
        {
            var LastSpellId = Core.Resolve<MemApiSpell>().GetLastComboSpellId();
            var target = Core.Me.GetCurrTarget();
            var 连续咏唱buff = Core.Me.HasAura(AurasDefine.Dualcast);
            var 雷火buff = Core.Me.HasAura(AurasDefine.VerfireReady) && Core.Me.HasAura(AurasDefine.VerstoneReady);
            var 促进buff = Core.Me.HasAura(AurasDefine.Acceleration);
            var 连按 = !SpellsDefine.Acceleration.RecentlyUsed(2000) && SpellsDefine.Acceleration.GetSpell().IsReadyWithCanCast();
            var AOE判断 = (AOEHelper.TargerastingIsAOE(target, 4) || TargetHelper.targetCastingIsBossAOE(target, 4000));
            var 有昏乱 = Core.Me.GetCurrTarget().HasAura(1203) || Core.Me.GetCurrTarget().HasAura(1988);
            var 有抗死 = Core.Me.HasAura(2707) || Core.Me.HasAura(3240);
            var 刚放昏乱 = SpellsDefine.Addle.RecentlyUsed(10000);

            var 荆棘绞剑预备 = Core.Me.HasAura(Buffs.荆棘绞剑预备);
            var 光芒四射预备 = Core.Me.HasAura(Buffs.光芒四射预备);
            var 战斗时间 = AI.Instance.BattleData.CurrBattleTimeInMs;
            var 鼓励cd = SpellsDefine.Embolden.GetSpell().Cooldown.TotalMilliseconds;
            var 魔元集 = Core.Resolve<JobApi_RedMage>().ManaStacks;
            var 不在魔六连中 = (魔元集 == 0) && !(LastSpellId == SpellsDefine.Verflare || LastSpellId == SpellsDefine.Verholy || LastSpellId == SpellsDefine.Scorch || LastSpellId == SpellsDefine.Resolution);
            var 移动中 = Core.Me.IsMoving();

            var 输出即刻1 = Anmi.RedMage.QT.QTGET("输出即刻");
            var 输出即刻2 = !连续咏唱buff && !促进buff;
            var 输出即刻3 = SpellsDefine.Swiftcast.GetSpell().IsReadyWithCanCast();
            var 输出即刻4 = 不在魔六连中 && 移动中;
            var 前三连 = RedMageSpellHelper.前三连();

            ImGui.Text("输出即刻1-QT判断：" + 输出即刻1);
            ImGui.Text("输出即刻2-瞬发BUFF：" + 输出即刻2);
            ImGui.Text("输出即刻3-即刻CD：" + 输出即刻3);
            ImGui.Text("输出即刻4-魔六+移动：" + 输出即刻4);

            ImGui.Text("魔三连中：" + 前三连);
            var 读条中 = Core.Me.IsCasting;
            ImGui.Text("读条中：" + 读条中);
            var 连击剩余时间 = Core.Resolve<MemApiSpell>().GetComboTimeLeft().TotalMilliseconds;
            if (LastSpellId == 0) LastSpellId = 7561;
            ImGui.Text("连击剩余时间：" + 连击剩余时间 + ",上一个技能：" + LastSpellId + ":" + LastSpellId.GetSpell().Name);
            //LogHelper.Print("上一个技能：" + LastSpellId.GetSpell().Name+ LastSpellId);

            var 距离 = Core.Me.GetCurrTarget()?.DistanceToPlayer();
            ImGui.Text("与目标的距离：" + 距离);
            ImGui.Text("荆棘：" + 鼓励cd + "," + 荆棘绞剑预备 + "," + (鼓励cd < 115000));
        }

        if (ImGui.CollapsingHeader("更新日志"))
        {

            ImGui.Text("2025-6-24:更新支持7.2");
            ImGui.Text("2025-4-26:范围拉人QT同时影响复活逻辑 和 复活HOTKEY");
            ImGui.Text("2025-4-26:新增 范围拉人QT 取代锅炉圣人");
            ImGui.Text("2025-4-26:调整锅炉圣人只在脱战状态起作用");
            ImGui.Text("2025-4-13:增加玩具 赤菩萨日记");
            ImGui.Text("2025-4-10:暂时保留拉人各项参数提醒，等功能进一步完工将会取消显示");
            ImGui.Text("2025-4-10:微调魔三连逻辑，主要适配50以下,50-68阶段");
            ImGui.Text("2025-4-9:微调魔元化、鼓励逻辑，更合理一些");
            ImGui.Text("2025-4-8:配置文件新增救人日记.csv，可以查询自己的菩萨事迹");
            ImGui.Text("2025-4-7:增加拉人2.0，在通用面板拉滑块调整复活延迟");
            ImGui.Text("2025-3-27:优化魔三连逻辑，尽量不打断");
            ImGui.Text("2025-3-27:修复移动中读条频繁拉断");
            ImGui.Text("2024-12-11:添加老年圣人，拉人的时候更像人");
            ImGui.Text("2024-11-1:修正赤复活拉人，添加法系LB专用按钮");
            ImGui.Text("2024-10-14:修正魔化圆，短兵增加移动中不释放");
            ImGui.Text("2024-10-14:修复低级副本（70）有可能卡手的问题");
            ImGui.Text("2024-10-11:增加一些轴用的QT");
            ImGui.Text("2024-9-30:强化锅炉圣人模式");
            ImGui.Text("2024-8-7：适配100级");
            ImGui.Text("2024-7-12：移植3.0");
        }
    }

    public void DrawQtDev(JobViewWindow jobViewWindow)
    {
        var 单数次魔六 = RedMageSpellHelper.魔六次数 % 2 == 1;
        var 魔能50 = Core.Resolve<JobApi_RedMage>().BlackMana >= 50 && Core.Resolve<JobApi_RedMage>().WhiteMana >= 50;
        var 连击预备 = Core.Me.HasAura(Buffs.魔法剑连击预备); ;//存在连击
        var 对齐判断 = 单数次魔六 && !魔能50 && !连击预备;

        ImGui.Text($"单数次魔六:{单数次魔六}");
        ImGui.Text($"魔能是否大于50:{魔能50}");
        ImGui.Text($"是否有连击预备:{连击预备}");
        ImGui.Text($"是否需要对齐GCD:{对齐判断}");
    }
    public void Dispose()
    {
        // 释放需要释放的东西 没有就留空
    }
}
