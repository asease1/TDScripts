using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerShot : MonoBehaviour {
    public enum AttackType { Homing, Line, Artillery}

    public AttackType attackType;
    private GetTargetTower GetTargets;
    public GetTargetTower.TargetType targetType;
    public float getTargetDelay;
    private EnemyStats currentTarget;
    public GameObject gun, baseGun;
    public GameObject Bullet;
    private List<BaseBullet> Bullets = new List<BaseBullet>();
    private Queue<BaseBullet> InActive = new Queue<BaseBullet>();
    private UpgradeStats myStats;
    private float lastRun = 0;

    // Use this for initialization
    void Start () {
        GetTargets = GetComponent<GetTargetTower>();
        myStats = GetComponent<TowerUpgrade>().myStats;
        StartCoroutine("GetTarget", getTargetDelay);
        attackType = AttackType.Homing;
	}
	
	// Update is called once per frame
	void Update () {
        if (currentTarget != null)
        {
            gun.transform.LookAt(currentTarget.transform);
            Vector3 rotation = gun.transform.localEulerAngles;
            gun.transform.localEulerAngles = new Vector3(-rotation.x, rotation.y-180, 0);
            baseGun.transform.localEulerAngles = new Vector3(0, rotation.y, 0);
        }

        if(Time.timeSinceLevelLoad > lastRun + myStats.attackRate)
        {
            lastRun = lastRun + myStats.attackRate;
            if (currentTarget != null)
            {
                if (InActive.Count > 0)
                {
                    BaseBullet bulletTemp;
                    bulletTemp = InActive.Dequeue();
                    bulletTemp.target = currentTarget.transform;
                    bulletTemp.gameObject.SetActive(true);
                    bulletTemp.transform.position = gun.transform.position;
                    bulletTemp.transform.rotation = Quaternion.Euler(gun.transform.eulerAngles.x - 180, gun.transform.eulerAngles.y, gun.transform.eulerAngles.z);
                }
                else
                {
                    GameObject bulletTemp = Instantiate(Bullet, gun.transform.position, Quaternion.Euler(gun.transform.eulerAngles.x - 180, gun.transform.eulerAngles.y, gun.transform.eulerAngles.z)) as GameObject;
                    switch (attackType)
                    {
                        case AttackType.Homing:
                            Homing temp = bulletTemp.AddComponent<Homing>();
                            temp.SetBullet(myStats, 10, currentTarget.transform, BaseBullet.damageTypes.light, this);
                            Bullets.Add(temp);
                            Debug.Log("test");
                            break;
                        case AttackType.Artillery:
                        case AttackType.Line:
                            bulletTemp.AddComponent<Line>().SetBaseBullet(myStats, currentTarget.transform, BaseBullet.damageTypes.light, this);
                            break;
                    }
                }
            }
        }
    }

    public void SetInActiveBullet(BaseBullet bullet)
    {
        InActive.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
    }

    public void KillTarget(EnemyStats target)
    {
        GetTargets.KillTarget(target);
        UpdateBullets();
    }

    public void UpdateBullets()
    {
        currentTarget = GetTargets.GetTarget(targetType);
        if (currentTarget == null)
        {
            foreach (BaseBullet elm in Bullets)
            {
                elm.target = null;
            }
        }
        else
        {
            foreach (BaseBullet elm in Bullets)
            {
                elm.target = currentTarget.transform;
            }
        }
    }

    IEnumerator GetTarget(float delay)
    {
        while (true)
        {
            currentTarget = GetTargets.GetTarget(targetType);
            yield return new WaitForSeconds(delay);
        }
    }
}
