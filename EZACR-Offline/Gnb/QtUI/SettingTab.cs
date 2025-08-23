using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.GUI;
using JobViewWindow = ElliotZ.Common.ModernJobViewFramework.JobViewWindow;
using ImGuiNET;

namespace EZACR_Offline.Gnb.QtUI;

public static class SettingTab
{
    public static void Build(JobViewWindow instance)
    {
        instance.AddTab("设置", window =>
        {
            ImGui.Text("ACR设置");
            var previewValue = GnbSettings.Instance.ACRMode switch
            {
                "Normal" => "日随",
                "HighEnd1" => "100级2.46 2G循环",
                "HighEnd2" => "100级2.5 2G循环改",
                "HighEnd3" => "100级2.46 1G循环改",
                "HighEnd4" => "100级2.5 3G循环改",
                "HighEnd5" => "100级2.5 1G快速起手",
                "绝亚2G" => "80级 2G血壤绝亚起手",
                "绝欧1G" => "90级 1G血壤无情绝欧起手",
                "神兵5G" => "70级 5G无情绝神兵起手",
                "70级2G" => "70级 2G无情起手",
                _ => ""
            };

            if (ImGui.BeginCombo("循环选择", previewValue))
            {
                if (ImGui.Selectable("日随"))
                {
                    GnbSettings.Instance.ACRMode = "Normal";
                    GnbSettings.Instance.opener = 0;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("100级2.46 2G循环"))
                {
                    GnbSettings.Instance.ACRMode = "HighEnd1";
                    GnbSettings.Instance.opener = 1;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("100级2.5 2G循环改"))
                {
                    GnbSettings.Instance.ACRMode = "HighEnd2";
                    GnbSettings.Instance.opener = 2;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("100级2.5 1G快速起手"))
                {
                    GnbSettings.Instance.ACRMode = "HighEnd5";
                    GnbSettings.Instance.opener = 8;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("100级2.46 1G循环改"))
                {
                    GnbSettings.Instance.ACRMode = "HighEnd3";
                    GnbSettings.Instance.opener = 3;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("100级2.5 3G循环改"))
                {
                    GnbSettings.Instance.ACRMode = "HighEnd4";
                    GnbSettings.Instance.opener = 4;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("80级 2G血壤绝亚起手"))
                {
                    GnbSettings.Instance.ACRMode = "绝亚2G";
                    GnbSettings.Instance.opener = 5;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("90级 1G血壤无情绝欧起手"))
                {
                    GnbSettings.Instance.ACRMode = "绝欧1G";
                    GnbSettings.Instance.opener = 6;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("70级 5G无情绝神兵起手"))
                {
                    GnbSettings.Instance.ACRMode = "神兵5G";
                    GnbSettings.Instance.opener = 7;
                    GnbSettings.Instance.Save();
                }

                if (ImGui.Selectable("70级 2G无情起手"))
                {
                    GnbSettings.Instance.ACRMode = "70级2G";
                    GnbSettings.Instance.opener = 9;
                    GnbSettings.Instance.Save();
                }

                ImGui.EndCombo();
            }
            ImGui.Checkbox("日随停手", ref GnbSettings.Instance.HandleStopMechs);
            ImGui.Checkbox("小怪留爆发", ref GnbSettings.Instance.NoBurst);
            ImGui.Separator();
            //ImGui.Checkbox("自动重置QT", ref GnbSettings.Instance.RestoreQtSet);
            //ImGui.SameLine();
            if (ImGui.Button("记录当前QT设置"))
            {
                Qt.SaveQtStates();
            }
            ImGui.Text("会从当前记录过的QT设置重置，爆发药、爆发药2分、智能AOE以及自动突进这几个QT不会被重置。");
            ImGui.Separator();
            ImGui.Text("点击按钮设置为高难默认设置（超火减伤默认关闭 突进默认开启） ");
            if (ImGui.Button("高难设置"))
            {
                Qt.Instance.NewDefault("突进起手", newDefault: true);
                Qt.Instance.SetQt("突进起手", qtValue: true);
                Qt.Instance.NewDefault("自动超火", newDefault: false);
                Qt.Instance.SetQt("自动超火", qtValue: false);
                Qt.Instance.NewDefault("减伤", newDefault: false);
                Qt.Instance.SetQt("减伤", qtValue: false);
                Qt.Instance.NewDefault("雪仇", newDefault: false);
                Qt.Instance.SetQt("雪仇", qtValue: false);
                Qt.Instance.NewDefault("自动刚玉", newDefault: false);
                Qt.Instance.SetQt("自动刚玉", qtValue: false);
                Qt.Instance.NewDefault("音速破", newDefault: true);
                Qt.Instance.SetQt("音速破", qtValue: true);
                Qt.Instance.NewDefault("弓形冲波", newDefault: true);
                Qt.Instance.SetQt("弓形冲波", qtValue: true);
                Qt.Instance.NewDefault("狮心连", newDefault: true);
                Qt.Instance.SetQt("狮心连", qtValue: true);
                Qt.Instance.NewDefault("爆发击", newDefault: true);
                Qt.Instance.SetQt("爆发击", qtValue: true);
                Qt.Instance.NewDefault("子弹连", newDefault: true);
                Qt.Instance.SetQt("子弹连", qtValue: true);
                Qt.Instance.NewDefault("爆发", newDefault: true);
                Qt.Instance.SetQt("爆发", qtValue: true);
                Qt.Instance.NewDefault("无情", newDefault: true);
                Qt.Instance.SetQt("无情", qtValue: true);
                Qt.Instance.NewDefault("强制变命运", newDefault: false);
                Qt.Instance.SetQt("强制变命运", qtValue: false);
                Qt.Instance.NewDefault("强制爆发击", newDefault: false);
                Qt.Instance.SetQt("强制爆发击", qtValue: false);
                Qt.Instance.NewDefault("优先音速破", newDefault: false);
                Qt.Instance.SetQt("优先音速破", qtValue: false);
                Qt.Instance.NewDefault("TP开怪", newDefault: false);
                Qt.Instance.SetQt("TP开怪", qtValue: false);
                Qt.Instance.NewDefault("血壤", newDefault: true);
                Qt.Instance.SetQt("血壤", qtValue: true);
                Qt.Instance.NewDefault("倍攻", newDefault: true);
                Qt.Instance.SetQt("倍攻", qtValue: true);
                Qt.Instance.NewDefault("爆破领域", newDefault: true);
                Qt.Instance.SetQt("爆破领域", qtValue: true);
                Qt.Instance.NewDefault("使用基础Gcd", newDefault: true);
                Qt.Instance.SetQt("使用基础Gcd", qtValue: true);
                Qt.Instance.NewDefault("极光", newDefault: false);
                Qt.Instance.SetQt("极光", qtValue: false);
                Qt.Instance.NewDefault("自动拉怪", newDefault: false);
                Qt.Instance.SetQt("自动拉怪", qtValue: false);
                if (GnbSettings.Instance.ACRMode == "HighEnd1" || GnbSettings.Instance.ACRMode == "HighEnd3")
                {
                    Qt.Instance.NewDefault("无情后半", newDefault: true);
                    Qt.Instance.SetQt("无情后半", qtValue: true);
                }

                GnbSettings.Instance.Save();
            }

            ImGui.Text("点击按钮设置为普通默认设置（超火减伤默认开启 突进默认关闭）");
            if (ImGui.Button("普通本设置"))
            {
                Qt.Instance.NewDefault("突进起手", newDefault: false);
                Qt.Instance.SetQt("突进起手", qtValue: false);
                Qt.Instance.NewDefault("自动超火", newDefault: true);
                Qt.Instance.SetQt("自动超火", qtValue: true);
                Qt.Instance.NewDefault("减伤", newDefault: true);
                Qt.Instance.SetQt("减伤", qtValue: true);
                Qt.Instance.NewDefault("音速破", newDefault: true);
                Qt.Instance.SetQt("音速破", qtValue: true);
                Qt.Instance.NewDefault("雪仇", newDefault: true);
                Qt.Instance.SetQt("雪仇", qtValue: true);
                Qt.Instance.NewDefault("自动刚玉", newDefault: true);
                Qt.Instance.SetQt("自动刚玉", qtValue: true);
                Qt.Instance.NewDefault("音速破", newDefault: true);
                Qt.Instance.SetQt("音速破", qtValue: true);
                Qt.Instance.NewDefault("弓形冲波", newDefault: true);
                Qt.Instance.SetQt("弓形冲波", qtValue: true);
                Qt.Instance.NewDefault("狮心连", newDefault: true);
                Qt.Instance.SetQt("狮心连", qtValue: true);
                Qt.Instance.NewDefault("爆发击", newDefault: true);
                Qt.Instance.SetQt("爆发击", qtValue: true);
                Qt.Instance.NewDefault("子弹连", newDefault: true);
                Qt.Instance.SetQt("子弹连", qtValue: true);
                Qt.Instance.NewDefault("爆发", newDefault: true);
                Qt.Instance.SetQt("爆发", qtValue: true);
                Qt.Instance.NewDefault("无情", newDefault: true);
                Qt.Instance.SetQt("无情", qtValue: true);
                Qt.Instance.NewDefault("强制变命运", newDefault: false);
                Qt.Instance.SetQt("强制变命运", qtValue: false);
                Qt.Instance.NewDefault("强制爆发击", newDefault: false);
                Qt.Instance.SetQt("强制爆发击", qtValue: false);
                Qt.Instance.NewDefault("优先音速破", newDefault: false);
                Qt.Instance.SetQt("优先音速破", qtValue: false);
                Qt.Instance.NewDefault("TP开怪", newDefault: false);
                Qt.Instance.SetQt("TP开怪", qtValue: false);
                Qt.Instance.NewDefault("血壤", newDefault: true);
                Qt.Instance.SetQt("血壤", qtValue: true);
                Qt.Instance.NewDefault("倍攻", newDefault: true);
                Qt.Instance.SetQt("倍攻", qtValue: true);
                Qt.Instance.NewDefault("爆破领域", newDefault: true);
                Qt.Instance.SetQt("爆破领域", qtValue: true);
                Qt.Instance.NewDefault("使用基础Gcd", newDefault: true);
                Qt.Instance.SetQt("使用基础Gcd", qtValue: true);
                Qt.Instance.NewDefault("极光", newDefault: true);
                Qt.Instance.SetQt("极光", qtValue: true);
                GnbSettings.Instance.Save();
            }

            var v = GnbSettings.Instance.起手给MT刚玉;
            ImGui.Checkbox("是否默认起手2s刚玉PM2", ref v);
            if (v != GnbSettings.Instance.起手给MT刚玉)
            {
                GnbSettings.Instance.起手给MT刚玉 = v;
                GnbSettings.Instance.Save();
            }

            var v2 = GnbSettings.Instance.倒计时自动盾姿;
            ImGui.Checkbox("是否自动盾姿-基于时间轴职能设置", ref v2);
            if (v2 != GnbSettings.Instance.倒计时自动盾姿)
            {
                GnbSettings.Instance.倒计时自动盾姿 = v2;
                GnbSettings.Instance.Save();
            }

            var v3 = GnbSettings.Instance.倒计时是否ST关盾姿;
            ImGui.Checkbox("是否ST起手关盾-需求前置自动盾姿", ref v3);
            if (v3 != GnbSettings.Instance.倒计时是否ST关盾姿)
            {
                GnbSettings.Instance.倒计时是否ST关盾姿 = v3;
                GnbSettings.Instance.Save();
            }

            var v4 = GnbSettings.Instance.倒计时开大星云;
            ImGui.Checkbox("是否默认倒数开星云", ref v4);
            if (v4 != GnbSettings.Instance.倒计时开大星云)
            {
                GnbSettings.Instance.倒计时开大星云 = v4;
                GnbSettings.Instance.Save();
            }

            var v5 = GnbSettings.Instance.倒计时开铁壁;
            ImGui.Checkbox("是否默认倒数开铁壁", ref v5);
            if (v5 != GnbSettings.Instance.倒计时开铁壁)
            {
                GnbSettings.Instance.倒计时开铁壁 = v5;
                GnbSettings.Instance.Save();
            }

            if (ImGui.CollapsingHeader("阈值设置"))
            {
                ImGuiHelper.LeftInputInt("突进时间(倒数多少ms突进)建议400", ref GnbSettings.Instance.Time, 100, 10000, 100);
                ImGuiHelper.LeftInputInt("大星云时间(倒数多少ms开)", ref GnbSettings.Instance.大星云Time, 100, 10000, 100);
                ImGuiHelper.LeftInputInt("铁壁时间(倒数多少ms开)", ref GnbSettings.Instance.铁壁Time, 100, 10000, 100);
                ImGui.Text("减伤包括自动极光 需要开启才会使用");
                ImGui.Text("除刚玉外的减伤血量阈值只在自动拉怪QT关闭时生效");
                ImGui.Text("自动超火开启后达到阈值会自动释放");
                ImGui.Text("按住Ctrl左键单击滑块可以直接输入数字");
                if (ImGui.SliderFloat("铁壁阈值", ref GnbSettings.Instance.铁壁阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                if (ImGui.SliderFloat("伪装阈值", ref GnbSettings.Instance.伪装阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                if (ImGui.SliderFloat("星云阈值", ref GnbSettings.Instance.星云阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                if (ImGui.SliderFloat("超火流星阈值", ref GnbSettings.Instance.超火流星阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                if (ImGui.SliderFloat("极光阈值", ref GnbSettings.Instance.极光阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                if (ImGui.SliderFloat("刚玉之心阈值", ref GnbSettings.Instance.刚玉之心阈值, 0f, 0.99f))
                {
                    GnbSettings.Instance.Save();
                }

                ImGui.Text("点击此按钮设置为默认阈值设置");
                if (ImGui.Button("默认设置"))
                {
                    GnbSettings.Instance.铁壁阈值 = 0.82f;
                    GnbSettings.Instance.伪装阈值 = 0.79f;
                    GnbSettings.Instance.超火流星阈值 = 0.2f;
                    GnbSettings.Instance.极光阈值 = 0.8f;
                    GnbSettings.Instance.星云阈值 = 0.8f;
                    GnbSettings.Instance.刚玉之心阈值 = 0.8f;
                    Qt.Instance.NewDefault("自动超火", newDefault: false);
                    Qt.Instance.SetQt("自动超火", qtValue: false);
                    Qt.Instance.NewDefault("减伤", newDefault: true);
                    Qt.Instance.SetQt("减伤", qtValue: true);
                    Qt.Instance.NewDefault("自动刚玉", newDefault: true);
                    Qt.Instance.SetQt("自动刚玉", qtValue: true);
                    Qt.Instance.NewDefault("倾泻爆发", newDefault: false);
                    Qt.Instance.SetQt("倾泻爆发", qtValue: false);
                    GnbSettings.Instance.Save();
                }
            }

            try
            {
                if (ImGui.CollapsingHeader("拉怪设置"))
                {
                    ImGui.Text("自动拉怪设置:");
                    ImGui.Text("自动拉怪QT开启时，只会使用基础连击(单体与AOE)、闪雷弹、刚玉之心。");
                    ImGui.Text("当你拉到位站在原地不动一定时间后，会自动关闭自动拉怪QT（你也可以选择手动关闭）。开始正常使用输出与减伤技能。");
                    var v6 = GnbSettings.Instance.自动拉怪;
                    ImGui.Checkbox("是否默认打开自动拉怪", ref v6);
                    if (v6 != GnbSettings.Instance.自动拉怪)
                    {
                        GnbSettings.Instance.自动拉怪 = v6;
                        Qt.Instance.NewDefault("自动拉怪", v6);
                        Qt.Instance.SetQt("自动拉怪", v6);
                        GnbSettings.Instance.Save();
                    }

                    var v7 = GnbSettings.Instance.自动拉怪停止时间;
                    if (ImGui.SliderFloat("站定多少秒后关闭拉怪模式：", ref v7, 1f, 4f))
                    {
                        GnbSettings.Instance.自动拉怪停止时间 = v7;
                        GnbSettings.Instance.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UI异常: " + ex.Message);
            }

            if (!ImGui.CollapsingHeader("插入技能状态")) return;
            if (ImGui.Button("清除队列"))
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD.Clear();
                AI.Instance.BattleData.HighPrioritySlots_GCD.Clear();
            }

            ImGui.SameLine();
            if (!ImGui.Button("清除一个")) return;
            if (AI.Instance.BattleData.HighPrioritySlots_OffGCD.Count > 0)
            {
                AI.Instance.BattleData.HighPrioritySlots_OffGCD.Dequeue();
            }
            if (AI.Instance.BattleData.HighPrioritySlots_GCD.Count > 0)
            {
                AI.Instance.BattleData.HighPrioritySlots_GCD.Dequeue();
            }
        });
    }
}