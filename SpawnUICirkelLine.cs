using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUICirkelLine : MonoBehaviour {

    public RectTransform centerPoint;
    public float offset;
    public float lineDistance;
    [Tooltip("The max amount of stones shown, with the same distance between all stone")]
    public int maxVisableStones = 2;
    [Tooltip("Keep under minimum stone time, it in seconds")]
    public float moveTime = 1;
    public float UIDropTime = 5;

    private float dealyBetweenWaves = 0;
    private float maxTimeValue = 30;
    private float circumference;
    private float elementDistance;
    private RectTransform canvas;
    private GameObject[] monsterStoneUI;
    private GameObject baseStone;
    private Material[] spawnTimers;
    private Queue<RectTransform> passiveDropElements = new Queue<RectTransform>();
    public Transform Test;

    private float getMovementTime { get { return moveTime / Spawn.instance.waveSpeed; } }
    private float getDropTime { get { return UIDropTime / Spawn.instance.waveSpeed; } }

    // Use this for initialization
    void Start()
    {

        Spawn.instance.newWave.AddListener(newWaveCall);

        spawnTimers = new Material[maxVisableStones];
        monsterStoneUI = new GameObject[maxVisableStones];
        canvas = GameObject.FindGameObjectWithTag("ScreenCanvas").GetComponent<RectTransform>();
        baseStone = (GameObject)Resources.Load("Prefab/UI/SpawnStone", typeof(GameObject));

        circumference = offset * 2 * Mathf.PI/4;
        elementDistance = (circumference + lineDistance) / (maxVisableStones -1);

        for (int i = 0; i < maxVisableStones; i++)
        {
            monsterStoneUI[i] = Instantiate(baseStone);
            monsterStoneUI[i].transform.parent = Test;
            monsterStoneUI[i].transform.parent = centerPoint;
            monsterStoneUI[i].transform.localScale = Vector3.one;


            float distence = elementDistance * i;
            if(distence <= lineDistance)
            {
                monsterStoneUI[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(distence, offset, 0);
                monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else
            {
                float cirkelProcent = (distence - lineDistance) / circumference;
                float radians = (270 + 90f * cirkelProcent) * (Mathf.PI / 180);
                monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + lineDistance + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
                monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0,90*cirkelProcent);
            }

            spawnTimers[i] = Instantiate(monsterStoneUI[i].transform.Find("SpawnTimer").GetComponent<Image>().material);
            monsterStoneUI[i].transform.Find("SpawnTimer").GetComponent<Image>().material = spawnTimers[i];
            int temp = i;
            monsterStoneUI[i].transform.Find("BG").GetComponent<Button>().onClick.AddListener(() => { Spawn.instance.quickCallWave(temp); });

            if (i < Spawn.instance.spawnQueue.Count)
                updateMonsterStoneInfo(i);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Spawn.instance.spawnQueue.Count > 0)
            spawnTimers[0].SetFloat("_Health", (Spawn.instance.spawnQueue[0].maxTime - Spawn.instance.WaveTimer) / maxTimeValue);
    }

    public void newWaveCall(MonsterStone monsterStone)
    {
        for (int i = 0; i < maxVisableStones; i++)
        {
            if (i < Spawn.instance.spawnQueue.Count)
                updateMonsterStoneInfo(i);
            else
                monsterStoneUI[i].active = false;
        }
        StartCoroutine(MoveUI());
        StartCoroutine(DropUI(monsterStone));
    }



    private void updateMonsterStoneInfo(int index)
    {
        Image monsterType = monsterStoneUI[index].transform.Find("Type").GetComponent<Image>();
        monsterType.sprite = Spawn.instance.spawnQueue[index].getTypeImage();
        Transform abilityParentPointer = monsterStoneUI[index].transform.Find("Abilities");
        
        for (int i = 0; i < abilityParentPointer.childCount; i++)
        {
            if (i < Spawn.instance.spawnQueue[index].abilitys.Count)
            {
                abilityParentPointer.GetChild(i).gameObject.active = true;
                Image monsterAbility = abilityParentPointer.GetChild(i).GetComponent<Image>();
                monsterAbility.material = Spawn.instance.spawnQueue[index].getAblityImage(i);
            }
            else
            {
                abilityParentPointer.GetChild(i).gameObject.active = false;
            }
        }
        spawnTimers[index].SetFloat("_Health", (Spawn.instance.spawnQueue[index].maxTime - Spawn.instance.WaveTimer) / maxTimeValue);
    }

   IEnumerator DelayBetweenWaves()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime*Spawn.instance.waveSpeed;
            yield return new WaitForEndOfFrame();
            if (dealyBetweenWaves < timer)
                break;
            
        }
        
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
                    monsterStoneUI[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(centerPoint.anchoredPosition.x + distence, centerPoint.anchoredPosition.y + -offset, 0);
                    monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    float cirkelProcent = (distence - lineDistance) / circumference;
                    float radians = (270 + 90f * cirkelProcent) * (Mathf.PI / 180);
                    monsterStoneUI[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(centerPoint.anchoredPosition.x + lineDistance + offset * Mathf.Cos(radians), centerPoint.anchoredPosition.y + offset * Mathf.Sin(radians), 0);
                    monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, 90f * cirkelProcent);
                }
            }
            if (timer >= 1)
                break;
            

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DropUI(MonsterStone monsterStone)
    {
        float timer = 0;
        RectTransform UIPointer;
        if (passiveDropElements.Count > 0)
        {
            UIPointer = passiveDropElements.Dequeue();
            UIPointer.gameObject.active = true;
        }
        else
        {
            UIPointer = Instantiate(baseStone).GetComponent<RectTransform>();
            UIPointer.parent = centerPoint;
            UIPointer.localScale = Vector3.one;
        }

        UIPointer.anchoredPosition = new Vector3(centerPoint.anchoredPosition.x, centerPoint.anchoredPosition.y - offset, 0);
        Transform abilityParentPointer = UIPointer.transform.Find("Abilities");
        for (int i = 0; i < abilityParentPointer.childCount; i++)
        {
            if (i < monsterStone.abilitys.Count)
            {
                abilityParentPointer.GetChild(i).gameObject.active = true;
                Image monsterAbility = abilityParentPointer.GetChild(i).GetComponent<Image>();
                monsterAbility.material = monsterStone.getAblityImage(i);
            }
            else
            {
                abilityParentPointer.GetChild(i).gameObject.active = false;
            }
        }

        Vector2 startPosition = new Vector3(UIPointer.anchoredPosition.x, UIPointer.anchoredPosition.y);

        while (true)
        {
            timer += Time.fixedDeltaTime / getDropTime;

            Vector3 newPosition = new Vector3(startPosition.x, startPosition.y - (canvas.rect.height + startPosition.y) * timer, 0);
            UIPointer.anchoredPosition = newPosition;
            if (timer >= 1)
            {
                UIPointer.gameObject.active = false;
                passiveDropElements.Enqueue(UIPointer);
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        UnityEditor.Handles.color = Color.black;

        circumference = offset * 2 * Mathf.PI / 4;
        elementDistance = (circumference + lineDistance) / (maxVisableStones - 1);

        Gizmos.DrawLine(new Vector3(centerPoint.position.x, centerPoint.position.y - offset, 0), new Vector3(centerPoint.position.x + lineDistance, centerPoint.position.y - offset, 0));
        UnityEditor.Handles.DrawWireArc(centerPoint.position+new Vector3(lineDistance,0,0), Vector3.back, Vector3.up , 180, offset);
        for(int i = 0; i < maxVisableStones; i++)
        {
            float distence = elementDistance * i;
            if (distence <= lineDistance)
            {
                Vector3 newPosition = new Vector3(centerPoint.position.x + distence, centerPoint.position.y - offset, 0);
                Gizmos.DrawLine(newPosition+Vector3.up*20, newPosition + Vector3.up * -20);
            }
            else
            {
                float cirkelProcent = (distence - lineDistance) / circumference;
                float radians = (270 + 90f * cirkelProcent) * (Mathf.PI / 180);
                Vector3 newPosition1 = new Vector3(centerPoint.position.x + lineDistance + (offset - 20) * Mathf.Cos(radians), centerPoint.position.y + (offset - 20) * Mathf.Sin(radians), 0);
                Vector3 newPosition2 = new Vector3(centerPoint.position.x + lineDistance + (offset + 20) * Mathf.Cos(radians), centerPoint.position.y + (offset + 20) * Mathf.Sin(radians), 0);
                Gizmos.DrawLine(newPosition1, newPosition2);
            }
        }
    }
}
