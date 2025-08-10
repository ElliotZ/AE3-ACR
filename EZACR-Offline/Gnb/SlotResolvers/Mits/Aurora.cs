using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class Aurora : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!16151u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        if (Core.Me.HasAura(1835u))
        {
            return -8;
        }

        List<uint> auras = new List<uint> { 3255u, 409u, 810u };
        if (!Qt.Instance.GetQt("减伤"))
        {
            return -5;
        }

        if (16151u.RecentlyUsed())
        {
            return -15;
        }

        if (Core.Me.CurrentHpPercent() < GnbSettings.Instance.极光阈值 && Core.Me.HasAnyAura(auras, 3000))
        {
            return -6;
        }

        if (Core.Me.CurrentHpPercent() > GnbSettings.Instance.极光阈值)
        {
            return -9;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16151u.GetSpell());
    }
}
