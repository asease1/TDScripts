using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

    public float speed = 5;
    Vector3[] path;
    int targetIndex;
    
    public void GetInstantiate(Vector3[] newPath)
    {
        path = newPath;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine("FollowPath");
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];

        while (true)
        {
            if(transform.position == new Vector3( currentWayPoint.x, transform.position.y, currentWayPoint.z))
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
                transform.LookAt(new Vector3(currentWayPoint.x, transform.position.y, currentWayPoint.z));
            }
            transform.position = Vector3.MoveTowards(transform.position,new Vector3(currentWayPoint.x, transform.position.y, currentWayPoint.z), speed*Time.deltaTime);            
            yield return null;
        }
    }

    public void UpdateUnitPath(int newTargetIndex, Vector3[] newPath, Vector3[] interSect)
    {
        if (targetIndex < newTargetIndex)
            path = newPath;
        else if(targetIndex > newTargetIndex)
        {
            return;
        }
        else
        {
            if(MyMath.InterSectLine(transform.position, path[newTargetIndex], interSect))
            {
                PathRequestManager.Instance.pathFinding.GetNewPath(transform.position, path[path.Length - 1], out path);
                targetIndex = 0;
            }
        }
        StopCoroutine("FollowPath");
        StartCoroutine("FollowPath");
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i]+ Vector3.up *5, Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position + Vector3.up * 5, path[i] + Vector3.up * 5);
                }
                else
                    Gizmos.DrawLine(path[i - 1] + Vector3.up * 5, path[i] + Vector3.up * 5);
            }
        }
    }
}
