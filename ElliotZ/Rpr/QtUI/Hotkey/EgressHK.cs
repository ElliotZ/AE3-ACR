using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using ElliotZ.Common;
using System.Numerics;

namespace ElliotZ.Rpr.QtUI.Hotkey;

public class EgressHK(int hktype, bool waitForCD = true) : HotKeyResolver(SpellsDef.HellsEgress, SpellTargetType.Self, false, waitForCD)
{
    private readonly int HkType = hktype;  // 1 - use current direction, 2 - face target, 3 - face camera

    public override void Draw(Vector2 size)
    {
        if (Core.Me.HasAura(AurasDef.RegressReady))
        {
            HotkeyHelper.DrawSpellImage(size, Helper.GetActionChange(SpellId));
        }
        else
        {
            switch (HkType)
            {
                case IngressHK.FaceTarget:
                    HotkeyHelper.DrawSpellImage(size, "../../ACR/ElliotZ/HKImages/egress_t.png");
                    break;
                case IngressHK.FaceCam:
                    HotkeyHelper.DrawSpellImage(size, "../../ACR/ElliotZ/HKImages/egress_cam.png");
                    break;
                default:
                    HotkeyHelper.DrawSpellImage(size, SpellId);
                    break;
            }
        }
    }

    public override int Check()
    {
        if (HkType == IngressHK.FaceTarget && Core.Me.GetCurrTarget() is null) return -9;
        if (Core.Me.HasAura(AurasDef.RegressReady) &&
                IngressHK.RegressPosition().Equals(Vector3.Zero))
        {
            return -8;
        }
        return base.Check();
    }

    protected override async Task Run1(Spell spell, int delay = 0)
    {
        if (delay > 0) await Coroutine.Instance.WaitAsync(delay);
        if (Core.Me.HasAura(AurasDef.RegressReady))
        {
            AI.Instance.BattleData.AddSpell2NextSlot(
                        new Spell(SpellsDef.Regress, IngressHK.RegressPosition()));
        }
        else
        {
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
            AI.Instance.BattleData.AddSpell2NextSlot(SpellsDef.HellsEgress.GetSpell());
        }
    }
}

