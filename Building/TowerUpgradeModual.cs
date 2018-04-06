using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeModual : MonoBehaviour, ITowerModual
{
    private Material material;
    Color standartColor;

    public Transform modualSlot;
    private ITowerModual[] group;

    private GameObject modualHead;
    private GameObject instateHead;
    [HideInInspector]
    public ModualUpgradeSlot upgradeSlot;

    public void MouseHoverEnter()
    {
        material.color = Color.white;
        foreach (ITowerModual elm in group)
            elm.GroupHoverEnter();
    }

    public void MouseHoverExit()
    {
        material.color = standartColor;
        foreach (ITowerModual elm in group)
            elm.GroupHoverExit();
    }

    public void SetModual(Object caller)
    {
        ModualUpgradeSlot upgradeSlot = ((GameObject)caller).GetComponent<ModualUpgradeSlot>();
        if (upgradeSlot != null)
        {
            this.upgradeSlot = upgradeSlot;
            Destroy(caller);
            transform.parent.GetComponentInChildren<TurretScript>().updateStats();
            modualHead = ((GameObject)Resources.Load("Prefabs/modules/" + upgradeSlot.modualModel));
            Destroy(instateHead);
            instateHead = Instantiate(modualHead);
            instateHead.transform.SetParent(transform);
            instateHead.transform.position = modualSlot.position;
        }
        
    }

    // Use this for initialization
    void Start()
    {
        material = GetComponent<Renderer>().material;
        standartColor = material.color;
        ITowerModual[] tempTowers = transform.parent.GetComponentsInChildren<ITowerModual>();
        group = new ITowerModual[tempTowers.Length - 1];
        int i = 0;
        foreach (ITowerModual elm in tempTowers)
        {
            if(elm != this)
            {
                group[i] = elm;
                i++;
            }


        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GroupHoverEnter()
    {
        material.color = Color.cyan;
    }

    public void GroupHoverExit()
    {
        material.color = standartColor;
    }
}
