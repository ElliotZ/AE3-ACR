using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.DynamicComplie;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.FixedSeq;

public class Opener100_3gcd_mk2 : IOpener, ISlotSequence, IScript {
  public List<Action<Slot>> Sequence { get; } = [
      Step0, Step1, Step2, Step3, Step4, Step5, Step6, Step7, Step8, Step9,
      Step10,
  ];

  //public Action CompeltedAction { get; set; }

  public uint Level { get; } = 100u;

  public int StartCheck() {
    if (!16138u.GetSpell().IsReadyWithCanCast()) return -6;

    if (Core.Me.Level < 100) return -5;

    if (!16164u.GetSpell().IsReadyWithCanCast()) return -4;

    return 0;
  }

  public int StopCheck(int index) {
    return -1;
  }

  private static void Step0(Slot slot) {
    LogHelper.Print("KKxb绝枪", "开始3GCD起手改");
    slot.Add(new Spell(16137u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  private static void Step1(Slot slot) {
    slot.Add(new Spell(16139u, SpellTargetType.Target));
    if (Qt.Instance.GetQt("爆发药")) slot.Add2NdWindowAbility(Spell.CreatePotion());
  }

  private static void Step2(Slot slot) {
    slot.Add(new Spell(16145u, SpellTargetType.Target));
    slot.Add2NdWindowAbility(new Spell(16138u, SpellTargetType.Self));
  }

  private static void Step3(Slot slot) {
    slot.Add(new Spell(16153u, SpellTargetType.Target));
    slot.Add(new Spell(16165u, SpellTargetType.Target));
  }

  private static void Step4(Slot slot) {
    slot.Add(new Spell(16146u, SpellTargetType.Target));
    slot.Add(new Spell(16164u, SpellTargetType.Target));
    slot.Add(new Spell(16156u, SpellTargetType.Target));
  }

  private static void Step5(Slot slot) {
    slot.Add(new Spell(25760u, SpellTargetType.Self));
    slot.Add(new Spell(16159u, SpellTargetType.Self));
    slot.Wait2NextGcd = true;
  }

  private static void Step6(Slot slot) {
    slot.Add(new Spell(16147u, SpellTargetType.Target));
    slot.Add(new Spell(16157u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  private static void Step7(Slot slot) {
    slot.Add(new Spell(16150u, SpellTargetType.Target));
    slot.Add(new Spell(16158u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  private static void Step8(Slot slot) {
    slot.Add(new Spell(36937u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  private static void Step9(Slot slot) {
    slot.Add(new Spell(36938u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  private static void Step10(Slot slot) {
    slot.Add(new Spell(36939u, SpellTargetType.Target));
    slot.Wait2NextGcd = true;
  }

  public void InitCountDown(CountDownHandler countDownHandler) {
    if (Qt.Instance.GetQt("自动拉怪")) Qt.Instance.SetQt("自动拉怪", false);

    if (GnbSettings.Instance.起手给MT刚玉) countDownHandler.AddAction(2000, 25758u, SpellTargetType.Pm2);

    if (GnbSettings.Instance.倒计时开大星云) {
      countDownHandler.AddAction(GnbSettings.Instance.大星云Time, 36935u);
    }

    if (GnbSettings.Instance.倒计时开铁壁) countDownHandler.AddAction(GnbSettings.Instance.铁壁Time, 7531u);

    if (GnbSettings.Instance.倒计时自动盾姿) {
      if ((AI.Instance.PartyRole == "MT") && !Core.Me.HasAura(1833u)) {
        LogHelper.Print("KKxb绝枪", "MT未检测到盾姿，正在开启...");
        countDownHandler.AddAction(10000, 16142u);
      } else if (AI.Instance.PartyRole == "ST") {
        if (GnbSettings.Instance.倒计时是否ST关盾姿 && Core.Me.HasAura(1833u)) {
          LogHelper.Print("KKxb绝枪", "ST检测到需关盾，正在关闭...");
          countDownHandler.AddAction(10000, 32068u);
        } else if (!GnbSettings.Instance.倒计时是否ST关盾姿 && !Core.Me.HasAura(1833u)) {
          LogHelper.Print("KKxb绝枪", "ST检测到需开盾，正在开启...");
          countDownHandler.AddAction(10000, 16142u);
        }
      }
    }

    if (Qt.Instance.GetQt("突进起手")) {
      countDownHandler.AddAction(GnbSettings.Instance.Time, 36934u, SpellTargetType.Target);
    }
  }
}
