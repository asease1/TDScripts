using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

    public enum damageTypes { light, medium, heavy, special1, special2, special3, none}

    public damageTypes myDamageType;
    public UpgradeStats stats;
    public Transform target;
    protected TowerShot spawnTower;

    public void SetBaseBullet(UpgradeStats stats, Transform target, damageTypes bulletDamageType, TowerShot motherTower)
    {
        this.stats = stats;
        this.target = target;
        myDamageType = bulletDamageType;
        spawnTower = motherTower;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.layer != LayerMask.NameToLayer("RangeLayer"))
        {
            if (hit.gameObject.tag == "Enemy")
            {
                EnemyStats temp = hit.GetComponent<EnemyStats>();
                temp.Attacked(stats, myDamageType);
            }
            spawnTower.SetInActiveBullet(this);
        }
    }

    public static damageTypes GetDamageType(damageTypes damage1, damageTypes damage2)
    {
        switch (damage1)
        {
            case damageTypes.light:
                switch (damage2)
                {
                    case damageTypes.medium:
                        return damageTypes.special1;
                    case damageTypes.heavy:
                        return damageTypes.special2;
                    default:
                        return damage1;
                }
                break;
            case damageTypes.medium:
                switch (damage2)
                {
                    case damageTypes.medium:
                        return damage2;
                    case damageTypes.heavy:
                        return damageTypes.special3;
                    default:
                        return GetDamageType(damage2, damage1);
                }
                break;
            case damageTypes.heavy:
                switch (damage2)
                {
                    case damageTypes.heavy:
                        return damage2;
                    default:
                        return GetDamageType(damage2, damage1);
                }
                break;
            default:
                return damage2;
                break;
        }
    }
}
