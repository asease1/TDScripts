using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitNodeToNode : MonoBehaviour {

    public float speed = 5, rotateSpeed, slowAmount = 1;
    public Transform target;
    private Node CurrentNode;

    public float Speed { get
        {
            return speed * slowAmount;
        }
    }

    void Start()
    {
        CurrentNode = Grid.instance.NodeFromWorldPoint(transform.position);
        StartCoroutine("FollowPath");
        GetComponent<EnemyStats>().NewSlowEvent.AddListener(SlowAmount);
    }

    void SlowAmount(float slowAmount)
    {
        this.slowAmount = slowAmount;
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            if (transform.position == CurrentNode.worldPosition)
            {
                CurrentNode = CurrentNode.child;
                if (CurrentNode == null)
                    break;
                StopCoroutine("RotateTowards");
                StartCoroutine("RotateTowards", rotateSpeed);
            }

            transform.position = Vector3.MoveTowards(transform.position, CurrentNode.worldPosition, Speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotateTowards(float rotationSpeed)
    {
        Vector3 targetDir = (CurrentNode.worldPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDir);
        while (true)
        {
            if (lookRotation == transform.rotation)
                break;
            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotateSpeed);

            yield return null;
        }
    }
}
