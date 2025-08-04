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

public class SoulSowHvstMnHK(bool useHPSlot = true) : 
             HotKeyResolver(SpellsDef.Soulsow, SpellTargetType.Target, useHPSlot, false)
{
}
