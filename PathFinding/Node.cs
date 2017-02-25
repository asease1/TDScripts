using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Node : IHeapItem<Node> {

    public bool walkAble, buildAble, end;
    public Vector3 worldPosition;
    public int gCost, hCost, gridX, gridY;
    public Node parrent, child;
    public List<Node> parrents;
    int heapIndex;
    public int fCost { get { return gCost + hCost; } }

    public Node(Vector3 worldPosition, bool walkAble, bool buildAble, int gridX, int gridY)
    {
        parrents = new List<Node>();
        this.worldPosition = worldPosition;
        this.walkAble = walkAble;
        this.buildAble = buildAble;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set { heapIndex = value; }
    }


    public int CompareTo(Node nodeToComapere)
    {
        int compare = fCost.CompareTo(nodeToComapere.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToComapere.hCost);
        }
        return -compare;
    }
}
