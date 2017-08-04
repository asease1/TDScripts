using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public enum movementPaten { line, homing, artillery}

    private static Queue<GameObject> inActiveBullets;
    private const float roationSpeed = 20;
    private static GameObject[] bulletPrefabs;
    private movementPaten movementPatern;
    private BaseBullet.damageTypes damageType;
    private float speed;
    private UpgradeStats stats;

    public UpgradeStats setStats { set { stats = value; } }
    public movementPaten setMovementPatern { set { movementPatern = value; } }



    private static GameObject getBulletPrefab(int index)
    {
        if(bulletPrefabs == null)
        {
            bulletPrefabs = new GameObject[1];
            bulletPrefabs[0] = (GameObject)Resources.Load("Prefab/Bullets/Bullet1", typeof(GameObject));
        }
        return bulletPrefabs[index];
    }

    public static void InstanceBullet(Transform spawnTransform, float speed, int prefab)
    {
        if (inActiveBullets == null)
            inActiveBullets = new Queue<GameObject>();

        BulletScript bulletStats;
        Debug.Log(inActiveBullets.Count);
        if (inActiveBullets.Count == 0)
        {
            GameObject bullet = Instantiate(getBulletPrefab(prefab), spawnTransform.position, spawnTransform.rotation);
            bulletStats = bullet.GetComponent<BulletScript>();
        }
        else
            bulletStats = inActiveBullets.Dequeue().GetComponent<BulletScript>();
        
        bulletStats.setMovementPatern = movementPaten.line;
        bulletStats.speed = speed;

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (movementPatern)
        {
            case movementPaten.line:
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            default:
                Debug.Log("Something wrong this bullet is going to wrap");
                break;
        }
		
	}

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.layer != LayerMask.NameToLayer("RangeLayer"))
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
