using ElliotZ.Common;
using System.Numerics;

namespace ElliotZ.Rpr;

public class BattleData
{
    public static BattleData Instance = new();

    /// <summary>
    /// 用于记录gcd复唱时间
    /// </summary>
    public int GcdDuration = 2500;
    public static bool isChange = false;
    public int numBurstPhases = 0;

    /// <summary>
    /// 上一个技能是否为神秘环
    /// </summary>
    public bool justCastAC = false;

    public static void ReBuildSettings()
    {
        if (isChange)
        {
            isChange = false;
            GlobalSetting.Build(RprRotationEntry.SettingsFolderPath, "EZRpr", true);
            RprSettings.Build(RprRotationEntry.SettingsFolderPath);
        }
    }
}
