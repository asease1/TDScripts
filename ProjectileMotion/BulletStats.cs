using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletStats{

    public enum bulletType { Homing, Line, Artillery}
    public enum damageTypes { light, medium, heavy, special1, special2, special3, none }
    public UpgradeStats stats;


    public BulletStats(float speed, UpgradeStats stats)
    {
        
        this.stats = stats;

    }
}
