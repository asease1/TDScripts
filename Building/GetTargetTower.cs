using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#pragma warning disable
public class GetTargetTower : MonoBehaviour {

    public enum TargetType { Closed, Fadest, HighstHealth, LowestHealth}

    private CapsuleCollider collider;
    private List<EnemyStats> targets = new List<EnemyStats>();
    private TowerShot towerShot;

    public EnemyStats GetTarget(TargetType type)
    {
        if (targets.Count > 0)
        {
            return targets[0];
        }  
        
        return null;
    }
    // Use this for initialization
    void Awake() {
        collider = gameObject.AddComponent<CapsuleCollider>();
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        collider.isTrigger = true;

    }

    public void KillTarget(EnemyStats target)
    {
        targets.Remove(target);
    }

    public void UpdateRange(UpgradeStats stat)
    {
        collider.height = stat.range * 3;
        collider.radius = stat.range * 2;
    }

    public void UpdateRange(float stat)
    {
        collider.height = stat * 3;
        collider.radius = stat * 2;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy");
            EnemyStats temp = hit.gameObject.GetComponent<EnemyStats>();
            temp.towerTargets.Add(towerShot);
            targets.Add(temp);
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            EnemyStats temp = hit.gameObject.GetComponent<EnemyStats>();
            targets.Remove(temp);
            temp.towerTargets.Remove(towerShot);
        }
    }
}
