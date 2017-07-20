using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUICirkelLine : MonoBehaviour {

    public Transform centerPoint;
    public float offset;
    public float lineDistance;
    [Tooltip("The max amount of stones shown, with the same distance between all stone")]
    public int maxVisableStones = 2;
    [Tooltip("Keep under minimum stone time, it in seconds")]
    public float moveTime = 1;
    public float UIDropTime = 5;
    private float maxTimeValue = 30;
    private float circumference;
    private float elementDistance;
    private RectTransform canvas;
    private GameObject[] monsterStoneUI;
    private GameObject baseStone;
    private Material[] spawnTimers;
    private Queue<GameObject> passiveDropElements = new Queue<GameObject>();

    private float getMovementTime { get { return moveTime / Spawn.instance.waveSpeed; } }
    private float getDropTime { get { return moveTime / Spawn.instance.waveSpeed; } }

    // Use this for initialization
    void Start()
    {
        spawnTimers = new Material[maxVisableStones];
        monsterStoneUI = new GameObject[maxVisableStones];
        canvas = GameObject.FindGameObjectWithTag("ScreenCanvas").GetComponent<RectTransform>();
        baseStone = (GameObject)Resources.Load("UI/SpawnStone", typeof(GameObject));

        circumference = offset * 2 * Mathf.PI/4;
        elementDistance = (circumference + lineDistance) / (maxVisableStones -1);

        for (int i = 0; i < Spawn.instance.spawnQueue.Count && i < maxVisableStones; i++)
        {
            monsterStoneUI[i] = Instantiate(baseStone);
            monsterStoneUI[i].transform.parent = canvas;
            monsterStoneUI[i].transform.localScale = Vector3.one;

            float distence = elementDistance * i;
            if(distence <= lineDistance)
            {
                monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + distence, centerPoint.position.y - offset, 0);
                monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                float cirkelProcent = (distence - lineDistance) / circumference;
                float radians = (270 + 90f * cirkelProcent) * (Mathf.PI / 180);
                monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + lineDistance + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
                monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0,90*cirkelProcent);
            }
            
            

            spawnTimers[i] = Instantiate(monsterStoneUI[i].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("SpawnTimerBG").Find("SpawnTimer").GetComponent<Image>().material);
            monsterStoneUI[i].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("SpawnTimerBG").Find("SpawnTimer").GetComponent<Image>().material = spawnTimers[i];
            updateMonsterStoneInfo(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn.instance.spawnQueue.Count > 0)
            spawnTimers[0].SetFloat("_Health", (Spawn.instance.spawnQueue[0].maxTime - Spawn.instance.WaveTimer) / maxTimeValue);
    }

    public void newWaveCall()
    {
        for (int i = 0; i < maxVisableStones; i++)
        {
            if (i < Spawn.instance.spawnQueue.Count)
                updateMonsterStoneInfo(i);
            else
                monsterStoneUI[i].active = false;
        }

        StopCoroutine(MoveUI());
        StartCoroutine(MoveUI());
        StartCoroutine(DropUI());
    }

    private void updateMonsterStoneInfo(int index)
    {
        Image monsterType = monsterStoneUI[index].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("Type").GetComponent<Image>();
        monsterType.sprite = Spawn.instance.spawnQueue[index].getTypeImage();
        Transform abilityParentPointer = monsterStoneUI[index].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("[Vertical]Abilities");
        
        for (int i = 0; i < abilityParentPointer.childCount; i++)
        {
            Debug.Log(abilityParentPointer.childCount + "");
            if (i < Spawn.instance.spawnQueue[index].abilitys.Count)
            {
                abilityParentPointer.GetChild(i).gameObject.active = true;
                Image monsterAbility = abilityParentPointer.GetChild(i).GetComponent<Image>();
                monsterAbility = Spawn.instance.spawnQueue[index].getAblityImage(i);
            }
            else
            {
                abilityParentPointer.GetChild(i).gameObject.active = false;
            }
        }
        spawnTimers[index].SetFloat("_Health", (Spawn.instance.spawnQueue[index].maxTime - Spawn.instance.WaveTimer) / maxTimeValue);
    }

    IEnumerator MoveUI()
    {
        float timer = 0;
        while (true)
        {
            timer +=  Time.fixedDeltaTime / getMovementTime;

            for (int i = 0; i < Spawn.instance.spawnQueue.Count && i < maxVisableStones; i++)
            {
                float distence = elementDistance * (i + 1f - timer);
                if (distence <= lineDistance)
                {
                    monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + distence, centerPoint.position.y - offset, 0);
                    monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    float cirkelProcent = (distence - lineDistance) / circumference;
                    float radians = (270 + 90f * cirkelProcent) * (Mathf.PI / 180);
                    monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + lineDistance + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
                    monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 90f * cirkelProcent);
                }
            }
            if (timer >= 1)
                break;
            

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DropUI()
    {
        float timer = 0;
        GameObject UIPointer;
        if (passiveDropElements.Count > 0)
        {
            UIPointer = passiveDropElements.Dequeue();
            UIPointer.active = true;
            float radians = (270 + (90f / (maxVisableStones - 1)) * 0) * (Mathf.PI / 180);
            UIPointer.transform.position = new Vector3(centerPoint.position.x + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
        }
        else
        {
            UIPointer = Instantiate(baseStone);
            UIPointer.transform.parent = canvas;
            UIPointer.transform.localScale = Vector3.one;
            float radians = (270 + (90f / (maxVisableStones - 1)) * 0) * (Mathf.PI / 180);
            UIPointer.transform.position = new Vector3(centerPoint.position.x + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
        }

        Vector3 startPosition = new Vector3(UIPointer.transform.localPosition.x, UIPointer.transform.localPosition.y, UIPointer.transform.localPosition.z);

        while (true)
        {
            timer += Time.fixedDeltaTime / getDropTime;

            Vector3 newPosition = new Vector3(startPosition.x, startPosition.y - (canvas.rect.height - startPosition.y) * timer, 0);
            UIPointer.transform.localPosition = newPosition;
            if (timer >= 1)
            {
                UIPointer.active = false;
                passiveDropElements.Enqueue(UIPointer);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
