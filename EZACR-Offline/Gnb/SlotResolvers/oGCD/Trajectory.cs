using AEAssist;
using AEAssist.CombatRoutine;
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

namespace EZACR_Offline.Gnb.SlotResolvers.oGCD;

public class Trajectory : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!Qt.Instance.GetQt("突进无理由"))
        {
            return -50;
        }

        if (!36934u.GetSpell().IsReadyWithCanCast())
        {
            return -15;
        }

        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 20f)
        {
            return -10;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(36934u.GetSpell());
    }
}
