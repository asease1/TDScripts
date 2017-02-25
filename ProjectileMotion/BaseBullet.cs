using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

    public enum damageTypes { light, medium, heavy, special1, special2, special3}

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
}
