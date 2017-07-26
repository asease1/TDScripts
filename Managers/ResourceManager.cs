using UnityEngine;
using UnityEngine.Events;
using System.Collections;


public class ResourceManager : MonoBehaviour {

    private int level = 1;
    private int resources = 0;
    public UnityEvent emptyResource;
    public static ResourceManager instance;

    public float getResourceMulti { get { return 1; } }
    public int Resources { get { return resources; } }

    private void Awake()
    {
        if (emptyResource == null)
            emptyResource = new UnityEvent();
        if (instance == null)
            instance = this;
    }
    void Start () {
        
        resources = 50;
	}

    public bool addResources(int amount)
    {
        resources += Mathf.FloorToInt(amount * getResourceMulti);
        return true;
    }

    public bool subResources(int amount)
    {
        resources -= amount;
        if(resources < 0)
        {
            emptyResource.Invoke();
            return false;
        }
        return true;
    }
}
