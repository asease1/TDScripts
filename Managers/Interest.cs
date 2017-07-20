using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interest : MonoBehaviour {

	public void addInterest()
    {
        //TODO add code for Interest
        ResourceManager.instance.addResources(50);
    }
}
