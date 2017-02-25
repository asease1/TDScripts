using UnityEngine;
using System.Collections;

public class EnemyTest : MonoBehaviour {

    private Node currentNode;
    public Grid grid;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        Node temp = grid.NodeFromWorldPoint(transform.position);
        if(temp != currentNode)
        {
            if(temp != null)
            {
                temp.buildAble = true;
                if (currentNode != null)
                    currentNode.buildAble = false;
                currentNode = temp;
            }
        }
	}
}
