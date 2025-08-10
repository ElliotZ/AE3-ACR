using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class RoyalGuard : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!Qt.Instance.GetQt("减伤"))
        {
            return -101;
        }

        if (PartyHelper.Party.Count > 4)
        {
            return -7;
        }

        if (!16142u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        if (16142u.RecentlyUsed())
        {
            return -15;
        }

        if (Core.Me.HasAura(1833u))
        {
            return -50;
        }

        if (!Core.Me.HasAura(1833u))
        {
            if (GnbSettings.Instance.ACRMode == "Normal")
            {
                return 0;
            }

            if (GnbSettings.Instance.ACRMode == "HighEnd1")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "HighEnd2")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "HighEnd3")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "HighEnd4")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "HighEnd5")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "绝亚2G")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "绝欧1G")
            {
                return -15;
            }

            if (GnbSettings.Instance.ACRMode == "神兵5G")
            {
                return -15;
            }
        }

        if (Core.Resolve<MemApiSpell>().CheckActionChange(16142u) == 32068)
        {
            return -40;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16142u.GetSpell());
    }
}
