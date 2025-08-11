using ElliotZ.Common;

namespace EZACR_Offline.Gnb;

public class GnbHelper
{
    public static readonly string TxtCmdHandle = "/EZGnb";

    /// <summary>
    /// buffs for continuation
    /// </summary>
    public static readonly List<uint> ContBuffs = [AurasDef.ReadytoBlast,
                                                   AurasDef.ReadyToRaze,
                                                   AurasDef.ReadytoRip,
                                                   AurasDef.ReadytoGouge,
                                                   AurasDef.ReadytoTear];
}
