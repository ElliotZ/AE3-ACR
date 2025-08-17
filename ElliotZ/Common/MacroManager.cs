using AEAssist.Helper;
using ECommons;
using ElliotZ.Common.ModernJobViewFramework;
using ImGuiNET;
using System.Numerics;


namespace ElliotZ.Common;  // 改成你需要的Namespace，或者由IDE自动处理

/// <summary>
/// 宏命令控制Hotkey和QT的（希望算是比较易用的）轮子
/// </summary>
/// <param name="instance">QT窗口的参照</param>
/// <param name="cmdHandle">宏命令的柄，通常可以用ACR的名字，一定要用“/”开头</param>
/// <param name="qtKeys">QT的列表</param>
/// <param name="hkResolvers">Hotkeys的列表</param>
/// <param name="HandleAddingQT">是否由MacroManager来帮你把QT和Hotkey加到QT窗口里</param>
public class MacroManager(JobViewWindow instance,
                         string cmdHandle,
                         List<(string name, string en, bool defVal, string tooltip)> qtKeys,
                         List<(string name, string en, AEAssist.CombatRoutine.View.JobView.IHotkeyResolver hkr)> hkResolvers,
                         bool HandleAddingQT = false)
{
    private bool _handleAddingQTs = HandleAddingQT;

    // 用Toast2提示QT状态会用到的东西
    private readonly List<string> QtToastBuffer = [];
    private static bool _qtToastScheduled = false;

    private string commandHandle = cmdHandle;
    private JobViewWindow instance = instance;
    private readonly List<(string name, string en, bool defVal, string tooltip)> QtKeys = qtKeys;
    private readonly List<(string name, string en, AEAssist.CombatRoutine.View.JobView.IHotkeyResolver hkr)> HKResolvers = hkResolvers;

    // 这三条会自动生成
    private readonly List<(string cmdType, string CNCmd, string ENCmd)> cmdList = [];
    private readonly Dictionary<string, string> _qtKeyDict = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, AEAssist.CombatRoutine.View.JobView.IHotkeyResolver> _hotkeyDict = new(StringComparer.OrdinalIgnoreCase);

    // 是否用Toast2提示QT状态
    public bool UseToast2 = false;

    /// <summary>
    /// 在OnEnterRotation()里面需要用到的初始化方法
    /// </summary>
    public void Init()
    {
        RegisterHandle();
        BuildCommandList();
    }

    /// <summary>
    /// 在OnExitRotation()里面需要用到的方法
    /// </summary>
    public void Exit() => ECHelper.Commands.RemoveHandler(commandHandle);

    /// <summary>
    /// 单个QT的添加
    /// </summary>
    /// <param name="name"></param>
    /// <param name="en"></param>
    /// <param name="defVal"></param>
    /// <param name="tooltip"></param>
    public void AddQt(string name, string en, bool defVal, string tooltip)
    {
        if (_handleAddingQTs) instance.AddQt(name, defVal, tooltip);

        _qtKeyDict.TryAdd(name, name);
        var cncmd = commandHandle + " " + name + "_qt";
        string encmd = "";
        if (!en.IsNullOrEmpty())
        {
            _qtKeyDict.TryAdd(en.ToLower(), name);
            encmd = commandHandle + " " + en.ToLower() + "_qt";
        }
        cmdList.Add(("QT", cncmd, encmd));
    }

    /// <summary>
    /// 单个QT的添加，支持Action callback
    /// </summary>
    /// <param name="name"></param>
    /// <param name="en"></param>
    /// <param name="defVal"></param>
    /// <param name="tooltip"></param>
    /// <param name="callback"></param>
    public void AddQt(string name, string en, bool defVal, string tooltip, Action<bool> callback)
    {
        if (_handleAddingQTs)
        {
            instance.AddQt(name, defVal, callback);
            instance.SetQtToolTip(tooltip);
        }

        _qtKeyDict.TryAdd(name, name);
        var cncmd = commandHandle + " " + name + "_qt";
        string encmd = "";
        if (!en.IsNullOrEmpty())
        {
            _qtKeyDict.TryAdd(en.ToLower(), name);
            encmd = commandHandle + " " + en.ToLower() + "_qt";
        }
        cmdList.Add(("QT", cncmd, encmd));
    }

    /// <summary>
    /// 单个Hotkey的添加
    /// </summary>
    /// <param name="name"></param>
    /// <param name="en"></param>
    /// <param name="hkr"></param>
    public void AddHotkey(string name, string en, AEAssist.CombatRoutine.View.JobView.IHotkeyResolver hkr)
    {
        if (_handleAddingQTs) instance.AddHotkey(name, hkr);

        _hotkeyDict.TryAdd(name, hkr);
        var cncmd = commandHandle + " " + name + "_hk";
        string encmd = "";
        if (!en.IsNullOrEmpty())
        {
            _hotkeyDict.TryAdd(en.ToLower(), hkr);
            encmd = commandHandle + " " + en.ToLower() + "_hk";
        }
        cmdList.Add(("Hotkey", cncmd, encmd));
    }

    private void RegisterHandle()
    {
        try
        {
            ECHelper.Commands.RemoveHandler(commandHandle);
        }
        catch (Exception) { }

        ECHelper.Commands.AddHandler(commandHandle, new Dalamud.Game.Command.CommandInfo(CommandHandler));
    }

    private void CommandHandler(string command, string args)
    {
        if (string.IsNullOrWhiteSpace(args))
        {
            LogHelper.PrintError(commandHandle[1..] + " 命令无效，请提供参数");
            return;
        }

        string processed = args.Trim().ToLower();
        if (processed.EndsWith("_qt"))
        {
            if (_qtKeyDict.ContainsKey(processed[..^3]))
            {
                ToggleQtSetting(_qtKeyDict.GetValueOrDefault(processed[..^3]));
            }
            else
            {
                LogHelper.PrintError("未知QT参数：" + args);
            }
            return;
        }

        if (processed.EndsWith("_hk"))
        {
            if (_hotkeyDict.ContainsKey(processed[..^3]))
            {
                ExecuteHotkey(_hotkeyDict.GetValueOrDefault(processed[..^3]));
            }
            else
            {
                LogHelper.PrintError("未知Hotkey参数：" + args);
            }
            return;
        }

        if (processed == "hello")
        {
            LogHelper.Print("Hello World!");
        }
        else
        {
            LogHelper.PrintError("未知参数：" + args);
        }
    }

    private static void ExecuteHotkey(AEAssist.CombatRoutine.View.JobView.IHotkeyResolver? hkr)
    {
        if (hkr is null)
        {
            LogHelper.PrintError("HotkeyResolver未初始化");
        }
        else if (hkr.Check() >= 0)
        {
            hkr.Run();
        }
        else
        {
            LogHelper.Print("无法执行Hotkey，可能条件不满足或技能不可用。");
        }
    }

    private void ToggleQtSetting(string? qtName)
    {
        if (!string.IsNullOrEmpty(qtName))
        {
            if (instance.ReverseQt(qtName))
            {
                var SuccessNote = $"QT\"{qtName}\"已设置为 {instance.GetQt(qtName)}。";
                LogHelper.Print(SuccessNote);
                if (UseToast2)
                {
                    QtToastBuffer.Add(SuccessNote);
                    if (!_qtToastScheduled)
                    {
                        _qtToastScheduled = true;
                        Task.Delay(50).ContinueWith(delegate
                        {
                            string msg = string.Join("\n", QtToastBuffer);
                            Helper.SendTips(msg, 1, 1000);
                            QtToastBuffer.Clear();
                            _qtToastScheduled = false;
                        });
                    }
                }
            }
            else
            {
                LogHelper.PrintError("Failed to Toggle QT");
            }
        }
        else
        {
            LogHelper.PrintError("Empty QT name");
        }
    }

    public void BuildCommandList()
    {
        if (cmdList.Count > 0) { return; }  // protect against multiple calls
        foreach ((string name, string en, bool defVal, string tooltip) in QtKeys)
        {
            AddQt(name, en, defVal, tooltip);
        }

        foreach ((string name, string en, AEAssist.CombatRoutine.View.JobView.IHotkeyResolver hkr) in HKResolvers)
        {
            AddHotkey(name, en, hkr);
        }
    }

    /// <summary>
    /// 描绘一个宏命令的展示窗口，一般会在QT窗口的UpdateAction里面调用
    /// </summary>
    /// <param name="windowOpenSettings">ACR的Settings里面需要建立一个控制bool来控制这个窗口是否打开</param>
    public void DrawCommandWindow(ref bool windowOpenSettings)
    {
        if (!windowOpenSettings) { return; }

        ImGuiViewportPtr mainViewport = ImGui.GetMainViewport();
        ImGui.SetNextWindowSize(new Vector2(mainViewport.Size.X / 2f,
                                            mainViewport.Size.Y / 1.5f),
                                    ImGuiCond.Always);
        ImGui.Begin(commandHandle[1..] + " 宏命令帮助", ref windowOpenSettings);
        ImGui.TextWrapped("通过 " + commandHandle + " 使用快捷指令。结合游戏内宏使用可以方便手柄用户的操作。");
        ImGui.Separator();
        ImGui.Columns(3, "CommandColumns", true);
        ImGui.SetColumnWidth(0, mainViewport.Size.X / 10f);
        ImGui.SetColumnWidth(1, mainViewport.Size.X / 5f);
        ImGui.SetColumnWidth(2, mainViewport.Size.X / 5f);
        ImGui.Text("指令类型");
        ImGui.NextColumn();
        ImGui.Text("中文指令");
        ImGui.NextColumn();
        ImGui.Text("英文指令");
        ImGui.NextColumn();
        ImGui.Separator();
        foreach (var (CmdType, CNCmd, ENCmd) in cmdList)
        {
            ImGui.Text(CmdType);
            ImGui.NextColumn();
            if (ImGui.Button("复制##" + CNCmd)) { ImGui.SetClipboardText(CNCmd); }
            ImGui.SameLine();
            ImGui.Text(CNCmd);
            ImGui.NextColumn();
            if (ImGui.Button("复制##" + ENCmd)) { ImGui.SetClipboardText(ENCmd); }
            ImGui.SameLine();
            ImGui.Text(ENCmd);
            ImGui.NextColumn();
        }

        ImGui.Columns(1);
        ImGui.Separator();
        if (ImGui.Button("关闭"))
        {
            windowOpenSettings = false;
        }
        ImGui.End();
    }
}
