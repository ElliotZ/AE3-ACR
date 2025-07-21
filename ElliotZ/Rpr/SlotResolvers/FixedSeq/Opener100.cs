using AEAssist;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.Helper;
using AEAssist.JobApi;
using ElliotZ.Common;
using ElliotZ.Rpr.QtUI;

namespace ElliotZ.Rpr.SlotResolvers.FixedSeq;

public class Opener100 : IOpener
{
    public void InitCountDown(CountDownHandler cdh)
    {
        Qt.Reset();

        const int startTime = 15000;
        cdh.AddAction(startTime - 13300, () => SpellsDef.Harpe.GetSpell());
    }

    private class SecondGcdAc : ISlotSequence
    {
        public List<Action<Slot>> Sequence { get; } =
            [
            Step1,
            ];
    }
}
