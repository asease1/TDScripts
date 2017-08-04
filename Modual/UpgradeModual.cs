using UnityEngine;
using System.Collections;

public class UpgradeModual {
    

    public float range, duration, damage, attackRate, projectileSpeed;
    //Effects
    public float slow, DOT, splash, leech, curse, armorReduce, hpRegReduce;
    private Sprite mySprite;
    private int level;

    public int Level { get { return level; } }

    public Sprite Icon { get { return mySprite; } }

    public UpgradeModual(Sprite Icon)
    {
        mySprite = Icon;
        range = 5;
        attackRate = 0.1f;
    }

    public UpgradeStats GetUpgradeStats()
    {
        UpgradeStats stats = new UpgradeStats();
        stats.SetStandartStats(range, duration, damage, attackRate, projectileSpeed);
        stats.SetEffectStats(slow, DOT, splash, leech, curse, armorReduce, hpRegReduce);
        return stats;
    }
}
