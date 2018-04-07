using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
    public GameObject modualSlot;
    private GameObject weaponModual;

    private GetTargetTower getTarget;

    private TowerUpgradeModual[] towerUpgrade;
    private TowerWeaponModual[] towerWeapon;

    private Constants.DamageType damageType;
    private float range;
    private float attackSpeed = 0.001f;

    private Transform barrel, mount;

    private GameObject bulletModel;

	// Use this for initialization
	void Start () {
        GameObject getTarget = new GameObject();
        getTarget.transform.parent = transform;
        getTarget.transform.localPosition = Vector3.zero;
        this.getTarget = getTarget.AddComponent<GetTargetTower>();
        this.getTarget.UpdateRange(10);
        towerWeapon = transform.parent.GetComponentsInChildren<TowerWeaponModual>();
        towerUpgrade = transform.parent.GetComponentsInChildren<TowerUpgradeModual>();
	}
	
	public void updateStats()
    {
        foreach(TowerWeaponModual elm in towerWeapon)
        {
            //TODO Do some mixing of types
            damageType = elm.weaponSlot.damageType;
            range += elm.weaponSlot.range;
            attackSpeed += elm.weaponSlot.attackSpeed;

            Destroy(weaponModual);

            switch (damageType)
            {
                case Constants.DamageType.Magic:
                    weaponModual = Instantiate((GameObject)Resources.Load("Prefabs/turrets/turret_cannon", typeof(GameObject)));
                    weaponModual.transform.parent = modualSlot.transform;
                    weaponModual.transform.localPosition = Vector3.zero;
                    break;
                case Constants.DamageType.Elementel:
                    break;
                case Constants.DamageType.Primal:
                    break;
                case Constants.DamageType.mix1:
                    break;
                case Constants.DamageType.mix2:
                    break;
                case Constants.DamageType.mix3:
                    break;
            }
            bulletModel = ((GameObject)Resources.Load("Prefabs/projectiles/" + elm.bulletModual));
            barrel = weaponModual.transform.FindChild("turret_barrel");
            mount = weaponModual.transform.FindChild("turret_mount");

            StopCoroutine(Attack());
            StartCoroutine(Attack());
        }
    }

    // every 2 seconds perform the print()
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/attackSpeed);
            EnemyStats enemy = getTarget.GetTarget(GetTargetTower.TargetType.Closed);
            if (enemy == null)
                continue;
            barrel.LookAt(enemy.transform);

            mount.localEulerAngles = new Vector3(0, barrel.eulerAngles.y, 0);
            BulletScript.InstanceBullet(barrel, 10, bulletModel);
        }
    }
}
