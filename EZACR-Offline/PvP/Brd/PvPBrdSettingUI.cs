using AEAssist.CombatRoutine.View;
using Dalamud.Bindings.ImGui;

namespace EZACR_Offline.PvP.Brd;

public class PvPBrdSettingUI : ISettingUI {
  public string Name => "诗人";

  public void Draw() {
    ImGui.Text("爱唱拦不住");
  }
}
