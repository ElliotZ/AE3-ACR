using System.Numerics;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Textures.TextureWraps;

namespace EZACR_Offline.PvP;

public enum Filter {
  None,
  可施法,
  可攻击,
  无无敌,
  可控制,
}

public enum Group {
  队友,
  敌人,
  全部,
}

public class SkillDecision {
  public static uint 技能变化(uint skillId) {
    return Core.Resolve<MemApiSpell>().CheckActionChange(skillId);
  }
}

public class HotkeyData {
/*  public class 蛇LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(39190U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(39190U, (IBattleChara) Core.Me), size, isActive);
    }

    public int Check() => 0;

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (!(!Core.Me.HasLocalPlayerAura(4094U) & Core.Me.InCombat() & Core.Me.LimitBreakCurrentValue() >= (ushort) 3000 & Core.Me.GetCurrTarget() != null) || PVPTargetHelper.TargetSelector.Get最合适目标(20, 39190U) == Core.Me || (double) Core.Me.GetCurrTarget().DistanceToPlayer() >= 25.0)
        return;
      if (PvPSettings.Instance.技能自动选中)
      {
        if (PvPSettings.Instance.最合适目标)
        {
          if (PVPTargetHelper.TargetSelector.Get最合适目标(20, 39190U) != null && PVPTargetHelper.TargetSelector.Get最合适目标(20, 39190U) != Core.Me)
            AI.Instance.BattleData.NextSlot.Add(new Spell(39190U, PVPTargetHelper.TargetSelector.Get最合适目标(20, 39190U)));
        }
        else if (PVPTargetHelper.TargetSelector.Get最近目标() != null && PVPTargetHelper.TargetSelector.Get最合适目标(20, 39190U) != Core.Me)
          AI.Instance.BattleData.NextSlot.Add(new Spell(39190U, PVPTargetHelper.TargetSelector.Get最近目标()));
      }
      else if (Core.Me.GetCurrTarget() != null)
        AI.Instance.BattleData.NextSlot.Add(new Spell(39190U, Core.Me.GetCurrTarget()));
    }
  }*/

/*  public class 机工LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(29415U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(29415U, Core.Me.GetCurrTarget()), size, isActive);
    }

    public int Check() => 0;

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (Core.Me.LimitBreakCurrentValue() < (ushort) 3000)
        return;
      if (PvPMCHSettings.Instance.智能魔弹)
      {
        if (PVPTargetHelper.TargetSelector.Get最合适目标(50, 29415U).IsTargetable)
        {
          AI.Instance.BattleData.NextSlot.Add(new Spell(29415U, PVPTargetHelper.TargetSelector.Get最合适目标(50, 29415U)));
          Core.Resolve<MemApiChatMessage>().Toast2($"正在尝试对 {PVPTargetHelper.TargetSelector.Get最合适目标(50, 29415U).Name} 释放 魔弹射手,距离你{PVPTargetHelper.TargetSelector.Get最合适目标(50, 29415U).DistanceToPlayer()}米!", 1, 3000);
        }
      }
      else
      {
        AI.Instance.BattleData.NextSlot.Add(new Spell(29415U, Core.Me.GetCurrTarget()));
        Core.Resolve<MemApiChatMessage>().Toast2($"正在尝试对 {Core.Me.GetCurrTarget().Name} 释放 魔弹射手,距离你{Core.Me.GetCurrTarget().DistanceToPlayer()}米!", 1, 3000);
      }
    }
  }*/

/*  public class 霰弹枪 : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(29404U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(29404U, (IBattleChara) Core.Me), size, isActive);
    }

    public int Check()
    {
      if (Core.Me.GetCurrTarget() == null)
        return -1;
      if ((double) Core.Me.GetCurrTarget().DistanceToPlayer() > 12.0)
        return -3;
      return !29404U.GetSpell().IsReadyWithCanCast() ? -2 : 0;
    }

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      AI.Instance.BattleData.NextSlot.Add(new Spell(29404U, Core.Me.GetCurrTarget()));
    }
  }*/

/*  public class 画家LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(39215U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(39215U, (IBattleChara) Core.Me), size, isActive);
    }

    public int Check() => 0;

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (!(!Core.Me.HasLocalPlayerAura(4094U) & Core.Me.InCombat() & Core.Me.LimitBreakCurrentValue() >= (ushort) 3500))
        return;
      AI.Instance.BattleData.NextSlot.Add(new Spell(39215U, (IBattleChara) Core.Me));
    }
  }*/

/*  public class 绝枪LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(29130U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(29130U, (IBattleChara) Core.Me), size, isActive);
    }

    public int Check() => Core.Me.LimitBreakCurrentValue() < (ushort) 2000 ? -1 : 0;

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (!(!Core.Me.HasLocalPlayerAura(4094U) & Core.Me.InCombat()))
        return;
      AI.Instance.BattleData.NextSlot.Add(new Spell(29130U, (IBattleChara) Core.Me));
    }
  }*/

/*  public class 武士LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(29537U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(29537U, (IBattleChara) Core.Me), size, isActive);
    }

    public int Check() => 0;

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (PVPHelper.CanActive() || !Core.Me.InCombat() || Core.Me.LimitBreakCurrentValue() <= (ushort) 3500)
        return;
      if (PvPSAMSettings.Instance.多斩模式)
      {
        if (PvPSAMSettings.Instance.斩铁调试)
          LogHelper.Print($"尝试斩铁目标：{PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数)}");
        AI.Instance.BattleData.NextSlot.Add(new Spell(29537U, PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数)));
        Core.Resolve<MemApiChatMessage>().Toast2($"正在尝试对 {PVPTargetHelper.TargetSelector.Get多斩Target(PvPSAMSettings.Instance.多斩人数).Name} 释放 斩铁剑", 1, 1500);
      }
      else
      {
        if (PvPSAMSettings.Instance.斩铁调试)
          LogHelper.Print($"尝试斩铁目标：{PVPTargetHelper.TargetSelector.Get斩铁目标()}");
        AI.Instance.BattleData.NextSlot.Add(new Spell(29537U, PVPTargetHelper.TargetSelector.Get斩铁目标()));
        Core.Resolve<MemApiChatMessage>().Toast2($"正在尝试对 {PVPTargetHelper.TargetSelector.Get斩铁目标().Name} 释放 斩铁剑", 1, 1500);
      }
    }
  }*/

  public class 诗人LB : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);

      if (!Core.Resolve<MemApiIcon>()
               .GetActionTexture(29401U, out IDalamudTextureWrap? textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(29401U, Core.Me), size, isActive);
    }

    public int Check() {
      return 0;
    }

    public void Run() {
      AI.Instance.BattleData.NextSlot ??= new Slot();

      if (!(Core.Me.InCombat() & (Core.Me.LimitBreakCurrentValue() >= 4000))) {
        return;
      }

      AI.Instance.BattleData.NextSlot.Add(new Spell(29401U, Core.Me));
    }
  }

  public class 抗死 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap? textureWrap;

      if (Core.Me.HasLocalPlayerAura(3245U)) {
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(29697U, out textureWrap)) {
          return;
        }
      } else if (!Core.Resolve<MemApiIcon>().GetActionTexture(29698U, out textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(Core.Me.HasLocalPlayerAura(3245U)
                                    ? new Spell(29697U, Core.Me)
                                    : new Spell(29698U, Core.Me),
                                size,
                                isActive);
    }

    public int Check() {
      return !29697U.IsUnlockWithCDCheck() && !29698U.IsUnlockWithCDCheck() ? -1 : 1;
    }

    public void Run() {
      AI.Instance.BattleData.NextSlot ??= new Slot();
      AI.Instance.BattleData.NextSlot.Add(Core.Me.HasLocalPlayerAura(3245U)
                                              ? new Spell(29697U, Core.Me)
                                              : new Spell(29698U, Core.Me));
    }
  }

  public class 黑白魔元切换 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap? textureWrap;

      if (Core.Me.HasLocalPlayerAura(3245U)) {
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(29702U, out textureWrap)) {
          return;
        }
      } else if (!Core.Resolve<MemApiIcon>().GetActionTexture(29703U, out textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(Core.Me.HasLocalPlayerAura(3245U)
                                    ? new Spell(29702U, Core.Me)
                                    : new Spell(29703U, Core.Me),
                                size,
                                isActive);
    }

    public int Check() {
      return 0;
    }

    public void Run() {
      AI.Instance.BattleData.NextSlot ??= new Slot();
      AI.Instance.BattleData.NextSlot.Add(Core.Me.HasLocalPlayerAura(3245U)
                                              ? new Spell(29702U, Core.Me)
                                              : new Spell(29703U, Core.Me));
    }
  }

/*  public class 赤魔LB : IHotkeyResolver
  {
    public void Draw(Vector2 size)
    {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);
      IDalamudTextureWrap textureWrap;
      if (!Core.Resolve<MemApiIcon>().GetActionTexture(41498U, out textureWrap))
        return;
      ImGui.Image(textureWrap.ImGuiHandle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
      SpellHelper.DrawSpellInfo(new Spell(41498U, new Func<IBattleChara>(((GameObjectExtension) Core.Me).GetCurrTarget)), size, isActive);
    }

    public int Check()
    {
      return Core.Me.InCombat() & Core.Me.LimitBreakCurrentValue() < (ushort) 3000 ? -1 : 1;
    }

    public void Run()
    {
      if (AI.Instance.BattleData.NextSlot == null)
        AI.Instance.BattleData.NextSlot = new Slot();
      if (!(Core.Me.InCombat() & Core.Me.LimitBreakCurrentValue() >= (ushort) 3000))
        return;
      if (PvPRDMSettings.Instance.南天自己)
        AI.Instance.BattleData.NextSlot.Add(new Spell(41498U, (IBattleChara) Core.Me));
      else
        AI.Instance.BattleData.NextSlot.Add(new Spell(41498U, Core.Me.GetCurrTarget()));
    }
  }*/

  public class 后射 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);

      if (!Core.Resolve<MemApiIcon>()
               .GetActionTexture(29399U, out IDalamudTextureWrap? textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(29399U, Core.Me), size, isActive);
    }

    public int Check() {
      return !29399U.GetSpell().IsReadyWithCanCast() ? -2 : 1;
    }

    public void Run() {
      AI.Instance.BattleData.NextSlot ??= new Slot();
      AI.Instance.BattleData.NextSlot.Add(29399U.GetSpell());
    }
  }

  public class 后跳 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);

      if (!Core.Resolve<MemApiIcon>()
               .GetActionTexture(29494U, out IDalamudTextureWrap? textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(29494U, Core.Me), size, isActive);
    }

    public int Check() {
      return !29494U.GetSpell().IsReadyWithCanCast() ? -2 : 1;
    }

    public void Run() {
      Core.Resolve<MemApiMove>().SetRot(PVPHelper.GetCameraRotation反向());
      Core.Resolve<MemApiSpell>().Cast(29494U,
                                       PVPHelper.向量位移反向(Core.Me.Position,
                                                        PVPHelper.GetCameraRotation(),
                                                        15f));
    }
  }

  public class 速涂 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);

      if (!Core.Resolve<MemApiIcon>()
               .GetActionTexture(39210U, out IDalamudTextureWrap? textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(39210U, Core.Me), size, isActive);
    }

    public int Check() {
      return !39210U.GetSpell().IsReadyWithCanCast() ? -2 : 1;
    }

    public void Run() {
      Core.Resolve<MemApiMove>().SetRot(PVPHelper.GetCameraRotation());
      Core.Resolve<MemApiSpell>().Cast(39210U,
                                       PVPHelper.向量位移(Core.Me.Position,
                                                      PVPHelper.GetCameraRotation(),
                                                      15f));
    }
  }

  public class 以太步 : IHotkeyResolver {
    public void Draw(Vector2 size) {
      Vector2 vector2 = size * 0.8f;
      ImGui.SetCursorPos(size * 0.1f);

      if (!Core.Resolve<MemApiIcon>()
               .GetActionTexture(29660U, out IDalamudTextureWrap? textureWrap)) {
        return;
      }

      if (textureWrap != null) ImGui.Image(textureWrap.Handle, vector2);
    }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(29660U, Core.Me), size, isActive);
    }

    public int Check() {
      return !29660U.GetSpell().IsReadyWithCanCast() ? -2 : 1;
    }

    public void Run() {
      AI.Instance.BattleData.NextSlot ??= new Slot();
      AI.Instance.BattleData.NextSlot.Add(29660U.GetSpell());
    }
  }

  public class 喵 : IHotkeyResolver {
    public void Draw(Vector2 size) { }

    public void DrawExternal(Vector2 size, bool isActive) {
      SpellHelper.DrawSpellInfo(new Spell(39215U, Core.Me), size, isActive);
    }

    public int Check() {
      return 0;
    }

    public void Run() { }
  }
}
