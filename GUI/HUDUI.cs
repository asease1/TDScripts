using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDUI : MonoBehaviour {

    

    [Range(0, 1)]
    public float updateFrequency;
    public Text waveCount;
    public Text monsterReserve;
    public Text resource;

    private GameObject helpBox;
    private GameObject breakLine;

    private List<Text> textFields;
    private List<RectTransform> breakLines;

    [Header("Custom HelpBox")]
    public int fontSize = 26;
    public float bufferSpace = 3;
    public Font font;

    private void Start()
    {
        //StartCoroutine(updateUI());

        textFields = new List<Text>();
        breakLines = new List<RectTransform>();

        helpBox = Instantiate(((GameObject)Resources.Load("Prefab/UI/HelpBox", typeof(GameObject))), transform);
        helpBox.active = false;
        breakLine = (GameObject)Resources.Load("Prefab/UI/BreakLine", typeof(GameObject));
    }

    IEnumerator updateUI()
    {
        while (true)
        {
            waveCount.text = Spawn.instance.WaveCount.ToString();
            monsterReserve.text = Spawn.instance.monsterQueueCount.ToString();
            resource.text = ResourceManager.instance.Resources.ToString();      
            yield return new WaitForSeconds(updateFrequency);
        }
    }

    public void setGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    public void createHelpBox(string filePath)
    {
        UnityEngine.Object file = Resources.Load(filePath);
        string[] text = System.Text.RegularExpressions.Regex.Split(System.IO.File.ReadAllText(AssetDatabase.GetAssetPath(file)),"</break>");
        for (int i = 1; i < text.Length; i++)
            text[i] = text[i].TrimStart(System.Environment.NewLine.ToCharArray());

        Resources.UnloadAsset(file);
        float currentPosition = 5;
        for(int i = 0; i < text.Length*2-1; i++)
        {
            if(i%2 == 1)
            {
                if (breakLines.Count < Mathf.FloorToInt(i / 2)+1)
                    breakLines.Add(Instantiate(breakLine, helpBox.transform).GetComponent<RectTransform>());

                RectTransform temp = breakLines[Mathf.FloorToInt(i / 2)];
                temp.anchoredPosition = new Vector2(0, -currentPosition);
                currentPosition += (temp.sizeDelta.y + bufferSpace) * (1 - GetComponent<Canvas>().scaleFactor + 1);
            }
            else
            {
                if (textFields.Count < Mathf.CeilToInt(i / 2)+1)
                    textFields.Add(createTextUI());

                Text temp = textFields[Mathf.FloorToInt(i / 2)];

                temp.font = font;
                temp.fontSize = fontSize;
                temp.text = text[Mathf.FloorToInt(i / 2)];
                temp.rectTransform.anchoredPosition = new Vector2(0, -currentPosition);
             
                TextGenerationSettings generationSettings = temp.GetGenerationSettings(temp.rectTransform.rect.size);
                float height = temp.cachedTextGenerator.GetPreferredHeight(text[Mathf.FloorToInt(i / 2)], generationSettings) + bufferSpace;
                height *= (1 - GetComponent<Canvas>().scaleFactor + 1);
                currentPosition += height;
            }
            
        }
        helpBox.active = true;
    }

    public void dismisHelpBox()
    {
        helpBox.active = false;
    }

    private Text createTextUI()
    {
        Text test = new GameObject().AddComponent<Text>();
        test.transform.SetParent(helpBox.transform);
        RectTransform textTransform = test.GetComponent<RectTransform>();
        textTransform.anchorMax = new Vector2(0.5f, 1);
        textTransform.anchorMin = new Vector2(0.5f, 1);
        textTransform.anchoredPosition = Vector3.zero + Vector3.down * 5 ;
        textTransform.localScale = Vector3.one;
        textTransform.sizeDelta = new Vector2(500, 0);
        test.verticalOverflow = VerticalWrapMode.Overflow;
        test.alignment = TextAnchor.UpperCenter;
        
        return test;
    }


    private Transform modualParrent;
    ITowerModual towerModual;
    public void beginDragModual(Object caller)
    {
        modualParrent = ((GameObject)caller).transform.parent;
        ((GameObject)caller).transform.SetParent(gameObject.transform);
    }

    public void draggingModual(Object caller)
    {
        ((GameObject)caller).transform.position = Input.mousePosition;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1 << 12))
        {
            Transform objectHit = hit.transform;

            if (towerModual != null)
                towerModual.MouseHoverExit();

            towerModual = hit.transform.gameObject.GetComponent<ITowerModual>();
            if(towerModual != null)
            {
                towerModual.MouseHoverEnter();
            }
        }
    }

    public void endDragModual(Object caller)
    {
        ((GameObject)caller).transform.SetParent(modualParrent);
        if(towerModual != null)
        {
            towerModual.SetModual(caller);
            towerModual.MouseHoverExit();
            towerModual = null;
        }
    }
}
