using UnityEngine;
using System.Collections;

public class TestUpdateGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Node temp = Grid.instance.NodeFromWorldPoint(transform.position);
        temp.walkAble = true;
        PathRequestManager.Instance.pathFinding.Reverse(Grid.instance.NodeFromWorldPoint(transform.position));
	}	
}
