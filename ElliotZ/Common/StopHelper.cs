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
    private static bool _untargeted = false;
    private static bool _lastStopVal = false;

    public static readonly List<uint> AccelBomb = [4144, 3802, 3793, 1384, 2657, 1072];

    public static readonly List<uint> Pyretic = [960, 639, 3522, 1599, 1133, 1049];

    public static readonly List<uint> Invulns =
    [
        325,  // 无敌：一切攻击都无法造成伤害
        529,  // 无敌：一切攻击都无法造成伤害
        656,  // 无敌：一切攻击都无法造成伤害
        671,  // 无敌：一切攻击都无法造成伤害
        775,  // 无敌：一切攻击都无法造成伤害
        776,  // 无敌：一切攻击都无法造成伤害
        895,  // 无敌：所有攻击均无效化
        969,  // 无敌：一切攻击都无法造成伤害
        981,  // 无敌：一切攻击都无法造成伤害
        1570, // 无敌：一切攻击都无法造成伤害
        1697, // 无敌：一切攻击都无法造成伤害
        1444, // 魔导结界,塔结界在运转，一切攻击都无法造成伤害
        1829, // 无敌：一切攻击都无法造成伤害
        328,  // 土神的心石
        2287, // 纯正神圣领域
        2670, // 冥界行
        3012, // 风神障壁
        3039, // 不死救赎
        3255, // 出死入生
        394,  // 无敌：所有攻击均无效化
      //1125, // 特定方向无敌：令特定方向的攻击无效化
        1567, // 召唤兽的加护：受到了召唤兽的加护，处于暂时无敌的状态
    ];

    public static readonly List<uint> Incapacitated =
    [
        1,    // 石化：手足被石化，无法做出任何行动
        2,    // 眩晕：无法做出任何行动
        3,    // 睡眠：陷入沉睡，无法做出任何行动
        4,    // 昏厥：失去知觉，无法做出任何行动
        5,    // 能力封印：无法使用能力
        6,    // 战技封印：无法发动战技
        7,    // 沉默：无法咏唱魔法
        11,   // 混乱：头脑变得混乱，会向同伴发动攻击
        17,   // 麻痹：身体部分机能出现异常，偶尔会无法行动
        66,   // 恐怖：过于恐惧而无法做出任何行动
        142,  // 眩晕：无法做出任何行动
        149,  // 眩晕：无法做出任何行动
        201,  // 眩晕：无法做出任何行动
        292,  // 拘束：无法做出任何行动，体力逐渐流失
        401,  // 抓捕：被抓住，无法行动，体力逐渐减少
        421,  // 捕食：被吞了下去无法做出任何行动，体力逐渐减少
        487,  // 冻结：身体被冻结无法行动，体力逐渐流失
        609,  // 抓捕：被抓住，无法行动，体力逐渐减少
        961,  // 抓捕：被抓住，无法行动，体力逐渐减少
        1150, // 冻结：身体被冻结无法行动，体力逐渐流失
        1254, // 冻结：身体被冻结无法行动，体力逐渐流失
        1287, // 抓捕：被抓住，无法行动，体力逐渐减少
        1758, // 冻结：身体被冻结无法行动，体力逐渐流失
        2208, // 愤怒的化身：愤怒不已，愤怒状态的玩家无法造成伤害
        2209, // 悲叹的化身：悲叹不已，悲叹状态的玩家无法造成伤害
        2285, // 拘束：无法做出任何行动，体力逐渐流失
        2671, // 青蛙：变成一只青蛙，无法发动任何技能
        3080, // 狂欢：陷入狂欢，抑制不住地想要跳舞，体力逐渐减少
        3287, // 冻结：身体被冻结无法行动，体力逐渐流失
        3519, // 冻结：身体被冻结无法行动，体力逐渐流失
        3547, // 灵魂陷阱_止步：受到粘丝的影响无法做出任何行动，体力逐渐流失
        3697, // 抓捕：被抓住，无法行动，体力逐渐减少
        3165, // 击倒,被击倒在地，无法移动或发动技能
        3501, // 击倒,被击倒在地，无法移动或发动技能
        3730, // 击倒,被击倒在地，无法移动或发动技能
        3908, // 击倒,被击倒在地，无法移动或发动技能
        3909, // 精神失常,精神失常，无法做出任何行动，精神恢复正常后，清除所有仇恨
        3948, // 好脑袋,被换上了好脑袋，移动速度降低，无法发动技能
        3983, // 击倒,被击倒在地，无法移动或发动技能
        4132, // 击倒,被击倒在地，无法移动或发动技能
        514,  // 因果，每次发动技能都会收到伤害
        3085, // 自然的奇迹,陷入变身状态，无法发动技能
        2961, // 击倒,被击倒在地，无法移动或发动技能
        2910, // 击倒,被击倒在地，无法移动或发动技能
        2543, // 软体化,陷入软体化状态。移动速度降低，无法进行跳跃以及发动技能
        2529, // 吸引,被吸引，无法跳跃或发动技能，会朝着特定方向移动
        2505, // 草人化,变成了草人，无法移动或发动技能
        2486, // 吸引,被吸引，无法跳跃或发动技能，会朝着特定方向移动
        2408, // 击倒,被击倒在地，无法移动或发动技能
        2407, // 拘束,无法做出任何行动
        2109, // 无法发动技能,无法发出任何技能
        1963, // 击倒,被击倒在地，无法移动或发动技能
        1953, // 击倒,被击倒在地，无法移动或发动技能
        1950, // 击倒,被击倒在地，无法移动或发动技能
        1785, // 击倒,被击倒在地，无法移动或发动技能
        1762, // 击倒,被击倒在地，无法移动或发动技能
        1462, // 无法发动技能,无法发出任何技能
        1347, // 沉默,无法发动技能
        1113, // 无法发动技能,无法发出任何技能
        939,  // 无法发动技能,无法发出任何技能
        896,  // 击倒,被击倒在地，无法移动或发动技能
        783,  // 击倒,被击倒在地，无法移动或发动技能
        774,  // 击倒,被击倒在地，无法移动或发动技能
        625,  // 击倒,被击倒在地，无法移动或发动技能
        4150, // 冻结,身体被冻结无法行动，体力逐渐流失
        439,  // 蛙变,变成一只青蛙，无法发动任何技能
        441,  // 蛙变,变成一只青蛙，无法发动任何技能
        565,  // 变身,变身成了其他形态，无法发动任何技能
        511,  // 火蛙,变身成蕴藏火焰力量的青蛙，除了“蛙火”这种专用技能之外无法发动任何技能
        644,  // 小鸡,变成了小鸡，无法发动任何技能
        1134, // 河童,变成了河童，无法发动任何技能
        1292, // 波奇,变成了猪，无法发动任何技能
        1546, // 獭獭,变成了獭獭，无法发动任何技能
        2727, // 变身,变身成了其他形态，无法发动任何技能，不会受到特定敌人的攻击
        3502, // 猫头小鹰,变成了猫头小鹰，无法发动任何技能
        996,  // 脑震荡,受到脑震荡的影响，无法行动，且被攻击时所受到的伤害增加
        997,  // 脑震荡,受到脑震荡的影响，无法行动，且被攻击时所受到的伤害增加
        1521, // 昏厥,无法行动，受到的伤害增加
        1522, // 昏厥,失去意识，无法行动
        1990, // 平衡,受到的伤害大幅降低的同时无法行动
        3467, // 魅惑的香光,被香光所魅惑，无法行动
        3479, // 冻结,身体被冻结无法行动，体力逐渐流失
        3480, // 冻结,身体被冻结无法行动，体力逐渐流失
        3481, // 冻结,身体被冻结无法行动，体力逐渐流失
        3513, // 脑震荡,受到脑震荡的影响，无法行动，且被攻击时所受到的伤害增加 
        3549, // 灵魂陷阱：无法行动,受到粘丝的影响无法做出任何行动
    ];

    /// <summary>
    /// 敌人物理攻击无效Buff
    /// </summary>
    public static readonly List<uint> PhysImmune =
    [
        941, // 远程物理攻击无效化：远程物理攻击无法造成伤害
        624, // 584：防御力场：远距离攻击无效化
    ];

    /// <summary>
    /// 敌人魔法攻击无效Buff
    /// </summary>
    public static readonly List<uint> MagicImmune =
    [
        942,  // 魔法攻击无效化：魔法攻击无法造成伤害
        3621, // 魔法攻击无效化：魔法攻击无法造成伤害
        2166, // 魔导结界：吸收魔法攻击并进行反击
    ];

    /// <summary>
    /// 敌人物理反击Buff
    /// </summary>
    public static readonly List<uint> PhysReflect =
    [
        89,   // 复仇：受到攻击的伤害减少并且受到物理攻击时会发动反击
        197,  // 火棘屏障：能够发动火属性反击
        198,  // 冰棘屏障：能够发动冰属性反击，偶尔会追加减速效果
        199,  // 电棘屏障：能够发动雷属性反击，偶尔会追加眩晕效果
        478,  // 水神的面纱：反射远程物理攻击所造成的伤害
        519,  // 反击：所有物理攻击都会反弹到发动者身上
        555,  // 对射：对物理攻击发动反击
        557,  // 对射：对物理攻击发动反击
        870,  // 反推：受到物理攻击时给予对方反击伤害，并且在受到击退效果时，令击退无效的同时击退并击倒对方
        948,  // 雷属性反击：以雷属性伤害反击所受到的一切攻击
        949,  // 火属性反击：以火属性伤害反击所受到的一切攻击
        950,  // 冰属性反击：以冰属性伤害反击所受到的一切攻击
        951,  // 风属性反击：以风属性伤害反击所受到的一切攻击
        952,  // 土属性反击：以土属性伤害反击所受到的一切攻击
        953,  // 水属性反击：以水属性伤害反击所受到的一切攻击
        954,  // 无属性反击：以无属性伤害反击所受到的一切攻击
        1240, // 必杀剑·地天：令自身所受到的伤害减少，同时给予对方反击伤害
        1275, // 非火反射：反射获得了冰属性或雷属性之力的攻击
        1276, // 非冰反射：反射获得了雷属性或火属性之力的攻击
        1277, // 非雷反射：反射获得了火属性或冰属性之力的攻击
        2184, // 还击：受到攻击时会对攻击者造成反击伤害
        2263, // 电棘屏障：能够发动雷属性反击
        2406, // 电棘屏障：能够发动雷属性反击，偶尔会追加眩晕效果
        2450, // 模仿：模仿对手的行动，对于物理攻击将进行物理反击，对于魔法攻击将进行魔法反击
        2528, // 冰棘屏障：能够发动冰属性反击，追加减速效果
        2538, // 盾强化：强化了盾，受到的伤害降低，同时反射来自后背与侧面的攻击
        3051, // 星云：受到攻击的伤害减少，且受到攻击时，给予对方反击伤害
        3631, // 刺阵：在受到物理攻击时，将对攻击者造成反击伤害
        3784, // 狞豹的血潮：受到攻击的伤害减少并且受到物理攻击时会发动反击
        3832, // 戮罪：受到攻击的伤害减少并且受到物理攻击时会发动反击
        4170, // 狞豹的热血：受到攻击的伤害减少并且受到物理攻击时会发动反击
    ];

    /// <summary>
    /// 敌人魔法反击Buff
    /// </summary>
    public static readonly List<uint> MagicReflect =
    [
        342,  // 对射：对魔法攻击发动反击
        477,  // 水神的披风：反射魔法攻击所造成的伤害
        518,  // 反射：所有魔法攻击都会反射到施法者身上
        556,  // 对射：对魔法攻击发动反击
        558,  // 对射：对魔法攻击发动反击
        641,  // 弱化效果反射：将以自身为攻击目标的弱化效果反射给使用者
        948,  // 雷属性反击：以雷属性伤害反击所受到的一切攻击
        949,  // 火属性反击：以火属性伤害反击所受到的一切攻击
        950,  // 冰属性反击：以冰属性伤害反击所受到的一切攻击
        951,  // 风属性反击：以风属性伤害反击所受到的一切攻击
        952,  // 土属性反击：以土属性伤害反击所受到的一切攻击
        953,  // 水属性反击：以水属性伤害反击所受到的一切攻击
        954,  // 无属性反击：以无属性伤害反击所受到的一切攻击
        1275, // 非火反射：反射获得了冰属性或雷属性之力的攻击
        1276, // 非冰反射：反射获得了雷属性或火属性之力的攻击
        1277, // 非雷反射：反射获得了火属性或冰属性之力的攻击
        1649, // 文理反射：自身受到的魔法将被反射给对方
        2166, // 魔导结界：吸收魔法攻击并进行反击
        2337, // 失传反射：自身受到的魔法将被反射给对方
        2450, // 模仿：模仿对手的行动，对于物理攻击将进行物理反击，对于魔法攻击将进行魔法反击
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
            _untargeted = true;
            Core.Resolve<MemApiSpell>().CancelCast();

            //if (check is 1) { Core.Me.SetTarget(Core.Me); }
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
                if (_untargeted && TargetMgr.Instance.EnemysIn20.Count > 0 &&
                        !TargetMgr.Instance.EnemysIn20.Values.First().HasAnyAura(Invulns) &&
                        Core.Me.GetCurrTarget() is null)// &&
                        //Core.Me.GetCurrTarget()!.GameObjectId == Core.Me.GameObjectId)//)
                {
                    if (Debug) LogHelper.Print("Setting Target");
                    Core.Me.SetTarget(TargetMgr.Instance.EnemysIn20.Values.First());
                    _untargeted = false;
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
        if (Core.Me.HasAnyAura(Incapacitated)) { return 2; }
        if (Core.Me.GetCurrTarget() is not null && 
                //!(Core.Me.GetCurrTarget()!.GameObjectId == Core.Me.GameObjectId) &&  <- this does nothing
                Core.Me.GetCurrTarget().HasAnyAura(Invulns)) 
        {
            return 2; 
        }
        return -1;
    }
}
