using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public class MyUpgradeModualEvent : UnityEvent<UpgradeStats> { }

public class ModualsEvent : UnityEvent<UpgradeModual[]> { }

public class TowerUpgrade : MonoBehaviour
{
    public UpgradeModual[] Moduals;
    public MyUpgradeModualEvent UpgradeUpdateEvent;
    public UpgradeStats myStats;
    public ModualsEvent ChangeWeaponModual;


    public int MaxModualAmount;
    private int equipWeaponModuals, equipUpgradeModuals;

    public int amountEquipModuals
    {
        get { return equipWeaponModuals + equipUpgradeModuals; }
    }

    void Awake()
    {
        myStats = new UpgradeStats();
        if (UpgradeUpdateEvent == null)
            UpgradeUpdateEvent = new MyUpgradeModualEvent();
        if (ChangeWeaponModual == null)
            ChangeWeaponModual = new ModualsEvent();
        
    }

    void Start()
    {
        Moduals = new UpgradeModual[MaxModualAmount];
        AddModual(new WeaponModual(new Sprite()));
        AddModual(new UpgradeModual(new Sprite()));
        AddModual(new UpgradeModual(new Sprite()));
    }

   public bool AddModual(UpgradeModual modual)
    {
        if (modual.GetType() == typeof(WeaponModual))
        {
            if (equipWeaponModuals >= 2 || amountEquipModuals >= MaxModualAmount)
            {
                Debug.Log("there is all ready 2 weapon moduals or to many moduals");
                return false;
            }
            if(Moduals[equipWeaponModuals] != null)
            {
                Moduals[equipUpgradeModuals + 1] = Moduals[equipWeaponModuals];
                Moduals[equipWeaponModuals] = modual;
            }
            else
                Moduals[equipWeaponModuals] = modual;
            equipWeaponModuals++;
            UpdateTowerStats();
            ChangeWeaponModual.Invoke(Moduals);
            return true;
        }
        else
        {
            if(amountEquipModuals < MaxModualAmount && equipUpgradeModuals < MaxModualAmount-1)
            {
                Moduals[equipUpgradeModuals + equipWeaponModuals] = modual;
                equipUpgradeModuals++;
                UpdateTowerStats();
                return true;
            }
            Debug.Log("To many moduals equip");
            return false;
        }
    }

    private void UpdateTowerStats()
    {
        myStats.ResetValues();
        foreach(UpgradeModual elm in Moduals)
        {
            if (elm != null)
            {
                myStats.PlusTwoUpgradeStats(elm.GetUpgradeStats());
                
            }
            
        }
        if (myStats.attackRate <= 0)
            myStats.attackRate = 120;

        UpgradeUpdateEvent.Invoke(myStats);
    }

    public bool RemoveModual(UpgradeModual modual)
    {
        return false;
    }

    public UpgradeModual SwitfModual(int index, UpgradeModual newModual)
    {
        UpgradeModual tempUpgrade = Moduals[index];
        if(newModual.GetType() == typeof(WeaponModual))
        {
            ChangeWeaponModual.Invoke(Moduals);
            switch (index)
            {
                case 0:
                case 1:
                    if (Moduals[index].GetType() == typeof(UpgradeModual))
                    {
                        equipUpgradeModuals--;
                        equipWeaponModuals++;
                    }
                    Moduals[index] = newModual;
                    UpdateTowerStats();
                    return tempUpgrade;
                default:
                    Debug.Log("This is not a weapon slots");
                    return null;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    ErrorMessangerManager.instance.DisplayError("This is slot can only use weapon modual");
                    return null;
                case 1:
                    Moduals[index] = newModual;
                    if (tempUpgrade.GetType() == typeof(WeaponModual))
                    {
                        equipWeaponModuals--;
                        equipUpgradeModuals++;
                        ChangeWeaponModual.Invoke(Moduals);
                    }
                    UpdateTowerStats();
                    return tempUpgrade;
                default:
                    Moduals[index] = newModual;
                    UpdateTowerStats();
                    return tempUpgrade;
            }
        }
    }
}
