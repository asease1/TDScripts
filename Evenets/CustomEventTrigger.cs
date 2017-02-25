using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomEventTrigger : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public enum myDragType { inventory, towerModual};
    public myDragType dragType;

    public void OnBeginDrag(PointerEventData eventData)
    {       
        RectTransform temp = GetComponent<RectTransform>();
        temp.SetParent(InventoryManager.Instance.canvasTransfrom);
        temp.localScale = Vector3.one;
        temp.eulerAngles = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;     
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        gameObject.transform.SetParent(InventoryManager.Instance.ModualParrant);
        Vector3 tempVec = transform.localPosition;
        tempVec.x += InventoryManager.Instance.ModualParrant.sizeDelta.x / 2;
        tempVec.y += InventoryManager.Instance.ModualParrant.sizeDelta.y / 2;
        if (tempVec.x >= 0 && tempVec.y >= 0 && tempVec.x < InventoryManager.Instance.ModualParrant.sizeDelta.x && tempVec.y < InventoryManager.Instance.ModualParrant.sizeDelta.y)
        {
            int index = InventoryManager.Instance.GetElementFromPosition(gameObject);
            if (dragType == myDragType.inventory)
                InventoryManager.Instance.SwapInventoryIndex(InventoryManager.Instance.ModualUIElements.IndexOf(gameObject.GetComponent<Image>()), index);
            else if (dragType == myDragType.towerModual)
                InventoryManager.Instance.SwapTowerInventory(index, gameObject.GetComponent<Image>());
        }

        else 
        {
            bool swaped = false;
            //Check if i hit some of the active Modual UI for the towers
            if (TowerModualUIManager.instance != null)
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current);

                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
                foreach (RaycastResult elm in results)
                {

                    if (elm.gameObject.layer == LayerMask.NameToLayer("UI"))
                    {
                        Image tempImage = elm.gameObject.GetComponent<Image>();
                        if (TowerModualUIManager.instance.ContainsImage(tempImage))
                        {
                            InventoryManager.Instance.SwapTowerInventory(InventoryManager.Instance.ModualUIElements.IndexOf(gameObject.GetComponent<Image>()), tempImage);
                            swaped = true;
                            break;
                        }
                    }
                }
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!swaped && dragType == myDragType.inventory && Physics.Raycast(ray, out hit, TowerModualUIManager.instance.towerLayer))
            {
                int index = InventoryManager.Instance.ModualUIElements.IndexOf(gameObject.GetComponent<Image>());
                if (hit.transform.gameObject.GetComponentInChildren<TowerUpgrade>().AddModual(InventoryManager.Instance.inventory[index]))
                {
                    InventoryManager.Instance.inventory.RemoveAt(index);     
                }
            }
            TowerModualUIManager.instance.UpdateModual();
            InventoryManager.Instance.UpdateInventoryPosition();
        }
    }
}
