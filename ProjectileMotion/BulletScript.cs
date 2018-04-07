using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public enum movementPaten { line, homing, artillery}

    private static Queue<GameObject> inActiveBullets;
    private static GameObject[] bulletPrefabs;
    private movementPaten movementPatern;
    private BaseBullet.damageTypes damageType;
    private float speed;
    private UpgradeStats stats;

    public UpgradeStats setStats { set { stats = value; } }
    public movementPaten setMovementPatern { set { movementPatern = value; } }



    public static void InstanceBullet(Transform spawnTransform, float speed, GameObject prefab, movementPaten movement = movementPaten.line)
    {
        if (inActiveBullets == null)
            inActiveBullets = new Queue<GameObject>();

        BulletScript bulletStats;

        if (inActiveBullets.Count == 0)
        {
            GameObject bullet = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
            bulletStats = bullet.AddComponent<BulletScript>();
        }
        else
            bulletStats = inActiveBullets.Dequeue().GetComponent<BulletScript>();
        
        bulletStats.setMovementPatern = movementPaten.line;
        bulletStats.speed = speed;

    }
	
	// Update is called once per frame
	void Update () {
        switch (movementPatern)
        {
            case movementPaten.line:
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case movementPaten.homing:
            case movementPaten.artillery:
            default:
                Debug.Log("Something wrong this bullet is going to wrap");
                break;
        }

    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.layer != LayerMask.NameToLayer("IgnoreRaycast"))
        {
            if (hit.gameObject.tag == "Enemy")
            {
                EnemyStats temp = hit.GetComponent<EnemyStats>();
                temp.Attacked(stats, damageType);
            }
            inActiveBullets.Enqueue(gameObject);
            gameObject.active = false;
        }
    }
}
