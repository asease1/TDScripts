using UnityEngine;
using System.Collections;

public class SpawnPointsV2 : MonoBehaviour {

    public Transform target;
    public float SpawnRate;
    public GameObject[] enemies;


	// Use this for initialization
	void Start () {
        PathRequestManager.Instance.pathFinding.CreatePath(transform.position, target.position);
        StartCoroutine("Spawn");
    }
	
	IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(enemies[0], transform.position, Quaternion.identity);
            yield return new WaitForSeconds(SpawnRate);
        }
    }
}
