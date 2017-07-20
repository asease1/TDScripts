using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStone {
    public enum MonsterType { Slime, Harpy}
    public enum MonsterAbility { Shield, MaxHP}
    public MonsterType type;
    public int buffCount;
    public List<MonsterAbility> abilitys;
    public float maxTime;

    private static List<GameObject> enemies;

    public MonsterStone(MonsterType type, float maxTime, List<MonsterAbility> abilitys)
    {
        this.type = type;
        buffCount = 1;
        this.abilitys = abilitys;
        this.maxTime = maxTime;

        if (enemies == null)
        {
            enemies = new List<GameObject>();
            enemies.Add((GameObject)Resources.Load("Prefab/Enemy", typeof(GameObject)));
        }

    }

    public int monsterCount {
        get
        {
            switch (type)
            {
                //TODO create math for wave buffcount and monsterType
                case MonsterType.Harpy:
                    return 5;
                case MonsterType.Slime:
                    return 10;
                default:
                    Debug.Log("Unknown Monster type found");
                    return 5;

            }    
        }
    }

    public void setMonsterStone(MonsterType type, float maxTime, List<MonsterAbility> abilitys)
    {
        this.type = type;
        buffCount = 1;
        this.abilitys = abilitys;
        this.maxTime = maxTime;
    }

    public void instatiateMonster(Vector3 position)
    {
        switch (type)
        {
            default:
                UnityEngine.Object.Instantiate(enemies[0], position, Quaternion.identity);
                break;

        }
    }

    //TODO implement
    internal Sprite getTypeImage()
    {

        return null;
    }

    //TODO implement
    internal Image getAblityImage(int i)
    {
        return null;
    }
}
