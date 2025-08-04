using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.GCD;

public class Base : ISlotResolver
{
    //private static uint PrevCombo => Core.Resolve<MemApiSpell>().GetLastComboSpellId();
    private static readonly uint
        st_1 = SpellsDef.Slice,
        st_2 = SpellsDef.WaxingSlice,
        st_3 = SpellsDef.InfernalSlice,
        aoe_1 = SpellsDef.SpinningScythe,
        aoe_2 = SpellsDef.NightmareScythe;

    public int Check()
    {
        if (Core.Me.Distance(Core.Me.GetCurrTarget()) > Helper.GlblSettings.AttackRange)
        {
            return -2;  // -2 for not in range
        }
        if (Helper.GetActionChange(SpellsDef.BloodStalk).RecentlyUsed() ||
                    SpellsDef.Gluttony.RecentlyUsed())
        {
            return -10;
        }
        return 0;
    }

    public static uint Solve()
    {
        var enemyCount = TargetHelper.GetNearbyEnemyCount(5);

        if (Qt.Instance.GetQt("AOE") &&
            enemyCount >= 3 &&
            aoe_1.GetSpell().IsReadyWithCanCast() &&
            RprHelper.PrevCombo != st_2 &&
            RprHelper.PrevCombo != st_1)
        {
            if (aoe_2.GetSpell().IsReadyWithCanCast() && RprHelper.PrevCombo == aoe_1) { return aoe_2; }
            return aoe_1;
        }

        if (st_3.GetSpell().IsReadyWithCanCast() && RprHelper.PrevCombo == st_2) { return st_3; }
        if (st_2.GetSpell().IsReadyWithCanCast() && RprHelper.PrevCombo == st_1) { return st_2; }
        return st_1;
    }

    public void Build(Slot slot)
    {
        slot.Add(Solve().GetSpell());
    }
}
