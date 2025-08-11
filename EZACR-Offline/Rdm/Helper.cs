using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Game.ClientState.Objects.Types;
using EZACR_Offline.Rdm.Setting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Rdm;

public class AOEHelper
{
    public static bool TargerastingIsAOE(IBattleChara target, int timeleft)
    {
        int[] numbers = aoelist.list;
        int index = -1;
        if (target.IsCasting)
        {
            index = Array.IndexOf(numbers, (int)target.CastActionId);
            if (index != -1 && target.TotalCastTime - target.CurrentCastTime < timeleft)
                return true;
            else
                return false;
        }
        return false;
    }

    public static class aoelist
    {
        public static int[] list =
        {
            458,
            927,
            930,
            1038,
            1167,
            1365,
            1385,
            1688,
            2225,
            2279,
            2317,
            2375,
            2398,
            3170,
            3182,
            3282,
            3358,
            3416,
            3519,
            3676,
            3691,
            3696,
            3961,
            4013,
            4116,
            4126,
            4135,
            4149,
            4162,
            4323,
            4855,
            4918,
            5159,
            5182,
            5188,
            5317,
            5329,
            5331,
            5823,
            5919,
            5928,
            5959,
            6020,
            6107,
            6469,
            6547,
            6604,
            6607,
            6613,
            6887,
            7218,
            7242,
            7353,
            7356,
            7363,
            7364,
            7370,
            8023,
            8024,
            8025,
            8028,
            8039,
            8367,
            8521,
            8581,
            9349,
            9414,
            9416,
            9523,
            9568,
            9660,
            9665,
            9670,
            9676,
            9688,
            9760,
            9767,
            9774,
            9790,
            9808,
            9828,
            9841,
            9869,
            9990,
            10086,
            10097,
            10284,
            10415,
            10426,
            10532,
            10535,
            10541,
            11234,
            11238,
            11246,
            11256,
            11259,
            11440,
            13262,
            13314,
            13343,
            13389,
            13397,
            13404,
            13440,
            13461,
            13462,
            14119,
            14120,
            14135,
            14139,
            14177,
            14191,
            14195,
            14206,
            14439,
            14470,
            15042,
            15590,
            15593,
            15596,
            15601,
            15764,
            15772,
            15780,
            15783,
            15813,
            15824,
            15832,
            15982,
            16332,
            16339,
            16630,
            16631,
            17382,
            18245,
            18261,
            18437,
            18450,
            18627,
            18639,
            18668,
            18675,
            18678,
            18753,
            19288,
            19306,
            19315,
            19324,
            23367,
            23381,
            23383,
            23470,
            23517,
            23538,
            23541,
            23558,
            23629,
            23647,
            23670,
            23671,
            23703,
            23710,
            24220,
            24239,
            24241,
            24245,
            25386,
            25524,
            25654,
            25916,
            25922,
            25931,
            25936,
            25946,
            25950,
            26435,
            26437,
            26450,
            26451,
            27742,
            28033,
            28446,
            28474,
            28476,
            28477,
            28493,
            28495,
            28512,
            28527,
            28528,
            28779,
            28825,
            28827,
            28833,
            28837,
            28854,
            28904,
            28905,
            28996,
            28998,
            28999,
            29002,
            29003,
            29014,
            29021,
            29022,
            29046,
            29210,
            29211,
            29217,
            29272,
            29273,
            29281,
            29283,
            29352,
            29356,
            29561,
            29582,
            29584,
            29595,
            29818,
            30134,
            30140,
            30166,
            30206,
            30207,
            30224,
            30236,
            30243,
            30254,
            30257,
            31122,
            31139,
            31233,
            31234,
            31291,
            31292,
            31293,
            31303,
            31376,
            31390,
            31769,
            31773,
            31774,
            31780,
            31835,
            31885,
            31900,
            31910,
            31925,
            31926,
            31956,
            31978,
            32059,
            32100,
            32111,
            32115,
            32116,
            32117,
            32122,
            32132,
            32785,
            32868,
            32944,
            33024,
            33335,
            33338,
            33343,
            33356,
            33364,
            33907,
            33970,
            33974,
            34605,
            34699,
            7958,
            33449,
            33462,
            33453,
            14376,
            8269,
            8271,
            8301,
            8293,
            9606,
            25324,
            25338,
            25344,
            1761,
            1554,
            1384,
            8230,
            20428,
            21004,
            21000,
            20996,
            8150,
            8167,
            6893,
            6124,
            6137,
            6195,
            6181,
            6088,
            6090,
            6085,
            6091,
            6229,
            6453,
            6178,
            3254,
            25273,
            25268,
            16030,
            16106,
            16071,
            9408,
            9411,
            25272,
            25144,
            25153,
            25148,
            27851,
            25170,
            25169,
            7624,
            25181,
            25180,
            25886,
            25891,
            25741,
            25742,
            25672,
            25685,
            25690,
            25701,
            30421,
            30450,
            30955,
            30447,
            30798,
            5150,
            25147,
            7307,
            7311,
            7333,
            7762,
            7588,
            7581,
            7377,
            7474,
            7185,
            9686,
            3674,
            14336,
            14346,
            25677,
            7827,
            7843,
            26423,
            26422,
            23668,
            24021,
            10132,
            10162,
            10168,
            23667,
            24615,
            11635,
            12619,
            12615,
            27754,
            27481,
            26206,
            32054,
            32112,
            32114,
            31727,
            33018,
            7946,
            33080,
            33087,
            33250,
            33274,
            33275,
            33497,
            33490,
            33494,
            32113,
            11157,
            11178,
            11185,
            11528,
            651,
            3738,
            1357,
            20633,
            20486,
            20920,
            20912,
            10094,
            28973,
            6622,
            25648,
            34782,
            34813,
            34822,
            35011,
            35905,
            35587,
            35618,
            35034,
            35033,
            35015,
            34827,
            34841,
            34833,
            36026,
            35217,
            35240,
            35375,
            35385,
            36093,
            35420,
            36091,
            30981,
            30961,
            30974,
            35274,
            35268,
            35284,
            35279,
            35312,
            35280,
            35269,
            35285,
            35660,
            35676,
            35661,
            35853,
            35699,
            35702,
            35697,
            35698,
            5011,
            3809,
            3812,
            14433,
            14548,
            19328,
            3437,
            19538,
            35680,
            35695,
            35696,
            35870,
            35732,
            35457,
            35476,
            35725,
            34943,
            34921,
            35113,
            34947,
            34748,
            34763,
            29895,
            29906,
            29896,
            30508,
            30532,
            30534,
            29870,
            7822,
            8915,
            13708,
            12618,
            15728,
            15743,
            30294,
            30284,
            30287,
            30288,
            28907,
            24189
        };
    }
}
public class Helper
{
    public static string 下一个GCD技能(List<SlotResolverData> SlotResolvers)
    {
        if (SlotResolvers == null)
            return null;
        if (Core.Me == null || Core.Me.GetCurrTarget() == null)
            return null;

        // 获取所有技能check
        var firstGcdSkill = SlotResolvers
            .FirstOrDefault(srd => srd.SlotMode == SlotMode.Gcd &&
                                   srd.SlotResolver.Check() >= 0);
        if (firstGcdSkill != null)
        {
            //LogHelper.Print(AI.Instance.BattleData.CurrBattleTimeInSec + ","+ srd.SlotResolver.GetType().Name); 
            if (firstGcdSkill.SlotResolver.GetType().Name == "SamuraiGCD_斩浪")
            {
                return "奥义斩浪"; // 返回技能名称
            }
            if (firstGcdSkill.SlotResolver.GetType().Name == "SamuraiGCD_dot")
            {
                return "彼岸花"; // 
            }
            if (firstGcdSkill.SlotResolver.GetType().Name == "SamuraiGCD_Base")
            {
                return "基础技能"; // 
            }
            return firstGcdSkill.SlotResolver.GetType().Name;
        }

        return null; // 默认值
    }
    public static void 重置QT()
    {
        //LogHelper.Print("脱离战斗后将会重置 起手序列QT");
        QT.QTSET(QTKey.起手序列, false);
        //Dragoon.QT.QTSET(Dragoon.QTKey.起手序列, false);
        //Samurai.QT.QTSET(Samurai.QTKey.起手序列, false);
        //Summoner.QT.QTSET(Summoner.QTKey.起手序列, false);
    }
    private static readonly Stopwatch 发言计时器 = Stopwatch.StartNew();
    private static int _lastResult = -1;
    /// <summary>
    /// 以“每 1 秒最多一次”的频率把指定文本输出到日志。
    /// 如果在 1 秒内被多次调用，除第一次外都会被忽略，
    /// 从而避免刷屏、降低性能开销。
    /// </summary>
    /// <param name="str">要打印到日志的文本。</param>
    public static void 限时发送(string str)
    {
        // 距离上次判断不足 1000 ms 就直接返回旧结果
        if (发言计时器.ElapsedMilliseconds < 1000)
            return;
        LogHelper.Print(str);
        // 真正开始新一轮判定
        发言计时器.Restart();      // 重新计时
    }
    public static int 停手()
    {
        if (QT.QTGET(QTKey.小停一下))
            return 1;
        //if (Dragoon.QT.QTGET(Dragoon.QTKey.小停一下))
        //    return 1;
        //if (Samurai.QT.QTGET(Samurai.QTKey.小停一下))
        //    return 1;
        //if (Summoner.QT.QTGET(Summoner.QTKey.小停一下))
        //    return 1;

        if (!QT.QTGET(QTKey.自动停手))
            return -1;
        //if (!Dragoon.QT.QTGET(Dragoon.QTKey.自动停手))
        //    return -1;
        //if (!Samurai.QT.QTGET(Samurai.QTKey.自动停手))
        //    return -1;
        //if (!Summoner.QT.QTGET(Summoner.QTKey.自动停手))
        //    return -1;
        if (Core.Me.GetCurrTarget() != null && Core.Me.GetCurrTarget().CanAttack() && Helper.目标是否拥有其中的BUFF(Helper.敌人无敌BUFF))
        {
            限时发送("检测到目标无敌BUFF，暂停输出");
            return 1;
        }
        if (Helper.自身存在其中Buff剩余时间不足(Helper.无法造成伤害, 2))
        {
            限时发送("检测到自身有 变形/热病BUFF，暂停输出");
            return 1;
        }
        if (Helper.自身存在其中Buff剩余时间不足(Helper.加速度炸弹, 2))
        {
            限时发送("检测到自身有 加速度炸弹BUFF，暂停输出");
            return 1;
        }

        return -1;
    }

    public static void 停手等待()
    {/*
        //手动停手什么都不动
        if (PlayerOptions.Instance.Stop && _cachedTarget == null)
            return;


        if (Helper.停手())
        {        
            // 第一次进入停手：保存目标，然后调用官方停手（会清空目标）
            if (!PlayerOptions.Instance.Stop)
            {
                _cachedTarget = Core.Me.GetCurrTarget();   // 1. 缓存目标
                //LogHelper.Print("停手缓存目标：" + _cachedTarget.Name);
                PlayerOptions.Instance.Stop = true;
            }
            if (PlayerOptions.Instance.Stop)
            {
                if (_cachedTarget != null && _cachedTarget.CanAttack() && Core.Me.GetCurrTarget() == null)
                {
                    Core.Me.SetTarget(_cachedTarget);         // 4. 重新选中
                }
            }
        }
        else
        {        
            // 停手结束：关闭开关，恢复目标
            if (PlayerOptions.Instance.Stop)
            {
                PlayerOptions.Instance.Stop = false;          // 3. 关闭停手
                //切目标自动攻击
                if (Core.Me.GetCurrTarget() != null && Core.Me.GetCurrTarget().CanAttack())
                    _cachedTarget = null;

                if (_cachedTarget != null && _cachedTarget.CanAttack())
                {
                    Core.Me.SetTarget(_cachedTarget);         // 4. 重新选中
                }
                _cachedTarget = null;                        // 5. 清空缓存
            }
        }
*/
    }
    public static int 获取buff剩余时间(IBattleChara characterAgent, uint id)
    { return Core.Resolve<MemApiBuff>().GetAuraTimeleft(characterAgent, id, fromMe: false); }
    public static bool 目标是否在剧情状态(IBattleChara target)
    {

        var result = false;

        if (!target.IsValid())
        {
            return result;
        }

        try
        {
            //result = target.OnlineStatus.RowId == 15;
        }
        finally
        {

        }

        return result;
    }
    // 使用字典存储角色名和死亡时间
    public static Dictionary<string, DateTime> 死亡记录 = new Dictionary<string, DateTime>();
    public static bool 死亡计时(string characterName, int ttime)
    {
        // 获取当前时间
        DateTime currentDeathTime = DateTime.Now;

        // 检查该角色是否已有死亡记录
        if (死亡记录.ContainsKey(characterName))
        {
            // 获取该角色的上一次死亡时间
            DateTime lastDeathTime = 死亡记录[characterName];

            // 计算时间差
            TimeSpan timeDifference = currentDeathTime - lastDeathTime;

            // 如果时间差大于5秒，则更新死亡时间
            if (timeDifference.TotalSeconds >= ttime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public static void 更新死亡时间(string characterName)
    {
        // 获取当前时间作为死亡时间
        DateTime currentDeathTime = DateTime.Now;
        // 检查该角色是否已有死亡记录
        if (死亡记录.ContainsKey(characterName))
        {
            // 获取该角色的上一次死亡时间
            DateTime lastDeathTime = 死亡记录[characterName];

            // 计算时间差
            TimeSpan timeDifference = currentDeathTime - lastDeathTime;

            // 如果时间差大于5秒，则更新死亡时间
            if (timeDifference.TotalSeconds >= 10)
            {
                死亡记录[characterName] = currentDeathTime;
                //ChatHelper.SendMessage("/e 更新死亡时间" + characterName + "死亡时长:" + timeDifference.TotalSeconds + "秒");
                //Console.WriteLine($"角色 {characterName} 的死亡时间已更新为 {deathTime}。");
            }
            else
            {
                //Console.WriteLine($"角色 {characterName} 的死亡时间未更新，因为距离上次死亡不足5秒。");
            }
        }
        else
        {
            // 如果该角色没有死亡记录，则直接添加
            死亡记录[characterName] = currentDeathTime;
            //ChatHelper.SendMessage("/e 记录死亡时间" + characterName + "死亡时间:" + currentDeathTime);
            //Console.WriteLine($"角色 {characterName} 在 {deathTime} 死亡了。");
        }
    }

    public static string 查询地图(string id)
    {
        // 获取地图数据
        List<MapData> maps = MapDatabase.Maps;
        // 查找特定的地图
        var targetMap = maps.Find(map => map.Id == id);

        if (targetMap != null)  // 修正为检查 null
        {
            return targetMap.MapName;
        }
        return "未知";
    }
    private static DateTime? lastLogTime = null;
    public static void 救人日记(string 名字, string 技能)
    {
        // 记录到CSV文件
        var path = RedMageSettings.LogPath;
        var 地图id = Core.Resolve<MemApiZoneInfo>().GetCurrZoneInfo().MapBaseName;
        var 副本名 = Core.Resolve<MemApiDuty>().DutyName();

        if (副本名 == "")
            副本名 = "空";
        // 检查文件是否存在以及创建时间
        if (File.Exists(path))
        {
            var creationTime = File.GetCreationTime(path);
            var thresholdDate = new DateTime(2025, 4, 12);

            // 如果文件创建时间早于2025年4月12日，则删除文件
            if (creationTime < thresholdDate)
            {
                File.Delete(path);
            }
        }

        // 如果文件不存在，创建一个新文件
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "时间,地图ID,副本名,目标名,技能\n", Encoding.UTF8); // 写入表头
        }
        //时间,副本id,副本名,目标名,死亡时长(ms)
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{Helper.查询地图(地图id)},{副本名},{名字},{技能}\n";
        // 移除换行符
        //var logEntry2 = logEntry.Replace("\n", " ");
        //ChatHelper.SendMessage("/e " + logEntry2);
        // 检查最后一条记录的时间
        if (lastLogTime.HasValue && (DateTime.Now - lastLogTime.Value).TotalSeconds < 3)
        {
            //Console.WriteLine("最后一条记录的时间与当前时间相差小于3秒，不写入文件。");
            return; // 不写入文件
        }

        // 写入文件
        File.AppendAllText(path, logEntry, Encoding.UTF8);

        // 更新最后一条记录的时间
        lastLogTime = DateTime.Now;
    }
    public static (int TodayCount, int TotalCount) 导随日记()
    {
        // 记录到CSV文件
        var path = RedMageSettings.Instance.导随记录文件;

        // 检查文件是否存在
        if (!File.Exists(path))
        {
            return (0, 0);
        }

        // 获取今天的日期
        var today = DateTime.Today;

        // 计数器
        int todayCount = 0;
        int totalCount = 0;

        // 使用FileStream和StreamReader读取文件，指定文件共享模式
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
        {
            string line;
            // 跳过标题行
            sr.ReadLine();

            int tnum = 0; // 行号计数器
            while ((line = sr.ReadLine()) != null)
            {
                tnum++;
                // 分割每一行的字段
                var columns = line.Split(',');

                // 检查字段数量是否足够
                if (columns.Length < 7)
                {
                    //LogHelper.Print($"跳过第{tnum}行：{line}，字段数量不足");
                    continue; // 跳过当前行
                }
                columns[0] = columns[0].Replace("\uFEFF", ""); // 移除BOM字符
                // 检查任务类型是否为“随机任务：指导者任务”
                if (!string.Equals(columns[0].Trim(), "随机任务：指导者任务", StringComparison.OrdinalIgnoreCase))
                {
                    //LogHelper.Print($"跳过第{tnum}行，任务类型不匹配:{columns[0]}");
                    continue; // 跳过当前行
                }

                // 解析日期
                if (!DateTime.TryParseExact(columns[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                {
                    //LogHelper.Print($"跳过第{tnum}行，日期解析失败:{columns[1]}");
                    continue; // 跳过当前行
                }

                // 统计
                totalCount++; // 总计数加1
                if (taskDate.Date == today)
                {
                    todayCount++; // 今日计数加1
                }
            }
        }

        return (todayCount, totalCount);
    }
    public static (int TodayCount, int TotalCount) 赤菩萨日记()
    {
        // 记录到CSV文件
        var path = RedMageSettings.LogPath;

        // 检查文件是否存在
        if (!File.Exists(path))
        {
            return (0, 0);
        }

        // 获取今天的日期
        var today = DateTime.Today;

        // 计数器
        int todayCount = 0;
        int totalCount = 0;

        // 使用FileStream和StreamReader读取文件，指定文件共享模式
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (StreamReader sr = new StreamReader(fs))
        {
            string line;
            // 跳过标题行
            sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                // 分割每一行的字段
                var columns = line.Split(',');

                // 确保字段数量足够
                if (columns.Length >= 5)
                {
                    // 解析日期
                    if (DateTime.TryParseExact(columns[0], "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime taskDate))
                    {
                        totalCount++; // 总计数加1
                        if (taskDate.Date == today)
                        {
                            todayCount++; // 今日计数加1
                        }
                    }
                }
            }
        }

        return (todayCount, totalCount);
    }
    public static IBattleChara 获取死亡玩家(int 距离)
    {
        // 创建一个字典来存储25米内的所有死亡玩家数据
        Dictionary<uint, IBattleChara> all = new Dictionary<uint, IBattleChara>();

        // 遍历所有游戏对象，筛选出符合条件的死亡玩家
        foreach (IBattleChara unit in ECHelper.Objects.OfType<IBattleChara>())
        {
            // 跳过无效的单位（DataId为0）
            if (unit.ObjectKind != Dalamud.Game.ClientState.Objects.Enums.ObjectKind.Player)
            {
                continue;
            }

            // 计算战斗范围
            float combatReach = Core.Me.HitboxRadius + unit.HitboxRadius;

            // 计算与当前角色的距离
            var unitDis = Core.Me.Distance(unit);

            // 判断是否在指定范围内
            if (unitDis < 距离 - 1 + combatReach)
            {
                // 将符合条件的单位加入字典
                all[unit.EntityId] = unit;
            }
        }

        // 从字典中筛选出没有复活buff（AuraId为148）、已死亡且不是当前角色自己的玩家
        var allTarget = all.FirstOrDefault(r => !r.Value.HasAura(148) &&
                                                 r.Value.IsDead &&
                                                 //r.Value.ObjectKind == ObjectKind.Player &&
                                                 r.Value.Name.ToString() != Core.Me.Name.ToString());

        // 返回筛选结果
        return allTarget.Value;
    }

    #region buff1
    /** 限制类 **/
    public const uint 限制复活 = 3380u;

    /** 无敌类 **/
    public const uint 死而不僵 = 811u;

    public const uint 行尸走肉 = 810u;
    public const uint 出生入死 = 3255u;
    public const uint 神圣领域 = 1302u;
    public const uint 死斗 = 409u;
    public const uint 超火流星 = 1836u;

    public static List<uint> 无敌Buff =
    [
        死而不僵,
        行尸走肉,
        出生入死,
        死斗,
        超火流星,
        神圣领域
    ];

    public static List<uint> 真无敌Buff =
    [
        超火流星,
        神圣领域
    ];

    public static readonly List<uint> 假无敌Buff =
    [
        死而不僵,
        行尸走肉,
        出生入死,
        死斗
    ];

    /** 无敌类-BOSS **/
    public const uint 无敌_325 = 325u;

    public const uint 无敌_529 = 529u;
    public const uint 无敌_656 = 656u;
    public const uint 无敌_671 = 671u;
    public const uint 无敌_775 = 775u;
    public const uint 无敌_776 = 776u;
    public const uint 无敌_969 = 969u;
    public const uint 无敌_981 = 981u;
    public const uint 无敌_1570 = 1570u;
    public const uint 无敌_1697 = 1697u;
    public const uint 无敌_1829 = 1829u;
    public const uint 土神的心石 = 328u;
    public const uint 纯正神圣领域 = 2287u;
    public const uint 冥界行 = 2670u;
    public const uint 风神障壁 = 3012u;
    public const uint 不死救赎 = 3039u;
    public const uint 出死入生 = 3255u;
    #endregion
    #region buff2
    public const uint 昏乱1 = 1203u;
    public const uint 昏乱2 = 1988u;

    //特殊的
    public const uint 远程物理攻击无效化 = 941u; //远程物理攻击无法造成伤害
    public const uint 魔法攻击无效化_942 = 942u; //魔法攻击无法造成伤害
    public const uint 魔法攻击无效化_3621 = 3621u; //魔法攻击无法造成伤害
    public static readonly List<uint> 敌人无敌BUFF =
[
    //昏乱1,昏乱2,
    无敌_325,
    无敌_529,
    无敌_656,
    无敌_671,
    无敌_775,
    无敌_776,
    无敌_969,
    无敌_981,
    无敌_1570,
    无敌_1697,
    无敌_1829,
    土神的心石,
    纯正神圣领域,
    冥界行,
    风神障壁,
    不死救赎,
    出死入生,
    死而不僵,
    行尸走肉,
    出生入死,
    死斗,
    超火流星
];
    /** 无法造成伤害类 **/
    public const uint 愤怒的化身 = 2208u; //愤怒不已，愤怒状态的玩家无法造成伤害
    public const uint 拘束 = 292u;
    public const uint 悲叹的化身 = 2209u; //悲叹不已，悲叹状态的玩家无法造成伤害
    public const uint 青蛙 = 2671u; //变成一只青蛙，无法发动任何技能
    public const uint 蛙变 = 439u; //变成一只青蛙，无法发动任何技能 欧密茄 德尔塔3
    public const uint AccelerationBomb4144 = 4144u;
    public const uint AccelerationBomb3802 = 3802u;
    public const uint AccelerationBomb3793 = 3793u;
    public const uint AccelerationBomb1384 = 1384u;
    public const uint AccelerationBomb2657 = 2657u;
    public const uint AccelerationBomb1072 = 1072u;
    public const uint 热病1 = 639u;
    public const uint 热病2 = 960u;
    public const uint 热病3 = 1049u;
    public const uint 热病4 = 1133u;
    public const uint 热病5 = 1599u;
    public const uint 热病6 = 3522u;
    public const uint 魔法攻击无效化942 = 942u;
    public const uint 魔法攻击无效化3621 = 3621u;
    public const uint 逐渐升温 = 2898u;
    public const uint 即刻 = 167u;

    public static readonly List<uint> 无法造成伤害 = [愤怒的化身, 悲叹的化身, 青蛙, 蛙变, 拘束, 热病1, 热病2, 热病3, 热病4, 热病5, 热病6];
    public static readonly List<uint> 加速度炸弹 = [AccelerationBomb4144, AccelerationBomb3802, AccelerationBomb3793, AccelerationBomb1384, AccelerationBomb2657, AccelerationBomb1072];
    public static readonly List<uint> 魔法无效化 = [魔法攻击无效化942, 魔法攻击无效化3621];
    public static readonly List<uint> 不可选中 = [拘束];
    public static readonly List<uint> 无法发动技能类 = [青蛙, 拘束];

    #endregion
    public static bool 目标是否拥有其中的BUFF(List<uint> auras, int timeLeft = 0)
    {
        return Core.Me.GetCurrTarget().HasAnyAura(auras, timeLeft);
    }
    public static bool 自身存在其中Buff(List<uint> auras, int msLeft = 0)
    {
        //msleft 是 大于xx毫秒
        return Core.Me.HasAnyAura(auras, msLeft);
    }
    public static bool 自身存在其中Buff剩余时间不足(List<uint> buff列表, int 剩余秒阈值)
    {
        var me = Core.Me as IBattleChara;
        if (me == null || buff列表 == null || buff列表.Count == 0)
            return false;

        foreach (var buffId in buff列表)
        {
            int 剩余时间 = 获取buff剩余时间(me, buffId);
            //if (剩余时间 > 0)
            //    LogHelper.Print("剩余时间："+ buffId+":"+剩余时间);
            if (剩余时间 > 0 && 剩余时间 < 剩余秒阈值 * 1000)
                return true;
        }

        return false;
    }
    public static bool 是否拥有BUFF(IBattleChara target, uint id)
    {
        return target.HasAura(id);
    }
    public static bool 目标是否可见或在技能范围内(uint actionId)
    {
        //return true;
        var yy = Core.Resolve<MemApiSpell>().GetActionInRangeOrLoS(actionId) is not (566 or 562);
        return !yy;
    }
    /*
    /// <summary>
    /// 使用底层的 BGCollisionModule 直接检查两个角色之间是否存在视线遮挡。
    /// </summary>
    /// <param name="source">来源角色。</param>
    /// <param name="target">目标角色。</param>
    /// <param name="hitPoint">如果被阻挡，返回碰撞点的坐标。</param>
    /// <returns>如果视线被阻挡，返回 true；否则返回 false。</returns>
    public static unsafe bool IsBlocked(GameObject* source, GameObject* target)
    {
        // 为了更真实的判断，通常会给起点和终点一个运算符“+”对于“Vector3”和“Vector3”类型的操作数具有二义性Y轴上的偏移
        // 例如，从角色胸口位置发射，而不是脚底

        var sourcePos = *source->GetPosition();
        var targetPos = *target->GetPosition();

        sourcePos.Y += 2;
        targetPos.Y += 2;


        var offset = targetPos - sourcePos;
        var maxDist = offset.Magnitude;
        var direction = offset / maxDist;
        // 调用底层的射线投射函数
        // 注意：这个函数期望的距离是一个 float，而不是 byte，更精确
        bool hasHit = BGCollisionModule.RaycastMaterialFilter(sourcePos, direction, out _, maxDist);

        if (hasHit)
        {
            return true; // 有碰撞，视线被阻挡
        }

        return false; // 没有碰撞，视线清晰
    }
    */
}

