using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class Base : ISlotResolver
{
    private static uint PrevCombo => Core.Resolve<MemApiSpell>().GetLastComboSpellId();
    private static uint
        st_1 = SpellsDef.Slice,
        st_2 = SpellsDef.WaxingSlice,
        st_3 = SpellsDef.InfernalSlice,
        aoe_1 = SpellsDef.SpinningScythe,
        aoe_2 = SpellsDef.NightmareScythe;

    public int Check()
    {
        if (Core.Me.Distance(Core.Me.GetCurrTarget()) > SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -1;
        }
        return 0;
    }

    private static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (enemyCount >= 3 && aoe_1.IsUnlock() && PrevCombo != st_2 && PrevCombo != st_1) // TODO: add AOE QT
        {
            if (PrevCombo == aoe_1) return aoe_2;
            return aoe_1;
        }

        if (st_3.IsUnlock() && PrevCombo == st_2) return st_3;
        if (st_2.IsUnlock() && PrevCombo == st_1) return st_2;
        return st_1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
