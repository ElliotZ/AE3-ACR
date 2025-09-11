using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.Module.Opener;
using AEAssist.DynamicComplie;
using AEAssist.Extension;
using AEAssist.Helper;
using EZACR_Offline.Gnb.QtUI;

namespace EZACR_Offline.Gnb.SlotResolvers.FixedSeq;

public class Opener90_1gcd_OPU : IOpener, ISlotSequence, IScript {
  public List<Action<Slot>> Sequence { get; } = [Step0, Step1];

  //public Action CompeltedAction { get; set; }

  public uint Level { get; } = 90u;

  public int StartCheck() {
    if (!16138u.GetSpell().IsReadyWithCanCast()) return -6;

    if (Core.Me.Level < 90) return -5;

    if (!16164u.GetSpell().IsReadyWithCanCast()) return -4;

    return 0;
  }

  public int StopCheck(int index) {
    return -1;
  }

  private static void Step0(Slot slot) {
    LogHelper.Print("KKxb绝枪", "开始绝欧1G血壤无情起手");
    slot.Add(new Spell(16137u, SpellTargetType.Target));
    slot.Add(new Spell(16164u, SpellTargetType.Target));
    slot.Add(new SlotAction(SlotAction.WaitType.WaitForSndHalfWindow, 0, 16138u.GetSpell()));
  }

  private static void Step1(Slot slot) {
    slot.Add(new Spell(16139u, SpellTargetType.Target));
  }

  public void InitCountDown(CountDownHandler countDownHandler) {
    if (Qt.Instance.GetQt("自动拉怪")) Qt.Instance.SetQt("自动拉怪", false);

    LogHelper.Print($"角色职能: {
      AI.Instance.PartyRole
    }, 盾姿状态: {
      Core.Me.HasAura(1833u)
    }, 自动盾姿配置: {
      GnbSettings.Instance.倒计时自动盾姿
    }, ST关盾配置: {
      GnbSettings.Instance.倒计时是否ST关盾姿
    }");
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
