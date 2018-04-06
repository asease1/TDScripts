using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeModual : MonoBehaviour, ITowerModual
{
    private Material material;
    Color standartColor;

    public Transform modualSlot;
    private ITowerModual[] group;

    private static GameObject TestWeaponPrefab;
    GameObject testWeaponPrefab
    {
        get
        {
            if (TestWeaponPrefab == null)
                TestWeaponPrefab = (GameObject)Resources.Load("Prefab/Upgrade", typeof(GameObject));
            return TestWeaponPrefab;
        }
    }

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
        GameObject weapon = Instantiate(testWeaponPrefab);
        weapon.transform.SetParent(transform);
        weapon.transform.position = modualSlot.position;
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
