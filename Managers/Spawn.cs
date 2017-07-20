using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawn : MonoBehaviour {


    //To spawn the first wave istent
    private float waveTimer = 500;
    public float waveSpeed = 1;
    private float spawnTimer = 0;
    private float lastSpawn = 0;
    [Range(0, 2)]
    public float spawnDelay;

    public Transform[] spawnPoints;
    public Transform targetPoint;

    public UnityEvent newWave;

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

    public float WaveTimer { get { return waveTimer; } }
   
    private void Awake()
    {
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Harpy, 30, new List<MonsterStone.MonsterAbility>()));
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Slime, 25, new List<MonsterStone.MonsterAbility>()));
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Harpy, 20, new List<MonsterStone.MonsterAbility>()));
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Slime, 15, new List<MonsterStone.MonsterAbility>()));
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Harpy, 10, new List<MonsterStone.MonsterAbility>()));
        spawnQueue.Add(new MonsterStone(MonsterStone.MonsterType.Slime, 5, new List<MonsterStone.MonsterAbility>()));

        if (newWave == null)
            newWave = new UnityEvent();

        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        foreach (Transform elm in spawnPoints)
            PathRequestManager.Instance.pathFinding.CreatePath(elm.position, targetPoint.position);
    }

    private void Update()
    {
        waveTimer += Time.deltaTime * waveSpeed;
        spawnTimer += Time.deltaTime;

        if(spawnQueue.Count > 0 && spawnQueue[0].maxTime < waveTimer)
        {
            callQueue.Add(spawnQueue[0]);
            spawnQueue.RemoveAt(0);
            waveTimer = 0;
            newWave.Invoke();
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
}
