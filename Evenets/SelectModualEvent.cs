using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class SelectModualEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

    public GameObject graphics;

    public BuildManager.ModualTypes modualSelect; 


    public void OnPointerClick(PointerEventData eventData)
    {}

    public void OnPointerDown(PointerEventData eventData)
    {
        graphics.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        graphics.SetActive(BuildManager.instance.selectModual(modualSelect));
    }
}
