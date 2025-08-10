using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Define;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.JobApi;
using AEAssist.MemoryApi;
using EZACR_Offline.Gnb.QtUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZACR_Offline.Gnb.SlotResolvers.GCD;

public class BurstStrike : ISlotResolver
{
    public SlotMode SlotMode { get; } = SlotMode.Gcd;

    public static Spell GetSpell()
    {
        if ((Qt.Instance.GetQt("AOE") || 
            Qt.Instance.GetQt("强制变命运")) && 
            Core.Me.Level >= 72)
        {
            int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 8, 8);
            int nearbyEnemyCount2 = TargetHelper.GetNearbyEnemyCount(Core.Me, 5, 5);
            if ((nearbyEnemyCount2 >= 2 || 
                    (!Qt.Instance.GetQt("强制变命运") && 
                        nearbyEnemyCount >= 2)) && 
                    16163u.GetSpell().IsReadyWithCanCast())
            {
                return 16163u.GetSpell();
            }
        }

        return 16162u.GetSpell();
    }

    public int Check()
    {
        int nearbyEnemyCount = TargetHelper.GetNearbyEnemyCount(Core.Me, 8, 8);
        //if (Core.Me.HasAnyAura(Buff.无法发动技能类))
        //{
        //    return -150;
        //}

        //if (Core.Me.HasAnyAura(Buff.无法造成伤害))
        //{
        //    return -151;
        //}

        //if (Core.Me.GetCurrTarget().HasAnyAura(Buff.敌人无敌BUFF))
        //{
        //    return -152;
        //}

        if (Core.Me.Level < 30) { return -50; }
        if (Qt.Instance.GetQt("自动拉怪")) { return -1; } 
        if (Core.Me.Distance(Core.Me.GetCurrTarget(), DistanceMode.IgnoreHitbox) > 
                (float)SettingMgr.GetSetting<GeneralSettings>().AttackRange)
        {
            return -30;
        }

        if (Core.Resolve<JobApi_GunBreaker>().Ammo <= 0) { return -20; } 
        if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 && 
                Qt.Instance.GetQt("倾泻爆发")) 
        {
            return 10;
        } 
        if (!Qt.Instance.GetQt("爆发击")) { return -10; } 

        if (!Qt.Instance.GetQt("爆发"))
        {
            if (Core.Me.Level >= 88 && Core.Resolve<JobApi_GunBreaker>().Ammo == 3)
            {
                if (nearbyEnemyCount >= 2 && 
                    Qt.Instance.GetQt("AOE") && 
                    Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141)
                {
                    return 122;
                } 
                if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139)
                {
                    return 123;
                }
            } 
            if (Core.Me.Level < 88 && Core.Resolve<JobApi_GunBreaker>().Ammo == 2)
            {
                if (nearbyEnemyCount >= 2 && 
                    Qt.Instance.GetQt("AOE") && 
                    Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16141)
                {
                    return 124;
                } 
                if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139)
                {
                    return 125;
                }
            } 
            return -10;
        }

        if (GnbSettings.Instance.ACRMode == "绝亚2G" && Core.Me.HasAura(1831u))
        {
            if (16146u.CoolDownInGCDs(1))
            {
                return -4;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0)
            {
                return 4;
            }
        }

        if (GnbSettings.Instance.ACRMode == "神兵5G" && Core.Me.HasAura(1831u))
        {
            if (16146u.CoolDownInGCDs(1))
            {
                return -4;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0)
            {
                return 4;
            }
        }

        if (GnbSettings.Instance.ACRMode == "HighEnd5" && 
            Core.Resolve<JobApi_GunBreaker>().Ammo > 1 && 
            16138u.CoolDownInGCDs(2) && 
            16164u.CoolDownInGCDs(6))
        {
            return 500;
        }

        if (GnbSettings.Instance.ACRMode == "HighEnd2" && 
            Core.Resolve<JobApi_GunBreaker>().Ammo > 1 && 
            16138u.CoolDownInGCDs(2) && 
            16164u.CoolDownInGCDs(6))
        {
            return 520;
        }

        if (Core.Me.Level >= 91)
        {
            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() == 16139 && 
                16138u.GetSpell().IsReadyWithCanCast() && 
                25760u.GetSpell().IsReadyWithCanCast() && 
                16146u.GetSpell().IsReadyWithCanCast() && 
                Core.Resolve<JobApi_GunBreaker>().Ammo == 3)
            {
                return 985;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo == 1 && 
                16146u.CoolDownInGCDs(2) && 
                Qt.Instance.GetQt("子弹连") && 
                16164u.GetSpell().IsReadyWithCanCast() && 
                Qt.Instance.GetQt("血壤"))
            {
                return 530;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo == 3 && 
                Core.Me.HasAura(1831u) && 
                !16146u.CoolDownInGCDs(1))
            {
                return 533;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo == 1 && 
                16164u.CoolDownInGCDs(2) && 
                Qt.Instance.GetQt("血壤"))
            {
                return 78;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 && 
                Core.Me.HasAura(1831u) && 
                !25760u.CoolDownInGCDs(5) && 
                Qt.Instance.GetQt("倍攻") && 
                !16146u.CoolDownInGCDs(1) && 
                (!36937u.GetSpell().IsReadyWithCanCast() || 
                !36938u.GetSpell().IsReadyWithCanCast() || 
                !36939u.GetSpell().IsReadyWithCanCast()))
            {
                return 9;
            }

            if (16146u.CoolDownInGCDs(2) && 
                Qt.Instance.GetQt("子弹连") && 
                25760u.CoolDownInGCDs(1) && 
                Qt.Instance.GetQt("倍攻") && 
                Core.Resolve<JobApi_GunBreaker>().Ammo == 2)
            {
                return -50;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 
                && !25760u.CoolDownInGCDs(5) && 
                !16146u.CoolDownInGCDs(2) && 
                Core.Me.HasAura(49u) && 
                Qt.Instance.GetQt("爆发药卸豆子"))
            {
                return 211;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 
                && (!25760u.CoolDownInGCDs(5) || 
                (!Qt.Instance.GetQt("倍攻") && 
                (!16146u.CoolDownInGCDs(2) || 
                !Qt.Instance.GetQt("子弹连")))) && 
                Qt.Instance.GetQt("强制爆发击"))
            {
                return 211;
            }
        }

        if (Core.Me.Level >= 100 && 
            (36937u.GetSpell().IsReadyWithCanCast() || 
            36938u.GetSpell().IsReadyWithCanCast() || 
            36939u.GetSpell().IsReadyWithCanCast()))
        {
            return -100;
        }

        if (GnbSettings.Instance.ACRMode == "Normal")
        {
            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 && Core.Me.HasAura(1831u))
            {
                return 99;
            }

            if (Core.Resolve<JobApi_GunBreaker>().Ammo > 0 && 16164u.CoolDownInGCDs(2) && Core.Me.Level >= 76)
            {
                return 95;
            }
        }

        if (Core.Me.Level < 88)
        {
            if (Core.Resolve<JobApi_GunBreaker>().Ammo < 2)
            {
                return -100;
            }
        }
        else if (Core.Resolve<JobApi_GunBreaker>().Ammo < 3)
        {
            return -90;
        }

        if (Core.Me.Level >= 88 && Core.Resolve<JobApi_GunBreaker>().Ammo == 3)
        {
            if (nearbyEnemyCount >= 2 && 
                Qt.Instance.GetQt("AOE") && 
                Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 16141)
            {
                return -5;
            }

            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 16139)
            {
                return -5;
            }
        }

        if (Core.Me.Level < 88 && 
            Core.Resolve<MemApiZoneInfo>().GetCurrTerrId() != 777 && 
            Core.Resolve<JobApi_GunBreaker>().Ammo == 2)
        {
            if (nearbyEnemyCount >= 2 && 
                Qt.Instance.GetQt("AOE") && 
                Core.Me.Level >= 72 && 
                Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 16141)
            {
                return -5;
            }

            if (Core.Resolve<MemApiSpell>().GetLastComboSpellId() != 16139)
            {
                return -5;
            }
        }

        return 0;
    }

    public void Build(Slot slot)
    {
        Spell spell = GetSpell();
        if (spell != null)
        {
            slot.Add(spell);
        }
    }
}
