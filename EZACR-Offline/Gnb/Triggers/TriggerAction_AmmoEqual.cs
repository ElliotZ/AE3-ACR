using AEAssist;
using AEAssist.CombatRoutine.Trigger;
using AEAssist.GUI;
using AEAssist.JobApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.Triggers;

public class TriggerAction_AmmoEqual : ITriggerCond, ITriggerBase
{
    [LabelName("子弹数量(只匹配此数量)")]
    public int Red { get; set; }

    public string DisplayName { get; } = "GNB/检测量谱-子弹为(只匹配此数量)";


    public string Remark { get; set; }

    public bool Draw()
    {
        return false;
    }

    public static int 绝枪量谱_子弹数()
    {
        return Core.Resolve<JobApi_GunBreaker>().Ammo;
    }

    public bool Handle(ITriggerCondParams triggerCondParams)
    {
        return 绝枪量谱_子弹数() == Red;
    }
}
