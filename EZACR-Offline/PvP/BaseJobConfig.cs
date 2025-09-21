using EZACR_Offline.PvP.Brd;

namespace EZACR_Offline.PvP;

public abstract class BaseJobConfig : IPvPJob {
  public virtual void 权限获取() {
    PVPHelper.权限获取();
  }

  public virtual void ConfigureSkillBool(
      uint SkillID,
      string SkillName,
      string description,
      ref bool variable,
      int ID) {
    PVPHelper.技能配置2(SkillID, SkillName, description, ref variable, ID);
  }

  public virtual void ConfigureSkillInt(
      uint SkillID,
      string SkillName,
      string description,
      ref int value,
      int step,
      int quickstep,
      int id) {
    PVPHelper.技能配置3(SkillID, SkillName, description, ref value, step, quickstep, id);
  }

  public virtual void ConfigureSkillBoolInt(
      uint SkillID,
      string SkillName,
      string IntDescription,
      string description,
      ref bool status,
      ref int value,
      int step,
      int quickstep,
      int id) {
    PVPHelper.技能配置4(SkillID,
                    SkillName,
                    IntDescription,
                    description,
                    ref status,
                    ref value,
                    step,
                    quickstep,
                    id);
  }

  public virtual void ConfigureSkillSliderFloat(
      uint SkillID,
      string SkillName,
      string IntDescription,
      ref float value,
      float min,
      float max,
      int id) {
    PVPHelper.技能配置5(SkillID, SkillName, IntDescription, ref value, min, max, id);
  }

  public virtual void
      ConfigureSkilldescription(uint SkillID, string SkillName, string description) {
    PVPHelper.技能解释(SkillID, SkillName, description);
  }
}
