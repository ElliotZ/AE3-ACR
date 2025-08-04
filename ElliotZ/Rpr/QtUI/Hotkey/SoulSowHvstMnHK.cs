using AEAssist.CombatRoutine;
using ElliotZ.Common;

namespace ElliotZ.Rpr.QtUI.Hotkey;

public class SoulSowHvstMnHK(bool useHPSlot = true) :
             HotKeyResolver(SpellsDef.Soulsow, SpellTargetType.Target, useHPSlot, false)
{
}
