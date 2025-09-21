using AEAssist.Helper;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.Text.SeStringHandling;
using EZACR_Offline.PvP.Brd;

namespace EZACR_Offline.PvP;

public class 职业配置 : BaseJobConfig {
/*  public void 配置龙骑技能()
  {
    this.权限获取();
    PvPDRGSettings.Instance.药血量 = Math.Clamp(PvPDRGSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPDRGSettings.Instance.药血量, 3, 10, 77);
    this.ConfigureSkillBool(29490U, "樱花缭乱", "仅龙血时使用", ref PvPDRGSettings.Instance.樱花缭乱龙血, 11);
    职业配置.自定义.死者之岸();
    this.ConfigureSkillBoolInt(29493U, "高跳", "使用时目标距离多少米以内", "仅龙血才使用", ref PvPDRGSettings.Instance.高跳龙血, ref PvPDRGSettings.Instance.高跳范围, 1, 2, 12);
    this.ConfigureSkillBool(29489U, "苍穹刺", "仅龙血才能使用", ref PvPDRGSettings.Instance.苍穹刺龙血, 15);
    职业配置.自定义.后跳();
    PvPDRGSettings.Instance.天龙点睛主目标距离 = Math.Clamp(PvPDRGSettings.Instance.天龙点睛主目标距离, 0, 25);
    this.ConfigureSkillBoolInt(29495U, "天龙点睛", "使用时至少距离目标多少米", "仅龙血才使用", ref PvPDRGSettings.Instance.天龙龙血, ref PvPDRGSettings.Instance.天龙点睛主目标距离, 1, 5, 13);
    PvPDRGSettings.Instance.恐惧咆哮人数 = Math.Clamp(PvPDRGSettings.Instance.恐惧咆哮人数, 1, 48 /*0x30#1#);
    this.ConfigureSkillBoolInt(29496U, "恐惧咆哮", "使用时周围敌人人数", "仅龙血才使用", ref PvPDRGSettings.Instance.恐惧咆哮x龙血, ref PvPDRGSettings.Instance.恐惧咆哮人数, 1, 2, 14);
    PvPDRGSettings.Instance.Save();
  }

  public void 配置机工技能()
  {
    this.权限获取();
    PvPMCHSettings.Instance.药血量 = Math.Clamp(PvPMCHSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPMCHSettings.Instance.药血量, 5, 10, 77);
    this.ConfigureSkillBool(29405U, "钻头", "仅分析使用", ref PvPMCHSettings.Instance.钻头分析, 1);
    this.ConfigureSkillBool(29406U, "毒菌冲击", "仅分析使用", ref PvPMCHSettings.Instance.毒菌分析, 2);
    this.ConfigureSkillBool(29407U, "空气锚", "仅分析使用", ref PvPMCHSettings.Instance.空气锚分析, 3);
    this.ConfigureSkillBool(29408U, "回转飞锯", "仅分析使用", ref PvPMCHSettings.Instance.回转飞锯分析, 4);
    this.ConfigureSkillBool(29414U, "分析", "钻头套装可用才使用分析", ref PvPMCHSettings.Instance.分析可用, 5);
    this.ConfigureSkillBool(29409U, "野火", "仅过热状态时使用", ref PvPMCHSettings.Instance.过热野火, 6);
    this.ConfigureSkillBool(41469U, "全金属爆发", "仅野火期间使用", ref PvPMCHSettings.Instance.金属爆发仅野火, 12);
    this.ConfigureSkillBool(41468U, "烈焰弹(整活)", "使用旧版热冲击\n(只会降低2000威力)", ref PvPMCHSettings.Instance.热冲击, 10);
    this.ConfigureSkillBool(29415U, "魔弹射手(热键)", "使用智能目标", ref PvPMCHSettings.Instance.智能魔弹, 7);
    PvPMCHSettings.Instance.Save();
  }

  public void 配置画家技能()
  {
    this.权限获取();
    PvPPCTSettings.Instance.药血量 = Math.Clamp(PvPPCTSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPPCTSettings.Instance.药血量, 3, 10, 77);
    PvPPCTSettings.Instance.盾自身血量 = Math.Clamp(PvPPCTSettings.Instance.盾自身血量, 0.01f, 1f);
    this.ConfigureSkillSliderFloat(39211U, "坦培拉涂层", "自身血量", ref PvPPCTSettings.Instance.盾自身血量, 0.01f, 1f, 1);
    this.ConfigureSkillInt(39213U, "减色混合", "最低切换间隔(毫秒)", ref PvPPCTSettings.Instance.减色切换, 100, 1000, 76);
    PvPPCTSettings.Instance.Save();
  }

  public void 配置赤魔技能()
  {
    this.权限获取();
    PvPRDMSettings.Instance.药血量 = Math.Clamp(PvPRDMSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPRDMSettings.Instance.药血量, 3, 10, 77);
    职业配置.自定义.剑身强部();
    PvPRDMSettings.Instance.鼓励人数 = Math.Clamp(PvPRDMSettings.Instance.鼓励人数, 1, 8);
    this.ConfigureSkillInt(41494U, "鼓励", "30米内队友人数", ref PvPRDMSettings.Instance.鼓励人数, 1, 2, 556);
    this.ConfigureSkillBool(41495U, "光芒四射", "仅鼓励期间释放", ref PvPRDMSettings.Instance.鼓励光芒四射, 90);
    this.ConfigureSkillBool(41492U, "决断", "仅鼓励期间释放", ref PvPRDMSettings.Instance.鼓励决断, 95);
    this.ConfigureSkillBool(41498U, "南天十字(热键)", "以自己为目标", ref PvPRDMSettings.Instance.南天自己, 7);
    PvPRDMSettings.Instance.Save();
  }

  public void 配置黑魔技能()
  {
    this.权限获取();
    PvPBLMSettings.Instance.药血量 = Math.Clamp(PvPBLMSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPBLMSettings.Instance.药血量, 3, 10, 77);
    职业配置.自定义.磁暴();
    PvPBLMSettings.Instance.昏沉 = Math.Clamp(PvPBLMSettings.Instance.昏沉, 0, 1000);
    this.ConfigureSkillInt(41510U, "昏沉(插入时机)", "数值越低越优先\n推荐0", ref PvPBLMSettings.Instance.昏沉, 10, 100, 1414);
    this.ConfigureSkillBool(41475U, "元素天赋(默认烈火环)", "开烈火环时不会排除龟壳\n允许寒冰环", ref PvPBLMSettings.Instance.寒冰环, 1112);
    this.ConfigureSkillInt(29663U, "冰火切换(冰判定时间)", "数值越低打的冰越少\n推荐50~300", ref PvPBLMSettings.Instance.冰时间, 10, 100, 1412);
    this.ConfigureSkilldescription(29662U, "灵魂共鸣(技能逻辑说明)", "LB需要手动按\n仅自动耀星(有QT开关)\n仅使用核爆\n移动时仅打冰连击");
    PvPBLMSettings.Instance.Save();
  }

  public void 配置武士技能()
  {
    this.权限获取();
    ImGui.Checkbox("斩铁日志调试模式", ref PvPSAMSettings.Instance.斩铁调试);
    if (PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数) != null && PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数) != Core.Me)
      ImGui.Text($"多斩目标：{PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数)}");
    if (PVPTargetHelper.TargetSelector.Get斩铁目标() != null && PVPTargetHelper.TargetSelector.Get斩铁目标() != Core.Me)
      ImGui.Text($"斩铁目标：{PVPTargetHelper.TargetSelector.Get斩铁目标().Name}");
    ImGui.Separator();
    PvPSAMSettings.Instance.药血量 = Math.Clamp(PvPSAMSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "喝热水", "热水阈值", ref PvPSAMSettings.Instance.药血量, 3, 10, 5);
    this.ConfigureSkillBool(29530U, "斩浪&雪月花", "移动时不读条", ref PvPSAMSettings.Instance.读条检查, 1);
    职业配置.自定义.地天();
    职业配置.自定义.斩铁();
    PvPSAMSettings.Instance.Save();
  }*/

  public void 配置诗人技能() {
    权限获取();
    PvPBrdSettings.Instance.药血量 = Math.Clamp(PvPBrdSettings.Instance.药血量, 1, 100);
    ConfigureSkillInt(29711U,
                      "喝热水",
                      "热水阈值",
                      ref PvPBrdSettings.Instance.药血量,
                      3,
                      10,
                      5);
    PvPBrdSettings.Instance.和弦箭 = Math.Clamp(PvPBrdSettings.Instance.和弦箭, 1, 4);
    ConfigureSkillInt(41464U,
                      "和弦箭",
                      "使用层数",
                      ref PvPBrdSettings.Instance.和弦箭,
                      1,
                      1,
                      87);
    自定义.光阴神();
    PvPBrdSettings.Instance.Save();
  }

/*  public void 配置召唤技能()
  {
    this.权限获取();
    PvPSMNSettings.Instance.药血量 = Math.Clamp(PvPSMNSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "癒しの水", "热水阈值", ref PvPSMNSettings.Instance.药血量, 3, 10, 1);
    PvPSMNSettings.Instance.火神冲 = Math.Clamp(PvPSMNSettings.Instance.火神冲, 0, 28);
    this.ConfigureSkillInt(29667U, "猛き炎", "火神冲敌人距离", ref PvPSMNSettings.Instance.火神冲, 1, 5, 2);
    PvPSMNSettings.Instance.溃烂阈值 = Math.Clamp(PvPSMNSettings.Instance.溃烂阈值, 0.1f, 1f);
    this.ConfigureSkillSliderFloat(41483U, "坏死爆发", "敌人血量阈值", ref PvPSMNSettings.Instance.溃烂阈值, 0.1f, 1f, 3);
    职业配置.自定义.守护之光();
    PvPSMNSettings.Instance.Save();
  }

  public void 配置绝枪技能()
  {
    this.权限获取();
    ImGui.Text("当前职业职能技能推荐使用:铁壁");
    PvPGNBSettings.Instance.药血量 = Math.Clamp(PvPGNBSettings.Instance.药血量, 1, 100);
    this.ConfigureSkillInt(29711U, "自愈", "自愈阈值", ref PvPGNBSettings.Instance.药血量, 3, 10, 1);
    PvPGNBSettings.Instance.粗分斩最大距离 = Math.Clamp(PvPGNBSettings.Instance.粗分斩最大距离, 0, 20);
    this.ConfigureSkillInt(29123U, "粗分斩", "离目标()米以内使用", ref PvPGNBSettings.Instance.粗分斩最大距离, 1, 5, 2);
    PvPGNBSettings.Instance.爆破血量 = Math.Clamp(PvPGNBSettings.Instance.爆破血量, 1, 100);
    this.ConfigureSkillInt(29128U, "爆破领域", "目标血量多少才使用", ref PvPGNBSettings.Instance.爆破血量, 3, 10, 3);
    职业配置.自定义.刚玉之心();
    PvPGNBSettings.Instance.Save();
  }*/

  public class 自定义 {
/*    public static void 死者之岸()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29492U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (死者之岸));
      ImGui.Text("有樱花缭乱时不用");
      ImGui.Checkbox($"##{2}", ref PvPDRGSettings.Instance.死者之岸樱花);
      ImGui.Text("有天龙点睛时不用");
      ImGui.Checkbox($"##{3}", ref PvPDRGSettings.Instance.死者之岸天龙);
      ImGui.Text("有苍穹刺时不用");
      ImGui.Checkbox($"##{4}", ref PvPDRGSettings.Instance.死者之岸苍穹刺);
      ImGui.Columns(1);
    }

    public static void 磁暴()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29657U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (磁暴));
      PvPBLMSettings.Instance.磁暴敌人距离 = Math.Clamp(PvPBLMSettings.Instance.磁暴敌人距离, 5, 10);
      PvPBLMSettings.Instance.磁暴敌人数量 = Math.Clamp(PvPBLMSettings.Instance.磁暴敌人数量, 1, 48 /*0x30#1#);
      ImGui.Text("多少米范围内");
      ImGui.InputInt($"##{88}", ref PvPBLMSettings.Instance.磁暴敌人距离, 1, 5);
      ImGui.Text("存在敌人数量");
      ImGui.InputInt($"##{99}", ref PvPBLMSettings.Instance.磁暴敌人数量, 1, 5);
      ImGui.Columns(1);
    }

    public static void 剑身强部()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(41496U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (剑身强部));
      PvPRDMSettings.Instance.护盾距离 = Math.Clamp(PvPRDMSettings.Instance.护盾距离, 0, 30);
      PvPRDMSettings.Instance.护盾人数 = Math.Clamp(PvPRDMSettings.Instance.护盾人数, 1, 48 /*0x30#1#);
      ImGui.Text("多少米范围内");
      ImGui.InputInt($"##{8787}", ref PvPRDMSettings.Instance.护盾距离, 1, 5);
      ImGui.Text("存在敌人数量");
      ImGui.InputInt($"##{6565}", ref PvPRDMSettings.Instance.护盾人数, 1, 5);
      ImGui.Columns(1);
    }

    public static void 后跳()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29494U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (后跳));
      ImGui.Text("被控时主动使用后跳");
      ImGui.Checkbox($"##{55}", ref PvPDRGSettings.Instance.后跳解除控制);
      ImGui.Text("往镜头方向后跳");
      ImGui.Checkbox($"##{68}", ref PvPDRGSettings.Instance.后跳面向);
      ImGui.Columns(1);
    }

    public static void 地天()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29533U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (地天));
      PvPSAMSettings.Instance.周围敌人数量 = Math.Clamp(PvPSAMSettings.Instance.周围敌人数量, 1, 48 /*0x30#1#);
      ImGui.Text("10米内人数大于");
      ImGui.InputInt($"##{55}", ref PvPSAMSettings.Instance.周围敌人数量, 1, 100);
      PvPSAMSettings.Instance.地天自身血量 = Math.Clamp(PvPSAMSettings.Instance.地天自身血量, 0.01f, 1f);
      ImGui.Text("自身血量小于");
      ImGui.SliderFloat($"##{58}", ref PvPSAMSettings.Instance.地天自身血量, 0.01f, 1f);
      ImGui.Columns(1);
    }

    public static void 斩铁()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29537U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 200f);
      ImGui.Text(nameof (斩铁));
      ImGui.Text("确认不被控再使用");
      ImGui.Checkbox($"##{41}", ref PvPSAMSettings.Instance.斩铁检查状态);
      ImGui.Text("多斩(推荐去用PvPAuto)");
      ImGui.Checkbox($"##{42}", ref PvPSAMSettings.Instance.多斩模式);
      PvPSAMSettings.Instance.多斩人数 = Math.Clamp(PvPSAMSettings.Instance.多斩人数, 2, 48 /*0x30#1#);
      ImGui.Text("多斩人数");
      ImGui.InputInt($"##{43}", ref PvPSAMSettings.Instance.多斩人数, 1, 100);
      ImGui.Columns(1);
    }*/

    public static void 光阴神() {
      ImGui.Separator();
      ImGui.Columns(2, (string)null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29400U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof(光阴神));
      ImGui.Text("解控队友|聊天框打印:");
      PvPBrdSettings.Instance.光阴对象 = Math.Clamp(PvPBrdSettings.Instance.光阴对象,
                                                0,
                                                PartyHelper.Party.Count - 1);
      ImGui.Checkbox($"##{1}", ref PvPBrdSettings.Instance.光阴队友);
      ImGui.SameLine();
      ImGui.Checkbox($"##{54}", ref PvPBrdSettings.Instance.光阴播报);
      ImGui.Text("优先玩家名");
      ImGui.InputText($"##{678}", ref PvPBrdSettings.Instance.优先对象, 10);
      IBattleChara? battleChara =
          PartyHelper.Party.FirstOrDefault(x => x?.Name?.TextValue == PvPBrdSettings.Instance.优先对象);
      ImGui.Text("优先对象:");

      if (battleChara != null) {
        int num = PartyHelper.Party.IndexOf(battleChara);
        PvPBrdSettings.Instance.光阴对象 = num;
        ImGui.SameLine();

        if ((PartyHelper.Party.Count > 0)
         && (PvPBrdSettings.Instance.光阴对象 >= 0)
         && (PvPBrdSettings.Instance.光阴对象 < PartyHelper.Party.Count)) {
          ImGui.Text($"{PartyHelper.Party[PvPBrdSettings.Instance.光阴对象]?.Name ?? "未存在此玩家"}");
        } else {
          ImGui.Text("未存在此玩家");
        }
      } else {
        ImGui.InputInt($"##{78}", ref PvPBrdSettings.Instance.光阴对象, 1, 100);
        PvPBrdSettings.Instance.光阴对象 = Math.Clamp(PvPBrdSettings.Instance.光阴对象,
                                                  0,
                                                  PartyHelper.Party.Count - 1);
        ImGui.SameLine();

        if ((PartyHelper.Party.Count > 0)
         && (PvPBrdSettings.Instance.光阴对象 >= 0)
         && (PvPBrdSettings.Instance.光阴对象 < PartyHelper.Party.Count)) {
          ImGui.Text($"{PartyHelper.Party[PvPBrdSettings.Instance.光阴对象]?.Name ?? "未存在此玩家"}");
        } else {
          ImGui.Text("未存在此玩家");
        }

        if (ImGui.Button("优先玩家名设定该对象")) {
          PvPBrdSettings.Instance.优先对象 = PartyHelper.Party[PvPBrdSettings.Instance.光阴对象].Name
                                                    .TextValue;
        }
      }

      ImGui.Columns();
    }

/*    public static void 守护之光()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(29670U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (守护之光));
      PvPSMNSettings.Instance.守护之光血量 = Math.Clamp(PvPSMNSettings.Instance.守护之光血量, 1, 101);
      ImGui.InputInt($"血量阈值##{6666}", ref PvPSMNSettings.Instance.守护之光血量, 10, 20);
      ImGui.Text("守护队友|聊天框打印:");
      PvPSMNSettings.Instance.守护对象 = Math.Clamp(PvPSMNSettings.Instance.守护对象, 0, PartyHelper.Party.Count - 1);
      ImGui.Checkbox($"##{45454}", ref PvPSMNSettings.Instance.守护队友);
      ImGui.SameLine();
      ImGui.Checkbox($"##{54}", ref PvPSMNSettings.Instance.守护播报);
      ImGui.Text("优先玩家名");
      ImGui.InputText($"##{678}", ref PvPSMNSettings.Instance.优先对象, 10U);
      IBattleChara battleChara = PartyHelper.Party.FirstOrDefault<IBattleChara>((Func<IBattleChara, bool>) (x => x?.Name?.TextValue == PvPSMNSettings.Instance.优先对象));
      if (battleChara != null)
      {
        ImGui.Text("优先对象:");
        int num = PartyHelper.Party.IndexOf(battleChara);
        PvPSMNSettings.Instance.守护对象 = num;
        ImGui.SameLine();
        if (PartyHelper.Party != null && PartyHelper.Party.Count > 0 && PvPSMNSettings.Instance.守护对象 >= 0 && PvPSMNSettings.Instance.守护对象 < PartyHelper.Party.Count)
          ImGui.Text($"{PartyHelper.Party[PvPSMNSettings.Instance.守护对象]?.Name ?? (SeString) "未存在此玩家"}");
        else
          ImGui.Text("未存在此玩家");
      }
      else
      {
        ImGui.Text("优先对象:");
        ImGui.InputInt($"##{78}", ref PvPSMNSettings.Instance.守护对象, 1, 100);
        PvPSMNSettings.Instance.守护对象 = Math.Clamp(PvPSMNSettings.Instance.守护对象, 0, PartyHelper.Party.Count - 1);
        ImGui.SameLine();
        if (PartyHelper.Party != null && PartyHelper.Party.Count > 0 && PvPSMNSettings.Instance.守护对象 >= 0 && PvPSMNSettings.Instance.守护对象 < PartyHelper.Party.Count)
          ImGui.Text($"{PartyHelper.Party[PvPSMNSettings.Instance.守护对象]?.Name ?? (SeString) "未存在此玩家"}");
        else
          ImGui.Text("未存在此玩家");
        if (ImGui.Button("优先玩家名设定该对象"))
          PvPSMNSettings.Instance.优先对象 = PartyHelper.Party[PvPSMNSettings.Instance.守护对象].Name.TextValue;
      }
      ImGui.Columns(1);
    }

    public static void 刚玉之心()
    {
      ImGui.Separator();
      ImGui.Columns(2, (string) null, false);
      ImGui.SetColumnWidth(0, 70f);
      PVPHelper.技能图标(41443U);
      ImGui.NextColumn();
      ImGui.SetColumnWidth(1, 150f);
      ImGui.Text(nameof (刚玉之心));
      PvPGNBSettings.Instance.刚玉血量 = Math.Clamp(PvPGNBSettings.Instance.刚玉血量, 1, 101);
      ImGui.InputInt($"血量阈值##{6666}", ref PvPGNBSettings.Instance.刚玉血量, 10, 20);
      ImGui.Text("刚玉队友|聊天框打印:");
      PvPGNBSettings.Instance.刚玉对象 = Math.Clamp(PvPGNBSettings.Instance.刚玉对象, 0, PartyHelper.Party.Count - 1);
      ImGui.Checkbox($"##{45454}", ref PvPGNBSettings.Instance.刚玉队友);
      ImGui.SameLine();
      ImGui.Checkbox($"##{54}", ref PvPGNBSettings.Instance.刚玉播报);
      ImGui.Text("优先玩家名");
      ImGui.InputText($"##{678}", ref PvPGNBSettings.Instance.优先对象, 10U);
      IBattleChara battleChara = PartyHelper.Party.FirstOrDefault<IBattleChara>((Func<IBattleChara, bool>) (x => x?.Name?.TextValue == PvPGNBSettings.Instance.优先对象));
      if (battleChara != null)
      {
        ImGui.Text("优先对象:");
        int num = PartyHelper.Party.IndexOf(battleChara);
        PvPGNBSettings.Instance.刚玉对象 = num;
        ImGui.SameLine();
        if (PartyHelper.Party != null && PartyHelper.Party.Count > 0 && PvPGNBSettings.Instance.刚玉对象 >= 0 && PvPGNBSettings.Instance.刚玉对象 < PartyHelper.Party.Count)
          ImGui.Text($"{PartyHelper.Party[PvPGNBSettings.Instance.刚玉对象]?.Name ?? (SeString) "未存在此玩家"}");
        else
          ImGui.Text("未存在此玩家");
      }
      else
      {
        ImGui.Text("优先对象:");
        ImGui.InputInt($"##{78}", ref PvPGNBSettings.Instance.刚玉对象, 1, 100);
        PvPGNBSettings.Instance.刚玉对象 = Math.Clamp(PvPGNBSettings.Instance.刚玉对象, 0, PartyHelper.Party.Count - 1);
        ImGui.SameLine();
        if (PartyHelper.Party != null && PartyHelper.Party.Count > 0 && PvPGNBSettings.Instance.刚玉对象 >= 0 && PvPGNBSettings.Instance.刚玉对象 < PartyHelper.Party.Count)
          ImGui.Text($"{PartyHelper.Party[PvPGNBSettings.Instance.刚玉对象]?.Name ?? (SeString) "未存在此玩家"}");
        else
          ImGui.Text("未存在此玩家");
        if (ImGui.Button("优先玩家名设定该对象"))
          PvPGNBSettings.Instance.优先对象 = PartyHelper.Party[PvPGNBSettings.Instance.刚玉对象].Name.TextValue;
      }
      ImGui.Columns(1);
    }*/
  }
}
