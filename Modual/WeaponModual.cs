using UnityEngine;
using System.Collections;

public class WeaponModual : UpgradeModual {
    public GameObject weaponModel, projectile;
    public BulletScript.movementPaten attackType;
    public BaseBullet.damageTypes damageType;

    public WeaponModual(Sprite Icon) : base(Icon)
    {
       
    }

}
