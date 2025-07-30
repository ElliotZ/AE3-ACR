using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using System.Numerics;

namespace ElliotZ.Rpr.QtUI.Hotkey;

public class EgressHK : IHotkeyResolver
{
    private readonly uint SpellId;
    //private readonly SpellTargetType TargetType;
    //private readonly bool UseHighPrioritySlot;
    private readonly int HkType;  // 1 - use current direction, 2 - face target, 3 - face camera
    private readonly bool WaitCoolDown;

    /// <summary>
    /// 只使用不卡gcd的强插
    /// </summary>
    public EgressHK(int hktype, bool waitForCD = true)
    {
        SpellId = SpellsDef.HellsEgress;
        HkType = hktype;
        //TargetType = targetType;
        //UseHighPrioritySlot = false;
        WaitCoolDown = waitForCD;
    }

    public void Draw(Vector2 size)
    {
        switch (HkType)
        {
            case 2: 
                HotkeyHelper.DrawSpellImage(size, "../../ACR/ElliotZ/HKImages/egress_t.png");
                break;
            case 3:
                HotkeyHelper.DrawSpellImage(size, "../../ACR/ElliotZ/HKImages/egress_cam.png");
                break;
            default:
                HotkeyHelper.DrawSpellImage(size, SpellId);
                break;
        }
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
        if (HkType == 2 && Core.Me.GetCurrTarget() is null) return -9;
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
        switch (HkType)
        {
            case 2:
                Core.Resolve<MemApiMoveControl>().Stop();
                Core.Resolve<MemApiMove>().SetRot(Helper.GetRotationToTarget(Core.Me.Position,
                                                                     Core.Me.GetCurrTarget()!.Position));
                break;
            case 3:
                Core.Resolve<MemApiMoveControl>().Stop();
                Core.Resolve<MemApiMove>().SetRot(CameraHelper.GetCameraRotation());
                break;
            default:
                break;
        }
        AI.Instance.BattleData.AddSpell2NextSlot(spell);
        //if (UseHighPrioritySlot &&
        //        !RprSettings.Instance.ForceNextSlotsOnHKs &&
        //        Core.Me.GetCurrTarget() is not null &&
        //        Core.Me.InCombat())
        //{
        //    var slot = new Slot();
        //    slot.Add(spell);
        //    if (spell.IsAbility())
        //    {
        //        AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
        //    }
        //    else
        //    {
        //        AI.Instance.BattleData.HighPrioritySlots_GCD.Enqueue(slot);
        //    }

        //}
        //else
        //{
        //    //var gcdCooldown = GCDHelper.GetGCDCooldown();
        //    //if (gcdCooldown is < 700 and > 0)
        //    //{
        //    //    _ = Run2(spell, gcdCooldown + 100);
        //    //}
        //    //else
        //    //{
        //    //    _ = Run2(spell);
        //    //}
        //    AI.Instance.BattleData.AddSpell2NextSlot(spell);
        //}
    }

    //private static async Task Run2(Spell spell, int delay = 0)
    //{
    //    if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
    //    AI.Instance.BattleData.AddSpell2NextSlot(spell);
    //}
}

