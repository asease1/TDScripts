using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[SerializeField]
public class  MonsterEvent : UnityEvent<MonsterStone> { }

public class Spawn : MonoBehaviour {

    public UnityEngine.Object spawnFile;
    //To spawn the first wave istent
    private int waveCount = 0;
    private float waveTimer = 500;
    public float waveSpeed = 1;
    private float spawnTimer = 0;
    private float lastSpawn = 0;
    [Range(0, 2)]
    public float spawnDelay;
    private int targetWave = -1;

    public Transform[] spawnPoints;
    public Transform targetPoint;

    public MonsterEvent newWave;

    public List<MonsterStone> spawnQueue = new List<MonsterStone>();
    public List<MonsterStone> callQueue = new List<MonsterStone>();

    public static Spawn instance;
    
    public int monsterQueueCount {
        get
        {
            int count = 0;
            foreach(MonsterStone elm in callQueue)
            {
                count += elm.monsterCount;
            }
            return count - spawnCount;
        }
    }

    public int WaveCount { get { return waveCount; } }

    public float WaveTimer { get { return waveTimer; } }
   
    private void Awake()
    {
        

        if (newWave == null)
            newWave = new MonsterEvent();

        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        spawnQueue = XmlConverter.convertXmlToMonsterStone(AssetDatabase.GetAssetPath(spawnFile));
        foreach (Transform elm in spawnPoints)
            PathRequestManager.Instance.pathFinding.CreatePath(elm.position, targetPoint.position);

        //List<MonsterStone> resualt = XmlConverter.convertXmlToMonsterStone(AssetDatabase.GetAssetPath(spawnFile));      
    }

    private void Update()
    {
        waveTimer += Time.deltaTime * waveSpeed;
        spawnTimer += Time.deltaTime;

        if(spawnQueue.Count > 0 && spawnQueue[0].maxTime < waveTimer)
        {
            callQueue.Add(spawnQueue[0]);
            MonsterStone temp = spawnQueue[0];
            spawnQueue.RemoveAt(0);
            waveTimer = 0;
            waveCount++;
            newWave.Invoke(temp);
            if (targetWave == 0)
                waveSpeed = 1;
                
            

            targetWave--;
        }

        if(spawnTimer > lastSpawn + spawnDelay)
        {
            lastSpawn = spawnTimer;
            if(callQueue.Count > 0)
            {
                SpawnMonster();
            }
        }
    }


    private int spawnCount = 0;
    private void SpawnMonster()
    {
        foreach(Transform elm in spawnPoints)
        {
            callQueue[0].instatiateMonster(elm.position);
            spawnCount++;
            if (spawnCount >= callQueue[0].monsterCount)
            {
                spawnCount = 0;
                callQueue.RemoveAt(0);
                return;
            }
                
        }
    }

    internal void quickCallWave(int temp)
    {
        targetWave = temp;
        waveSpeed = 10;
    }
}
