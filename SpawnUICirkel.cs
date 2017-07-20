using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnUICirkel : MonoBehaviour {

    public Transform centerPoint;
    public float offset;
    public int maxVisableStones = 2;
    private float maxHealthValue = 30;
    private GameObject canvas;
    private GameObject[] monsterStoneUI;
    private  GameObject baseStone;
    private Material[] spawnTimers;

    // Use this for initialization
    void Start () {
        spawnTimers = new Material[maxVisableStones];
        monsterStoneUI = new GameObject[maxVisableStones];
        canvas = GameObject.FindGameObjectWithTag("ScreenCanvas");
	    baseStone = (GameObject)Resources.Load("UI/SpawnStone", typeof(GameObject));

        for (int i = 0; i < Spawn.instance.spawnQueue.Count && i < maxVisableStones; i++)
        {
            monsterStoneUI[i] = Instantiate(baseStone);
            monsterStoneUI[i].transform.parent = canvas.transform;
            monsterStoneUI[i].transform.localScale = Vector3.one;
            float radians = (270 + (90f / (maxVisableStones - 1)) * i) * (Mathf.PI / 180);
            monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
            monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, (90f / (maxVisableStones - 1)) * i);

            spawnTimers[i] = Instantiate(monsterStoneUI[i].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("SpawnTimerBG").Find("SpawnTimer").GetComponent<Image>().material);
            monsterStoneUI[i].transform.Find("StoneList").Find("[Horizontal]StoneListGrid").Find("SpawnTimerBG").Find("SpawnTimer").GetComponent<Image>().material = spawnTimers[i];
            spawnTimers[i].SetFloat("_Health", Spawn.instance.spawnQueue[i].maxTime / maxHealthValue);
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(Spawn.instance.spawnQueue.Count > 0)
            spawnTimers[0].SetFloat("_Health", (Spawn.instance.spawnQueue[0].maxTime - Spawn.instance.waveTimer) / maxHealthValue);
        
	}

    public void newWaveCall()
    {
        Debug.Log("test");
        for (int i = Spawn.instance.spawnQueue.Count; i < maxVisableStones; i++)
        {
            monsterStoneUI[i].active = false;
        }
        StartCoroutine(RotateCirkel());
    }


    IEnumerator RotateCirkel()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.fixedDeltaTime;
            for(int i = 0; i < Spawn.instance.spawnQueue.Count && i < maxVisableStones; i++)
            {
                float radians = (270 + (90f / (maxVisableStones - 1)) * (i+1-timer)) * (Mathf.PI / 180);
                monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
                monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, (90f / (maxVisableStones - 1)) * (i + 1 - timer));
            }
            if (timer >= 1)
            {
                for (int i = 0;  i < maxVisableStones; i++)
                {
                    float radians = (270 + (90f / (maxVisableStones - 1)) * i) * (Mathf.PI / 180);
                    monsterStoneUI[i].transform.position = new Vector3(centerPoint.position.x + offset * Mathf.Cos(radians), centerPoint.position.y + offset * Mathf.Sin(radians), 0);
                    monsterStoneUI[i].transform.eulerAngles = new Vector3(0, 0, (90f / (maxVisableStones - 1)) * i );
                }
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    }


}
