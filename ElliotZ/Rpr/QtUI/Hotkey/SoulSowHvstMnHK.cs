using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ElliotZ.Rpr.QtUI.Hotkey;

public class SoulSowHvstMnHK : IHotkeyResolver
{
    private readonly uint SpellId;
    //private readonly SpellTargetType TargetType;
    private readonly bool UseHighPrioritySlot;
    private readonly bool WaitCoolDown;

    /// <summary>
    /// 只使用不卡gcd的强插
    /// </summary>
    public SoulSowHvstMnHK(bool useHighPrioritySlot = true, bool waitCoolDown = true)
    {
        SpellId = SpellsDef.Soulsow;
        //TargetType = targetType;
        UseHighPrioritySlot = useHighPrioritySlot;
        WaitCoolDown = waitCoolDown;
    }

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, SpellId);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        var targetSpellId = Core.Resolve<MemApiSpell>().CheckActionChange(SpellId);
        var spell = targetSpellId.GetSpell();

        if (WaitCoolDown)
        {
            if (spell.Cooldown.TotalMilliseconds <= 5000.0)
            {
                if (isActive)
                {
                    HotkeyHelper.DrawActiveState(size);
                }
                else
                {
                    HotkeyHelper.DrawGeneralState(size);
                }
            }
            else
            {
                HotkeyHelper.DrawDisabledState(size);
            }

            HotkeyHelper.DrawCooldownText(spell, size);
            HotkeyHelper.DrawChargeText(spell, size);
        }
        else
        {
            SpellHelper.DrawSpellInfo(spell, size, isActive);
        }
    }

    public int Check()
    {
        var s = SpellId.GetSpell();
        var isReady = WaitCoolDown ? s.Cooldown.TotalMilliseconds <= 5000 : s.IsReadyWithCanCast();
        return isReady ? 0 : -2;
    }

    public void Run()
    {
        var targetSpellId = Core.Resolve<MemApiSpell>().CheckActionChange(SpellId);
        var spell = targetSpellId.GetSpell();
        var cooldown = spell.Cooldown.TotalMilliseconds;

        if (WaitCoolDown && cooldown > 0)
        {
            _ = Run1(spell, (int)cooldown);
        }
        else
        {
            _ = Run1(spell);
        }
    }

    private async Task Run1(Spell spell, int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);

        if (UseHighPrioritySlot &&
                !RprSettings.Instance.ForceNextSlotsOnHKs &&
                Core.Me.GetCurrTarget() is not null &&
                Core.Me.InCombat())
        {
            var slot = new Slot();
            slot.Add(spell);
            if (spell.IsAbility())
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
            }
            else
            {
                AI.Instance.BattleData.HighPrioritySlots_GCD.Enqueue(slot);
            }

        }
        else
        {
            //var gcdCooldown = GCDHelper.GetGCDCooldown();
            //if (gcdCooldown is < 700 and > 0)
            //{
            //    _ = Run2(spell, gcdCooldown + 100);
            //}
            //else
            //{
            //    _ = Run2(spell);
            //}
            AI.Instance.BattleData.AddSpell2NextSlot(spell);
        }
    }

    //private static async Task Run2(Spell spell, int delay = 0)
    //{
    //    if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
    //    AI.Instance.BattleData.AddSpell2NextSlot(spell);
    //}
}
