using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStone {
    public enum MonsterType { Standard, Fast, Tough, Phasing, Gatherer, Hovering, Slime, Harpy}
    public enum MonsterAbility { Shield, MaxHP, Haste, Split, Demolition, HpReg, Armor, HealOnDeath, SlowAura}
    public MonsterType type;
    public int buffCount;
    public List<MonsterAbility> abilitys;
    public float maxTime;

    private static Material[] abiltityMaterials;
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
        if (abiltityMaterials == null)
        {
            abiltityMaterials = new Material[Enum.GetNames(typeof(MonsterAbility)).Length];
            for (int i = 0; i < Enum.GetNames(typeof(MonsterAbility)).Length; i++)
            {
                switch ((MonsterAbility)i)
                {
                    case MonsterAbility.Armor:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_armor", typeof(Material));
                        break;
                    case MonsterAbility.HealOnDeath:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_lifelink", typeof(Material));
                        break;
                    case MonsterAbility.HpReg:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_hp_reg", typeof(Material));
                        break;
                    case MonsterAbility.MaxHP:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_hp_max", typeof(Material));
                        break;
                    case MonsterAbility.Shield:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_shield", typeof(Material));
                        break;
                    case MonsterAbility.SlowAura:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_armor", typeof(Material));
                        break;
                    case MonsterAbility.Haste:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_haste", typeof(Material));
                        break;
                    case MonsterAbility.Split:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_split", typeof(Material));
                        break;
                    case MonsterAbility.Demolition:
                        abiltityMaterials[i] = (Material)Resources.Load("Materials/UI/monsterstone/ability_demolition", typeof(Material));
                        break;
                    default:
                        Debug.Log("Unknown Monster type matrial load");
                        break;
                }
            }
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

    internal static MonsterType ParseStringToType(string value)
    {
        switch (value.ToLower())
        {
            case "standard":
                return MonsterType.Standard;
            case "fast":
                return MonsterType.Fast;
            case "though":
                return MonsterType.Tough;
            case "phasing":
                return MonsterType.Phasing;
            case "gatherer":
                return MonsterType.Gatherer;
            case "hovering":
                return MonsterType.Hovering;
            default:
                Debug.Log("Unknown monster type");
                return MonsterType.Slime;
        }
    }

    internal static MonsterAbility getAbility(string value)
    {
        switch (value.ToLower())
        {
            case "shield":
                return MonsterAbility.Shield;
            case "speed":
                return MonsterAbility.Haste;
            case "armor":
                return MonsterAbility.Armor;
            case "hpreg":
                return MonsterAbility.HpReg;
            case "healondeath":
                return MonsterAbility.HealOnDeath;
            case "slowaura":
                return MonsterAbility.SlowAura;
            case "split":
                return MonsterAbility.Split;
            case "suiciders":
                return MonsterAbility.Demolition;
            case "maxhp":
                return MonsterAbility.MaxHP;
            default:
                Debug.Log("Unknown ability");
                return MonsterAbility.MaxHP;
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
    internal Material getAblityImage(int i)
    {
        return abiltityMaterials[i];
    }
}
