using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.DynamicComplie;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.FixedSeq;

public class Opener80_2gcd_TEA : IOpener, ISlotSequence, IScript {
  public List<Action<Slot>> Sequence { get; } = [Step0, Step1, Step2, Step3];

  //public Action CompeltedAction { get; set; }

  public uint Level { get; } = 80u;

  public int StartCheck() {
    if (!16138u.GetSpell().IsReadyWithCanCast()) return -6;

    if (Core.Me.Level < 80) return -5;

    if (!16164u.GetSpell().IsReadyWithCanCast()) return -4;

    return 0;
  }

  public int StopCheck(int index) {
    return -1;
  }

  private static void Step0(Slot slot) {
    LogHelper.Print("KKxb绝枪", "开始绝亚血壤起手");
    slot.Add(new Spell(16164u, SpellTargetType.Target));
  }

  private static void Step1(Slot slot) {
    slot.Add(new Spell(16137u, SpellTargetType.Target));
    if (Qt.Instance.GetQt("爆发药")) slot.Add(Spell.CreatePotion());

    if (Core.Me.HasAura(1833u)) slot.Add(new Spell(32068u, SpellTargetType.Self));
  }

  private static void Step2(Slot slot) {
    slot.Add(new Spell(16139u, SpellTargetType.Target));
    slot.Add(new Spell(16138u, SpellTargetType.Self));
    slot.Add(new Spell(16165u, SpellTargetType.Target));
  }

  private static void Step3(Slot slot) {
    slot.Add(new Spell(16146u, SpellTargetType.Target));
    slot.Add(new Spell(16156u, SpellTargetType.Target));
  }

  public void InitCountDown(CountDownHandler countDownHandler) {
    if (Qt.Instance.GetQt("自动拉怪")) Qt.Instance.SetQt("自动拉怪", false);

    if (GnbSettings.Instance.倒计时开铁壁) countDownHandler.AddAction(GnbSettings.Instance.铁壁Time, 7531u);

    if (GnbSettings.Instance.倒计时自动盾姿) {
      if (!Core.Me.HasAura(1833u) && (AI.Instance.PartyRole == "MT")) {
        countDownHandler.AddAction(10000, 16142u);
      }

      if (!Core.Me.HasAura(1833u)
       && (AI.Instance.PartyRole == "ST")
       && !GnbSettings.Instance.倒计时是否ST关盾姿) {
        countDownHandler.AddAction(10000, 16142u);
      }

      if (Core.Me.HasAura(1833u)
       && (AI.Instance.PartyRole == "ST")
       && GnbSettings.Instance.倒计时是否ST关盾姿) {
        countDownHandler.AddAction(10000, 32068u);
      }
    }

    if (Qt.Instance.GetQt("突进起手")) {
      countDownHandler.AddAction(GnbSettings.Instance.Time, 36934u, SpellTargetType.Target);
    }
  }
}
