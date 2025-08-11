using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Rdm;

// 直接定义好 方便编码
public static class QTKey
{
    public const string 爆发 = "爆发",
                        最终爆发 = "最终爆发",
                        输出即刻 = "输出即刻",
                        起手序列 = "起手序列",
                        AOE = "AOE",
                        魔元化 = "魔元化",
                        鼓励 = "鼓励",
                        拉人 = "拉人",
                        自动昏乱 = "自动昏乱",
                        交剑 = "交剑",
                        短兵 = "短兵",
                        自奶 = "自奶",

                        魔六 = "魔六",
                        锅炉圣人 = "锅炉圣人",
                        保留促进 = "保留促进",
                        短交留一层 = "短交留一层",
                        老年圣人 = "老年圣人",
                        飞刺六分 = "飞刺六分",
                        强制魔元化 = "强制魔元化",
                        对齐GCD = "对齐GCD",

                        范围拉人 = "范围拉人",
                        移动即刻 = "移动即刻",
                        小停一下 = "小停一下",
                        自动停手 = "自动停手";
}
public static class QT
{
    // 假设 RedMageRotationEntry 是一个已经定义好的类
    // 并且它有一个静态属性 QT，该属性有一个 GetQt 方法
    public static bool QTGET(string qtName)
    {
        try
        {
            // 尝试调用 RedMageRotationEntry.QT 的 GetQt 方法
            return RedMageRotationEntry.QT.GetQt(qtName);
        }
        catch (KeyNotFoundException ex)
        {
            // 如果 qtName 不存在，则记录错误并返回一个默认值
            //LogHelper.Error($"QT 设置 '{qtName}' 没有找到: {ex.Message}");
            return false; // 或者根据实际情况返回一个合适的默认值
        }
        catch (Exception ex)
        {
            // 捕获其他所有异常，并记录错误
            //LogHelper.Error($"An error occurred while getting QT setting '{qtName}': {ex.Message}");
            // 可以选择抛出异常，或者返回一个默认值
            //throw; // 重新抛出异常，让调用者处理
            // 或者
            return false; // 返回默认值
        }
    }
    public static bool QTSET(string qtName, bool qtValue) => RedMageRotationEntry.QT.SetQt(qtName, qtValue);
}
