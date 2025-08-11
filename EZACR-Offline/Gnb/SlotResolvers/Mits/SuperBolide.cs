using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.Mits;

public class SuperBolide : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.OffGcd;

    public int Check()
    {
        if (!16152u.GetSpell().IsReadyWithCanCast())
        {
            return -3;
        }

        List<uint> auras = [3255u, 409u, 810u];
        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 992)
        {
            return -66;
        }

        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 1168)
        {
            return -67;
        }

        if (Core.Resolve<MemApiMap>().GetCurrTerrId() == 922)
        {
            return -68;
        }

        if (!Qt.Instance.GetQt("自动超火"))
        {
            return -5;
        }

        if (Core.Me.CurrentHpPercent() < GnbSettings.Instance.超火流星阈值 && Core.Me.HasAura(3255u) && Core.Me.HasAura(810u) && Core.Me.HasAnyAura(auras, 3000))
        {
            return -3;
        }

        if (Core.Me.CurrentHpPercent() > GnbSettings.Instance.超火流星阈值)
        {
            return -5;
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        slot.Add(16152u.GetSpell());
    }

}
