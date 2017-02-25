using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class MyFloatEvent : UnityEngine.Events.UnityEvent<float> { }

public class EnemyStats : MonoBehaviour {

    public enum ArmorTypes { light, medium, heavy}
    public List<TowerShot> towerTargets = new List<TowerShot>();
    public ArmorTypes myArmorType;
    public GameObject healthBar;
    public MyFloatEvent NewSlowEvent;
    private Renderer healthBarRend;
    private float Health = 50, HealthMax = 50, armor = 0;
    private float hpRegen = 1;
    private const float highDamage = 1.25f, normalDamage = 1f, lowDamage = 0.75f;
    private const float specialHigh = 1.50f, specialNormal = 1f;
    private float curseAmount = 1, slowAmount = 1, dotAmount;
    public float health
    {
        get { return Health; }
        set{
            Health = Mathf.Clamp(value, -1f, HealthMax);
            healthBarRend.material.SetFloat("_Health", Health / HealthMax);
            if (Health <= 0)
            {
                DestroyThisEnemy();
            }
        }
    }
    private float HPReg {
        set
        {
            hpRegen = Mathf.Max(0, hpRegen + value);
        }
        get { return hpRegen; }
    }
    private float Armor
    {
        set
        {
            armor = Mathf.Max(0, armor + value);
        }
        get { return armor; }
    }

    void Awake()
    {
        if (NewSlowEvent == null) { NewSlowEvent = new MyFloatEvent(); }
    }

    void Start()
    {
        healthBarRend = healthBar.GetComponent<Renderer>();
        healthBarRend.material.SetFloat("_Health", Health / HealthMax);
        StartCoroutine("HpRegen");
        
    }

    void Update()
    {
        healthBar.transform.LookAt(Camera.main.transform);        
    }

    private void DestroyThisEnemy()
    {   
        for(int i = 0; i < towerTargets.Count; i++) {
            towerTargets[i].KillTarget(this);
        }
        Destroy(gameObject);
    }

    public void Attacked(UpgradeStats stats, BaseBullet.damageTypes damageType)
    {
        TakeDamage(stats.damage, damageType);
        EffectCheck(stats);
        Armor += stats.armorReduce;
        HPReg += stats.hpRegReduce;
    }

    private void EffectCheck(UpgradeStats stats)
    {
        if(stats.duration > 0)
        {
            if (stats.slow > 0 && stats.slow < slowAmount)
            {
                StopCoroutine("SlowTimer");
                StartCoroutine(SlowTimer(stats.duration, stats.slow));
            }
            if (stats.DOT > 0 && stats.DOT > dotAmount)
            {
                StopCoroutine("DamageOverTime");
                StartCoroutine(DamageOverTime(stats.duration, stats.DOT));
            }
            if (stats.curse > 0 && stats.curse > curseAmount)
            {
                StopCoroutine("CurseDuration");
                StartCoroutine(CurseDuration(stats.duration, stats.DOT));
            }
        }
    }

    private void TakeDamage(float damage, BaseBullet.damageTypes damageType)
    {
        float calcDamage = 0;
        damage *= curseAmount;
        switch (damageType)
        {
            case BaseBullet.damageTypes.heavy:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                        calcDamage = damage * highDamage - Armor;
                        break;
                    case ArmorTypes.medium:
                        calcDamage = damage * lowDamage - Armor;
                        break;
                    case ArmorTypes.heavy:
                        calcDamage = damage * normalDamage - Armor;
                        break;
                }
                break;
            case BaseBullet.damageTypes.light:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                        calcDamage = damage * normalDamage - Armor;
                        break;
                    case ArmorTypes.medium:
                        calcDamage = damage * highDamage - Armor;
                        break;
                    case ArmorTypes.heavy:
                        calcDamage = damage * lowDamage - Armor;
                        break;
                }
                break;
            case BaseBullet.damageTypes.medium:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                        calcDamage = damage * lowDamage - Armor;
                        break;
                    case ArmorTypes.medium:
                        calcDamage = damage * normalDamage - Armor;
                        break;
                    case ArmorTypes.heavy:
                        calcDamage = damage * highDamage - Armor;
                        break;
                }
                break;
            case BaseBullet.damageTypes.special1:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                    case ArmorTypes.medium:
                    case ArmorTypes.heavy:
                        break;
                }
                break;
            case BaseBullet.damageTypes.special2:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                    case ArmorTypes.medium:
                    case ArmorTypes.heavy:
                        break;
                }
                break;
            case BaseBullet.damageTypes.special3:
                switch (myArmorType)
                {
                    case ArmorTypes.light:
                    case ArmorTypes.medium:
                    case ArmorTypes.heavy:
                        break;
                }
                break;
        }
        calcDamage = Mathf.Max(calcDamage, 0);
        health -= calcDamage;
    }

    IEnumerator HpRegen()
    {
        health += hpRegen;
        if (hpRegen != 0)
            yield return new WaitForSeconds(1);
    }

    IEnumerator SlowTimer(float timer, float slowAmount)
    {
        NewSlowEvent.Invoke(slowAmount);
        yield return new WaitForSeconds(timer);
        NewSlowEvent.Invoke(1);
        this.slowAmount = 1;
    }
    IEnumerator DamageOverTime(float timer, float damage)
    {
        int count = 0;
        while (count < timer)
        {
            count++;
            health -= damage;
            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator CurseDuration(float timer, float curse)
    {
        curseAmount = curse;
        yield return new WaitForSeconds(timer);
        curseAmount = 1;
    }
}
