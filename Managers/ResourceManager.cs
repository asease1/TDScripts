using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class ResourceManager : MonoBehaviour {

    private int level = 1;
    int resources;
    int resourceCap;
    float CapIncrese;
    [SerializeField]
    private int baseResourceCap = 500;
    [SerializeField]
    private float baseIntrest = 1.05f;
    public IntEvent ResourceUpdateEvent;

    public int ResourceCap { get { return (int)Mathf.Pow(baseResourceCap, level); } }

    // Use this for initialization
    void Start () {
        if (ResourceUpdateEvent == null)
            ResourceUpdateEvent = new IntEvent();
	}
	
	// Update is called once per frame
	void FixedUpdate()
    {
        ResourceUpdateEvent.Invoke(resources);
	}

    public void CallIntrest()
    {
        resources = (int)((float)resources * baseIntrest);
    }

    public void IncresseResources(int amount)
    {
        resources += amount;
        if (resources > ResourceCap)
            level++;
    }

    public int GetResources()
    {
        return resources;
    }
}
