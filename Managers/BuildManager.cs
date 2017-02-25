using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour {

    public enum ModualTypes { Damage, AttackRate, DebuffDuration, ProjectileSpeed, Range, ArmorReduction, Curse, Dot, EnergyLeech, HpRegenReduction, Slow, Splash}

    public GameObject[] Buildings;
    public GameObject[] Gun;
    public GameObject[] GunBase;
    [Tooltip("1  = Damage\n2  = AttackRate\n3  = DebuffDuration\n4  = ProjectileSpeed\n5  = Range\n6  = ArmorReduction\n7  = Curse\n8  = Dot\n9  = EnergyLeech\n10 = HpRegenReduction\n11 = Slow\n12 = Splash")]
    public Sprite[] modualsIcon;
    public Sprite[] weaponModualsIcon;
    private static BuildManager buildManager;
    private bool[] SelectedModuals;

    private int selectedCount;

    public static BuildManager instance
    {
        get { return buildManager; }
    }

    void Start()
    {
        buildManager = this;

        SelectedModuals = new bool[System.Enum.GetNames(typeof(ModualTypes)).Length];
        for (int i = 0; i < SelectedModuals.Length; i++)
        {
            SelectedModuals[i] = false;
        }
    }

    public void SpawnBuilding(int buildNumber)
    {
        Instantiate(Buildings[buildNumber]).GetComponent<PlaceholderBuilding>();                   
    }

    public bool selectModual(ModualTypes select)
    {

        SelectedModuals[(int)select] = !SelectedModuals[(int)select];
        if (SelectedModuals[(int)select])
            selectedCount++;
        else
            selectedCount--;
        if(selectedCount > 2)
        {
            SelectedModuals[(int)select] = false;
            selectedCount--;
            ErrorMessangerManager.instance.DisplayError("To many moduals selected");
            return false;
        }
        return SelectedModuals[(int)select];

    }

    public void CraftModual()
    {
        int[] selectedModuals = GetSelectedElements();
        if (selectedCount == 0)
            return;
        else if(selectedCount == 1)
        {
            UpgradeModual modual = new UpgradeModual(modualsIcon[selectedModuals[0]]);
            switch ((ModualTypes)selectedModuals[0])
            {
                case ModualTypes.ArmorReduction:
                    modual.armorReduce = 10;                 
                    break;
                case ModualTypes.AttackRate:
                    modual.attackRate = 1;
                    break;
                case ModualTypes.Curse:
                    modual.curse = 10;
                    break;
                case ModualTypes.Damage:
                    modual.damage = 10;
                    break;
                case ModualTypes.DebuffDuration:
                    modual.duration = 10;
                    break;
                case ModualTypes.Dot:
                    modual.DOT = 10;
                    break;
                case ModualTypes.EnergyLeech:
                    modual.leech = 10;
                    break;
                case ModualTypes.HpRegenReduction:
                    modual.hpRegReduce = 2;
                    break;
                case ModualTypes.ProjectileSpeed:
                    modual.projectileSpeed = 20;
                    break;
                case ModualTypes.Range:
                    modual.range = 15;
                    break;
                case ModualTypes.Slow:
                    modual.slow = 10;
                    break;
                case ModualTypes.Splash:
                    modual.splash = 10;
                    break;
                default:
                    ErrorMessangerManager.instance.DisplayError("Modual Craft Error select modual dosent exist");
                    break;
                
            }
            InventoryManager.Instance.AddInventory(modual);
            InventoryManager.Instance.UpdateInventoryPosition();
        }
        else if(selectedCount == 2)
        {
            ErrorMessangerManager.instance.DisplayError("Mix modual not implementet(BuildManager)");
        }
        else
        {
            ErrorMessangerManager.instance.DisplayError("Error to many moduals selected(BuildManager)");
        }
    }

    public void CraftWeaponModual(int weapon)
    {
        WeaponModual newWeaponModual = new WeaponModual(weaponModualsIcon[weapon]);
        switch (weapon)
        {
            case 1:
                //some code to specialse weapon
                break;
        }
        InventoryManager.Instance.AddInventory(newWeaponModual);
        InventoryManager.Instance.UpdateInventoryPosition();
    }

    private int[] GetSelectedElements()
    {
        int[] elements = new int[selectedCount];
        int count = 0;
        for(int i = 0; i < SelectedModuals.Length; i++)
        {
            if (SelectedModuals[i])
            {
                elements[count] = i;
                count++;
            }
        }
        return elements;
    }
}
