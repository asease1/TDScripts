using UnityEngine;
using System.Collections;

public class DefencePoints : MonoBehaviour {

	void OnTriggerEnter(Collider hit)
    {
        if(hit.gameObject.tag == "Enemy")
        {
            Destroy(hit.gameObject);
        }
    }
}
