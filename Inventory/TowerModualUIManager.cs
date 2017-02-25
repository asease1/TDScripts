using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class TowerModualUIManager : MonoBehaviour {

    private static TowerModualUIManager Instance;
    public static TowerModualUIManager instance { get { return Instance; } }

    [Tooltip("Define the size of the canvas in the scene.\nThe canvas will be scaled down 0.02")]
    public Vector2 CanvasSize;
    [Tooltip("Define the modual size in pixel.\nsize is relative to canvas")]
    public Vector2 ModualSize;
    public float OffsetCenter;
    [Tooltip("Offset of the canvas in the y axis reletive to the tower")]
    public float towerOffset;
    public LayerMask towerLayer;

    private TowerUpgrade TowerStats;
    private RectTransform canvas;
    private List<Image> modualElements = new List<Image>();

    public TowerUpgrade towerStats { get { return TowerStats; } }

    void Start()
    {
        Instance = this;
        canvas = new GameObject("TowerModualUI", typeof(Canvas)).GetComponent<RectTransform>();
        canvas.gameObject.AddComponent<GraphicRaycaster>();
        canvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        canvas.sizeDelta = CanvasSize;
        canvas.localScale = new Vector3(0.02f, 0.02f, 1f);
    }

    void Update()
    {
        if(TowerStats != null)
        {
            canvas.position = TowerStats.gameObject.transform.position + Vector3.up * towerOffset;
            canvas.transform.LookAt(Camera.main.transform);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, towerLayer))
            {
                canvas.gameObject.SetActive(true);
                TowerStats = hit.transform.gameObject.GetComponentInChildren<TowerUpgrade>();
                SetUpModuals();
                UpdateModual();
            }
            else if(!EventSystem.current.IsPointerOverGameObject())
            {
                if (towerStats != null)
                    canvas.gameObject.SetActive(false);
                TowerStats = null;
            }
        }            
    }

    void SetUpModuals()
    {
        for(int i = modualElements.Count; i < TowerStats.Moduals.Length; i++)
        {
            modualElements.Add(new GameObject("modual" + i, typeof(Image)).GetComponent<Image>());
            modualElements[i].gameObject.AddComponent<CustomEventTrigger>().dragType = CustomEventTrigger.myDragType.towerModual;
            modualElements[i].gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }

    public void UpdateModual()
    {
        if (towerStats == null)
            return;
        for (int i = 0; i < towerStats.MaxModualAmount; i++)
        {
            if (towerStats.Moduals[i] != null)
            {
                modualElements[i].gameObject.SetActive(true);
                modualElements[i].transform.SetParent(canvas);
                //x is cos(rediant)
                float x = Mathf.Cos(((Mathf.PI / 2f) + Mathf.PI * 2 * (i / (float)towerStats.MaxModualAmount))) * OffsetCenter;
                //y is sin(rediant)
                float y = Mathf.Sin(((Mathf.PI / 2f) + Mathf.PI * 2 * (i / (float)towerStats.MaxModualAmount))) * OffsetCenter;
                modualElements[i].rectTransform.localPosition = new Vector3(x, y, 0);
                modualElements[i].rectTransform.sizeDelta = ModualSize;
                modualElements[i].transform.localEulerAngles = new Vector3(0, 180, 0);
                modualElements[i].rectTransform.localScale = Vector3.one;
                modualElements[i].sprite = towerStats.Moduals[i].Icon;
            }
            else
                modualElements[i].gameObject.SetActive(false);
        }
    }

    public bool ContainsImage(Image item)
    {
        return modualElements.Contains(item);
    }

    public int GetIndexOfImage(Image item)
    {
        return modualElements.IndexOf(item);
    }
}
