using AEAssist;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using JobViewWindow = ElliotZ.ModernJobViewFramework.JobViewWindow;

namespace EZACR_Offline.Gnb.QtUI;

public static class DevTab {
  public static void Build(JobViewWindow instance) {
    instance.AddTab("Dev",
                    _ => {
                      ImGui.Text($"爆发药：{Qt.Instance.GetQt("爆发药")}");
                      ImGui.Text($"gcd是否可用：{GCDHelper.CanUseGCD()}");
                      ImGui.Text($"gcd剩余时间：{GCDHelper.GetGCDCooldown()}");
                      ImGui.Text($"gcd总时间：{GCDHelper.GetGCDDuration()}");
                      ImGui.Text($"小队人数：{PartyHelper.CastableParty.Count}");
                      ImGui.Text($"小队坦克数量：{PartyHelper.CastableTanks.Count}");
                      ImGui.Text($"小队DPS数量：{PartyHelper.CastableDps.Count}");
                      ImGui.Text($"小队奶妈数量：{PartyHelper.CastableHealers.Count}");
                      ImGui.Separator();
                      ImGui.TextColored(new Vector4(0.2f, 0.8f, 0.8f, 1f),
                                        "当前起手式: " + GetOpenerName(GnbSettings.Instance.opener));

                      try {
                        var memApiCountdown = Core.Resolve<MemApiCountdown>();

                        if (memApiCountdown != null) {
                          float num = memApiCountdown.TimeRemaining();

                          if (num > 0f) {
                            ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.2f, 1f),
                                              $"倒计时剩余: {num:F1} 秒");
                          } else {
                            ImGui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, 1f), "倒计时未激活");
                          }
                        } else {
                          ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), "倒计时模块未加载");
                        }
                      } catch (Exception ex) {
                        ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), "倒计时读取失败: " + ex.Message);
                      }

                      try {
                        bool flag = Core.Me.HasAura(1833u);
                        ImGui.TextColored(
                            flag ? new Vector4(0f, 1f, 0f, 1f) : new Vector4(1f, 0f, 0f, 1f),
                            "盾姿状态: " + (flag ? "已开启" : "未开启"));
                      } catch (Exception ex2) {
                        ImGui.TextColored(new Vector4(1f, 0f, 0f, 1f), "盾姿检测失败: " + ex2.Message);
                      }

                      ImGui.Separator();
                      ImGui.TextColored(new Vector4(0.8f, 0.8f, 0.2f, 1f), "当前盾姿配置状态:");
                      ImGui.Text("自动盾姿(仅日随外起手判断): ");
                      ImGui.SameLine();
                      ImGui.TextColored(GnbSettings.Instance.倒计时自动盾姿
                                            ? new Vector4(0f, 1f, 0f, 1f)
                                            : new Vector4(1f, 0f, 0f, 1f),
                                        GnbSettings.Instance.倒计时自动盾姿 ? "已启用" : "已禁用");
                      ImGui.Text("ST自动关盾: ");
                      ImGui.SameLine();
                      ImGui.TextColored(GnbSettings.Instance.倒计时是否ST关盾姿
                                            ? new Vector4(0f, 1f, 0f, 1f)
                                            : new Vector4(1f, 0f, 0f, 1f),
                                        GnbSettings.Instance.倒计时是否ST关盾姿 ? "已启用" : "已禁用");
                      ImGui.TreePop();
                    });
  }

  private static string GetOpenerName(int opener) {
    string result = opener switch {
        0 => "日随（无固定起手）",
        1 => "100级2.46 2G循环",
        2 => "100级2.5 2G循环改",
        3 => "100级2.46 1G循环改",
        4 => "100级2.5 3G循环改",
        5 => "80级 2G血壤绝亚起手",
        6 => "90级 1G血壤无情绝欧起手",
        7 => "70级 5G无情绝神兵起手",
        8 => "100级2.5 1G快速起手",
        9 => "70级 2G无情起手",
        _ => "空起手",
    };

    return result;
  }
}
