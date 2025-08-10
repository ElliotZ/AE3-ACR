using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class Provoke : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!Qt.Instance.GetQt("减伤"))
        {
            return -101;
        }

        if ((DateTime.Now - BattleData.Instance.lastFalseTime).TotalSeconds <= 5.0)
        {
            return -2;
        }

        if (!7533u.GetSpell().IsReadyWithCanCast())
        {
            return -50;
        }

        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 25f)
        {
            return -10;
        }

        if (Qt.Instance.GetQt("自动拉怪") && Core.Me.GetCurrTarget().CurrentHpPercent() == 1f && !Core.Me.GetCurrTarget().IsMe())
        {
            return 10;
        }

        if (!Qt.Instance.GetQt("自动拉怪") && Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 0f)
        {
            return -5;
        }

        return -1;
    }

    public void Build(Slot slot)
    {
        slot.Add(7533u.GetSpell());
    }
}
