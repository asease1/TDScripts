using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
public class InventoryManager : MonoBehaviour {

    public Vector2 UISize;
    public float UIspace;
    public Vector2 BackGroundSize;
    public Vector3 BackgroundPosition;
    public RectTransform canvasTransfrom;
    public Sprite UIBackground;
    public Sprite ModualSlotSprite;

    public Sprite test;

    private Image BackGroundImage;
    private RectTransform modualParrent;
    public List<UpgradeModual> inventory = new List<UpgradeModual>();
    public List<Image> ModualUIElements = new List<Image>();

    private static InventoryManager instance;

    private int amountElementHorisotal { get { return Mathf.FloorToInt((BackGroundSize.x - UIspace) / (UISize.x + UIspace)); } }
    private int amountElementVertical { get { return Mathf.CeilToInt(inventory.Count / amountElementHorisotal) + 1; } }
    public static InventoryManager Instance
    {
        get { return instance; }
    }

    public RectTransform ModualParrant { get { return modualParrent; } }

	// Use this for initialization
	void Start () {
        instance = this;

        BackGroundImage = CreateUIImage(canvasTransfrom);
        SetupInventoryBackground();
        SetupInventoryModuelSlots();
        SetupInventoryModuels();

        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
        AddInventory(new WeaponModual(test));
    }

    public void AddInventory(UpgradeModual item)
    {
        inventory.Add(item);
        if(inventory.Count > ModualUIElements.Count-1)
        {
            int tempElementHorisotal = amountElementHorisotal;
            int tempElementVertical = amountElementVertical;
            for (int x = 0; x < tempElementHorisotal; x++)
            {
                //Setup new modual sprites
                Image tempImage = new GameObject((x + ((amountElementVertical-1) * tempElementHorisotal)).ToString(), typeof(Image)).GetComponent<Image>();
                ModualUIElements.Add(tempImage);
                CustomEventTrigger tempEvent = tempImage.gameObject.AddComponent<CustomEventTrigger>();
                tempEvent.dragType = CustomEventTrigger.myDragType.inventory;

                tempImage.rectTransform.SetParent(modualParrent);
                tempImage.rectTransform.localScale = Vector3.one;
                tempImage.rectTransform.sizeDelta = UISize;
                tempImage.rectTransform.localPosition = GetElementPositionFromIndex(x, amountElementVertical-1);
                if (x + ((amountElementVertical -1) * tempElementHorisotal) < inventory.Count)
                {
                    tempImage.sprite = inventory[x + ((amountElementVertical-1) * tempElementHorisotal)].Icon;
                }
                else
                    tempImage.gameObject.SetActive(false);
                //setup modual slots
                tempImage = new GameObject("Slots" + (x + ((amountElementVertical - 1) * tempElementHorisotal)).ToString(), typeof(Image)).GetComponent<Image>();
                tempImage.raycastTarget = false;
                tempImage.rectTransform.SetParent(modualParrent);
                tempImage.rectTransform.localScale = Vector3.one;
                tempImage.rectTransform.sizeDelta = UISize;
                tempImage.rectTransform.localPosition = GetElementPositionFromIndex(x, amountElementVertical );
                tempImage.sprite = ModualSlotSprite;
            }
        }
    }
	
	private void SetupInventoryBackground()
    {
        BackGroundImage.gameObject.AddComponent<Mask>();
        ScrollRect tempScroll = BackGroundImage.gameObject.AddComponent<ScrollRect>();
        modualParrent = new GameObject("ImageMove", typeof(RectTransform)).GetComponent<RectTransform>();
        modualParrent.parent = BackGroundImage.rectTransform;
        tempScroll.content = modualParrent;
        tempScroll.horizontal = false;
        tempScroll.scrollSensitivity = 10;

        float x = BackGroundSize.x - ((BackGroundSize.x - UIspace) % (UISize.x + UIspace));
        float y = BackGroundSize.y - ((BackGroundSize.y - UIspace) % (UISize.y + UIspace));

        int amountElementHorisotal = Mathf.FloorToInt((BackGroundSize.x - UIspace) / (UISize.x + UIspace));

        modualParrent.sizeDelta = new Vector2(x, (UISize.y+UIspace) * (Mathf.CeilToInt(inventory.Count / amountElementHorisotal)+1)+UIspace);
        
        BackGroundImage.rectTransform.sizeDelta = new Vector2(x, y);

        BackGroundImage.rectTransform.localPosition = BackgroundPosition - new Vector3(0, canvasTransfrom.sizeDelta.y *0.5f,0) + new Vector3(0, y*0.5f,0);
        BackGroundImage.sprite = UIBackground;
        BackGroundImage.rectTransform.localScale = new Vector3(1, 1, 1);
        BackGroundImage.type = Image.Type.Sliced;


    }

    private void SetupInventoryModuels()
    {
        int tempElementHorisotal = amountElementHorisotal;
        int tempElementVertical = amountElementVertical;

        for(int y = 0; y < tempElementVertical; y++)
        {
            for(int x = 0; x < tempElementHorisotal; x++)
            {
                Image tempImage = new GameObject((x + (y * tempElementHorisotal)).ToString(), typeof(Image)).GetComponent<Image>();
                ModualUIElements.Add(tempImage);
                CustomEventTrigger tempEvent = tempImage.gameObject.AddComponent<CustomEventTrigger>();
                tempEvent.dragType = CustomEventTrigger.myDragType.inventory;

                tempImage.rectTransform.SetParent(modualParrent);
                tempImage.rectTransform.localScale = Vector3.one;
                tempImage.rectTransform.sizeDelta = UISize;
                tempImage.rectTransform.localPosition = GetElementPositionFromIndex(x, y);
                if (x + (y * tempElementHorisotal) < inventory.Count)
                {
                    tempImage.sprite = inventory[x + (y * tempElementHorisotal)].Icon;
                }
                else
                    tempImage.gameObject.SetActive(false);
            }
        }
    }

    private void SetupInventoryModuelSlots()
    {
        int tempElementHorisotal = amountElementHorisotal;
        int tempElementVertical = amountElementVertical;

        for (int y = 0; y < tempElementVertical; y++)
        {
            for (int x = 0; x < tempElementHorisotal; x++)
            {
                Image tempImage = new GameObject("Slots"+(x + (y * tempElementHorisotal)).ToString(), typeof(Image)).GetComponent<Image>();
                tempImage.raycastTarget = false;
                tempImage.rectTransform.SetParent( modualParrent);
                tempImage.rectTransform.localScale = Vector3.one;
                tempImage.rectTransform.sizeDelta = UISize;
                tempImage.rectTransform.localPosition = GetElementPositionFromIndex(x, y);
                tempImage.sprite = ModualSlotSprite;
            }
        }
    }

    public int GetElementFromPosition(GameObject element)
    {
        Transform parent = element.transform.parent;
        //Convert to the right space
        element.transform.SetParent(modualParrent);
        Vector3 position = element.transform.localPosition;

        int xposition = Mathf.FloorToInt( ((position.x+ modualParrent.sizeDelta.x/2) - UIspace) / (UIspace+UISize.x));
        int yposition = amountElementVertical-1 - Mathf.FloorToInt(((position.y + modualParrent.sizeDelta.y / 2) - UIspace) / (UIspace + UISize.y));
        element.transform.SetParent(parent);
        return yposition* amountElementHorisotal + xposition;
    }

    private Vector3 GetElementPositionFromIndex(int index)
    {
        int x = index% amountElementHorisotal;
        int y = (index - x) / amountElementVertical;
        return new Vector3((-modualParrent.sizeDelta.x * 0.5f) + UIspace + UISize.x * 0.5f + x * UISize.x + x * UIspace, (-modualParrent.sizeDelta.y * 0.5f) + UIspace + UISize.y * 0.5f + (amountElementVertical - y - 1) * UISize.y + (amountElementVertical - y - 1) * UIspace, 0);
    }

    private Vector3 GetElementPositionFromIndex(int x, int y)
    {
        return new Vector3((-modualParrent.sizeDelta.x * 0.5f) + UIspace + UISize.x * 0.5f + x * UISize.x + x * UIspace, (-modualParrent.sizeDelta.y * 0.5f) + UIspace + UISize.y * 0.5f + (amountElementVertical - y - 1) * UISize.y + (amountElementVertical - y - 1) * UIspace, 0);
    }

    public void UpdateInventoryPosition()
    {
        for(int y = 0; y < amountElementVertical; y++)
        {
            for(int x = 0; x < amountElementHorisotal; x++)
            {
                ModualUIElements[y * amountElementHorisotal + x].transform.SetParent(modualParrent);
                ModualUIElements[y*amountElementHorisotal + x].rectTransform.localPosition = GetElementPositionFromIndex(x, y);
                if (y * amountElementHorisotal + x < inventory.Count)
                {
                    ModualUIElements[y * amountElementHorisotal + x].gameObject.SetActive(true);
                    ModualUIElements[y * amountElementHorisotal + x].sprite = inventory[y * amountElementHorisotal + x].Icon;
                }
                else
                    ModualUIElements[y * amountElementHorisotal + x].gameObject.SetActive(false);
            }
        }
    }

    public void SwapInventoryIndex(int element1, int element2)
    {
        if (element2 < inventory.Count && element2 >= 0)
        {
            UpgradeModual tempInv = inventory[element1];
            inventory[element1] = inventory[element2];
            inventory[element2] = tempInv;
            UpdateInventoryPosition();
        }
    }

    public void SwapTowerInventory(int localModual, Image towerModual)
    {
        UpgradeModual tempUpgrade = TowerModualUIManager.instance.towerStats.SwitfModual(TowerModualUIManager.instance.GetIndexOfImage(towerModual), inventory[localModual]);

        if(tempUpgrade != null)
        {
            inventory[localModual] = tempUpgrade;
            TowerModualUIManager.instance.UpdateModual();
        }
        UpdateInventoryPosition();
    }

    private Image CreateUIImage(RectTransform parrent)
    {
        RectTransform tempRect = new GameObject("BackgroundImage", typeof(Image)).GetComponent<RectTransform>();
        tempRect.parent = parrent;

        return tempRect.GetComponent<Image>();
    }
}
