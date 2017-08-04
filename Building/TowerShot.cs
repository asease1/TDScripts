using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerShot : MonoBehaviour {

    public BulletScript.movementPaten attackType;
    private GetTargetTower GetTargets;
    public GetTargetTower.TargetType targetType;
    public float getTargetDelay;
    private EnemyStats currentTarget;
    public GameObject gun, baseGun;
    private List<BaseBullet> Bullets = new List<BaseBullet>();
    private Queue<BaseBullet> InActive = new Queue<BaseBullet>();
    private UpgradeStats myStats;
    private float lastRun = 0;
    private GameObject currentWeaponModual;
    private BaseBullet.damageTypes weaponType1 = BaseBullet.damageTypes.none, weaponType2 = BaseBullet.damageTypes.none;

    // Use this for initialization
    void Start () {
        GetTargets = GetComponent<GetTargetTower>();
        myStats = GetComponent<TowerUpgrade>().myStats;
        GetComponent<TowerUpgrade>().ChangeWeaponModual.AddListener(UpdateWeaponModual);
        StartCoroutine("GetTarget", getTargetDelay);
        attackType = BulletScript.movementPaten.line;
	}
	
	// Update is called once per frame
	void Update () {
       // if (currentWeaponModual == null)
            //return;
        //Debug.Log(currentTarget);
        if (currentTarget != null)
        {
            gun.transform.LookAt(currentTarget.transform);
            
            Vector3 rotation = gun.transform.localEulerAngles;
            //Code to make blender files work
            //gun.transform.localEulerAngles = new Vector3(-rotation.x, rotation.y-180, 0);
            if (baseGun != null)
                baseGun.transform.localEulerAngles = new Vector3(0, rotation.y, 0);
        }
        
        if(Time.timeSinceLevelLoad > lastRun + myStats.attackRate)
        {           
            if (currentTarget != null)
            {
                lastRun = lastRun + myStats.attackRate;
                switch (attackType)
                {
                    case BulletScript.movementPaten.line:
                        BulletScript.InstanceBullet(gun.transform, 20, 0);
                        break;
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

    private void UpdateWeaponModual(UpgradeModual[] moduals)
    {
        if (moduals[0] != null && moduals[0].GetType() == typeof(WeaponModual))
            weaponType1 = ((WeaponModual)moduals[0]).damageType;
        else
            weaponType1 = BaseBullet.damageTypes.none;
        if (moduals[1] != null && moduals[1].GetType() == typeof(WeaponModual))
            weaponType2 = ((WeaponModual)moduals[1]).damageType;
        else
            weaponType2 = BaseBullet.damageTypes.none;
        BaseBullet.damageTypes damageType = BaseBullet.GetDamageType(weaponType1, weaponType2);
        Destroy(currentWeaponModual);
        switch (damageType)
        {
            case BaseBullet.damageTypes.light:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModual1);
                break;
            case BaseBullet.damageTypes.medium:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModual2);
                break;
            case BaseBullet.damageTypes.heavy:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModual2);              
                break;
            case BaseBullet.damageTypes.special1:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModualS1);
                break;
            case BaseBullet.damageTypes.special2:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModualS2);
                break;
            case BaseBullet.damageTypes.special3:
                currentWeaponModual = Instantiate(BuildManager.instance.gunModualS3);
                break;
            default:
                return;
        }
        currentWeaponModual.transform.position = gun.transform.position;
        currentWeaponModual.transform.parent = gun.transform;
        currentWeaponModual.transform.rotation = gun.transform.parent.rotation;

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
