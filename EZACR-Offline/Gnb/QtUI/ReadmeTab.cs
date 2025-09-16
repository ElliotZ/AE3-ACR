using AEAssist.GUI;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Colors;
using JobViewWindow = ElliotZ.ModernJobViewFramework.JobViewWindow;

namespace EZACR_Offline.Gnb.QtUI;

public static class ReadmeTab {
  private static readonly InfoBox _box = new() {
      AutoResize = true,
      BorderColor = ImGuiColors.ParsedGold,
      ContentsAction = () => {
        ImGui.Text("本ACR尽量按2.5GCD/2.46GCD? ");
        ImGui.Text("7.1更新后建议使用本版本");
        ImGui.Text("悬浮窗增加自动开盾姿及ST不自动关选项");
        ImGui.Text("模式为日随时 开启减伤QT 4人小队自动打开盾姿 ");
        ImGui.Text("突进时间 建议400左右 默认和最低改为100  每次加减改为100");
        ImGui.Text("除刚玉外的减伤血量阈值只在自动拉怪QT关闭时生效");
        ImGui.Text("有问题DC@KKxb ");
        ImGui.Text("2.5G 如果要开优化偏移 测试能用的设置是全局不卡GCD/提前使用G-80 延迟5 ");
        ImGui.Text("注意：");
        ImGui.Text("本ACR不支持处理队友抢仇自动抢回来");
        ImGui.Text("如果日随遇到喜欢提前跑老远拉怪的,拉怪了喜欢跟T拉两边的,骂你拉的慢的 请在聊天输出此目标 ");
        ImGui.Text("喜欢低等级狠狠多拉一波然后死了的 请更换其他ACR使用");
        ImGui.Text("8.5更新 增加70级2G无情起手-1G爆发药");
        ImGui.Text("7.11更新 2G无情前子弹数保留2变为1");
        ImGui.Text("7.4更新 尝试增加面板倒计时开铁壁大星云");
        ImGui.Text("5.5更新 增加悬浮窗增加自动开盾姿及ST不自动关选项（倒计时一次正常一次不正常处理 不知缘由）");
        ImGui.Text("4.13更新 增加优先音速破QT--开启后优先音速破");
        ImGui.Text("4.13更新 增加强制爆发击--开启后有子弹就打爆发击卸掉子弹  增加爆破领域QT-包括危险 ");
        ImGui.Text("4.12更新 修复部分导致锁倍攻等QT时开启无视无情QT 在倍攻转好时断子弹连的问题 ");
        ImGui.Text("4.11更新 增加QT-倍攻/血壤 爆发击‘不开AOE的QT在双目标状况’开强制变命运也打命运之环 ");
        ImGui.Text("4.10更新 增加新时间轴QT 增加无视无情QT 增加TP(等级限制*疑似暂不可用)和1G起手倒数2s给 PM2刚玉 ");
        ImGui.Text("2.13更新 ");
        ImGui.Text("11.27更新 尝试修复血壤前不打爆发击导致血壤延后");
        ImGui.Text("11.12更新 倍攻现在消耗只1颗子弹 逻辑尝试改变 有问题反馈");
        ImGui.Text("11.8更新 读基础设置删了 Try修复尴尬卡死状况");
        ImGui.Text("11.5更新 现在关闭爆发QT也会自己卸爆发击了");
        ImGui.Text("11.1更新 改了大量代码修复释放测试");
        ImGui.Text("10.31更新 热键一键减伤测试(关闭自动减伤使用)");
        ImGui.Text("10.27更新 增加QT控制爆发药中卸子弹打爆发击");
        ImGui.Text("10.27更新 修复闪雷最远释放距离限制");
        ImGui.Text("10.25更新 增加选中自己来停手热键");
        ImGui.Text("10.23更新 增加70绝神兵5G无情起手选择-Nagomi需求");
        ImGui.Text("10.23更新 测试时间轴起手选择 好像没啥用");
        ImGui.Text("10.22更新 增加对热风加速度炸弹的停手");
        ImGui.Text("10.17更新 修复自动拉怪炸UI情况");
        ImGui.Text("10.16更新 增加90绝欧1G血壤无情起手选择-YBB需求");
        ImGui.Text("10.14更新 修复满晶壤后连击溢出问题");
        ImGui.Text("10.13更新 减伤大概如愿使用 先开星云 然后伪装铁壁 雪仇亲疏");
        ImGui.Text("10.11更新 减伤做了一个更改 不在移动中/周围目标你1仇大于3才开");
        ImGui.Text("10.11更新 减伤判断优先开启星云 后开伪装和铁壁 雪仇QT关联雪仇和亲疏自行减伤");
        ImGui.Text("10.11更新 起手加了个血壤开的2G绝亚特化-Nagomi需求");
        ImGui.Text("10.8更新 拉怪QT大概会自动开吧 闪雷弹释放距离现在是AE设置攻击距离+2");
        ImGui.Text("10.2更新 修复可能子弹连打断狮心的情况");
        ImGui.Text("10.1版本 增加自动拉怪QT 开启打只AOE 闪雷 挑衅 停止后开始技能输出");
        ImGui.Text("10.1版本 日随循环自动开盾现在只会在4人小队开启了");
        ImGui.TreePop();
      },
  };

  public static void Build(JobViewWindow instance) {
    instance.AddTab("README",
                    window => {
                      ImGui.Dummy(new System.Numerics.Vector2(0, 1));
                      ImGui.Dummy(new System.Numerics.Vector2(5, 0));
                      ImGui.SameLine();
                      _box.DrawStretched();
                    });
  }
}
