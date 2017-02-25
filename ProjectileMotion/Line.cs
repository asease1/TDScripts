using UnityEngine;
using System.Collections;

public class Line : BaseBullet {
	
	void Update () {

        transform.position +=  transform.forward * stats.projectileSpeed * Time.deltaTime;
	}
}
