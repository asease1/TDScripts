using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPoints : MonoBehaviour {

    public Transform Target;
    private Vector3[] currentPath;
    public GameObject[] spawnEnemys;
    private List<Unit> activeEnemys = new List<Unit>();
    public Dictionary< string, Unit> disableEnemys = new Dictionary<string, Unit>();
    public float spawnTimer;

	// Use this for initialization
	void Start () {
        PathRequestManager.RequestPath(transform.position, Target.position, PathFound);
        StartCoroutine("Spawn");
	}

    public bool UpdatePath(List<Vector3> tempNodes)
    {

        if (!CheckInterSect(tempNodes))
        {
            return true;
        }

        Vector3[] wayPoints;
        bool temp = PathRequestManager.Instance.pathFinding.GetNewPath(transform.position, Target.position, out wayPoints);

        if (temp && wayPoints != currentPath)
        {
            UpdateUnits(wayPoints, tempNodes.ToArray());
            currentPath = wayPoints;
        }
        
        return temp;
    }

    public void PathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            currentPath = newPath;
        }
    }

    private void UpdateUnits( Vector3[] newWayPoints, Vector3[] interSect)
    {
        int newIndex = 0;
        for (int i = 0; i < currentPath.Length; i++)
        {
            if (i >= newWayPoints.Length)
                break;
            if (currentPath[i] != newWayPoints[i])
            {
                newIndex = i;
                break;
            }
        }
        foreach(Unit elm in activeEnemys)
        {
            elm.UpdateUnitPath(newIndex, newWayPoints, interSect);
        }

    }

    private bool CheckInterSect(List<Vector3> checkNodes)
    {
        for(int i = 1; i < currentPath.Length; i++)
        {
            for (int j = 0; j < checkNodes.Count; j++)
            {
                if (Vector3.Distance(currentPath[i-1], checkNodes[j]) + Vector3.Distance(currentPath[i], checkNodes[j]) == Vector3.Distance(currentPath[i-1], currentPath[i]))
                    return true;
            }
        }
        return false;
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            if (currentPath == null)
                yield return null;
            else
                break;
        }
        while (true)
        {
            Unit temp = null;
            if (disableEnemys.TryGetValue(spawnEnemys[0].name, out temp))
            {

            }
            else {
                activeEnemys.Add( (Instantiate(spawnEnemys[0], transform.position, Quaternion.identity) as GameObject).GetComponent<Unit>());
                activeEnemys[activeEnemys.Count - 1].GetInstantiate(currentPath);
            }
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}
