using UnityEngine;
using System.Collections;

public class UpgradeStats
{
    public float range, duration, damage, attackRate, projectileSpeed;
    //Effects
    public float slow, DOT, splash, leech, curse, armorReduce, hpRegReduce;

    public void SetStandartStats(float range, float duration, float damage, float attackRate, float projectileSpeed)
    {
        this.range = range;
        this.duration = duration;
        this.damage = damage;
        this.attackRate = attackRate;
        this.projectileSpeed = projectileSpeed;
    }
    public void SetEffectStats(float slow, float DOT, float splash, float leech, float curse, float armorReduce, float hpRegReduce)
    {
        this.slow = slow;
        this.DOT = DOT;
        this.splash = splash;
        this.leech = leech;
        this.curse = curse;
        this.armorReduce = armorReduce;
        this.hpRegReduce = hpRegReduce;
    }
    public void PlusTwoUpgradeStats(UpgradeStats stats)
    {
        range += stats.range;
        duration += stats.duration;
        damage += stats.damage;
        attackRate += stats.attackRate;
        projectileSpeed += stats.projectileSpeed;
        slow += stats.slow;
        DOT += stats.DOT;
        splash += stats.splash;
        leech += stats.leech;
        curse += stats.curse;
        armorReduce += stats.armorReduce;
        hpRegReduce += stats.hpRegReduce;
    }
    public void ResetValues()
    {
        range = 0;
        duration = 0;
        damage = 0;
        attackRate = 0;
        projectileSpeed = 0;
        slow = 0;
        DOT = 0;
        splash = 0;
        leech = 0;
        curse = 0;
        armorReduce = 0;
        hpRegReduce = 0;
    }
}
