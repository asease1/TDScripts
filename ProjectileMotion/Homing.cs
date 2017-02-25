using UnityEngine;
using System.Collections;

public class Homing : BaseBullet
{
    public float rotationspeed;

    public void SetBullet(UpgradeStats stats, float rotaionspeed, Transform target, damageTypes myDamageType, TowerShot spawnTower)
    {
        this.rotationspeed = rotaionspeed;
        SetBaseBullet(stats , target, myDamageType, spawnTower);
    }


    // Update is called once per frame
    void Update() {
        if (target != null)
        {
        
            Vector3 targetDir = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(targetDir);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationspeed);
            transform.position += transform.forward * stats.projectileSpeed * Time.deltaTime;
        }
        else
        {
            transform.position += transform.forward * stats.projectileSpeed * Time.deltaTime;
        }
    }

}
