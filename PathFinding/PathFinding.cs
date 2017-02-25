using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class PathFinding : MonoBehaviour {

    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public bool GetNewPath(Vector3 startPos, Vector3 targetpos, out Vector3[] newPath)
    {
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetpos);

        if (!startNode.walkAble && !targetNode.walkAble)
        {
            MyHeap<Node> openSet = new MyHeap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //Success to find a path from A to B
                    pathSuccess = true;
                    break;
                }

                foreach (Node elm in grid.GetNeighbours(currentNode))
                {
                    //Cant walk on this node because of either bool or closedSet
                    if (elm.walkAble || closedSet.Contains(elm))
                        continue;

                    int newMovementCost = currentNode.gCost + GetDistance(currentNode, elm);
                    if (newMovementCost < elm.gCost || !openSet.Contains(elm))
                    {
                        elm.gCost = newMovementCost;
                        elm.hCost = GetDistance(elm, targetNode);
                        elm.parrent = currentNode;

                        if (!openSet.Contains(elm))
                            openSet.Add(elm);
                        else
                            openSet.UpdateItem(elm);
                    }
                }
            }
        }
        if (pathSuccess)
            newPath = RetracePath(startNode, targetNode);
        else
            newPath = null;
        return pathSuccess;
    }

    public bool CreatePath(Vector3 startPos, Vector3 targetpos)
    {
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetpos);

        if (!startNode.walkAble && !targetNode.walkAble)
        {
            MyHeap<Node> openSet = new MyHeap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //Success to find a path from A to B
                    pathSuccess = true;
                    break;
                }

                foreach (Node elm in grid.GetNeighbours(currentNode))
                {
                    //Cant walk on this node because of either bool or closedSet
                    if (elm.walkAble || closedSet.Contains(elm))
                        continue;

                    int newMovementCost = currentNode.gCost + GetDistance(currentNode, elm);
                    if (newMovementCost < elm.gCost || !openSet.Contains(elm))
                    {
                        elm.gCost = newMovementCost;
                        elm.hCost = GetDistance(elm, targetNode);
                        elm.parrent = currentNode;

                        if (!openSet.Contains(elm))
                            openSet.Add(elm);
                        else
                            openSet.UpdateItem(elm);
                    }
                }
            }
        }
        if (pathSuccess)
            UpdateNodes(startNode, targetNode);

        return pathSuccess;
    }

    public bool UpdateGrid(Node startNode, Node targetNode)
    {
        bool pathSuccess = false;

        if (!startNode.walkAble && !targetNode.walkAble)
        {
            MyHeap<Node> openSet = new MyHeap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode || currentNode.end)
                {
                    //Success to find a path from A to B
                    targetNode = currentNode;
                    pathSuccess = true;
                    break;
                }

                foreach (Node elm in grid.GetNeighbours(currentNode))
                {
                    //Cant walk on this node because of either bool or closedSet
                    if (elm.walkAble || closedSet.Contains(elm))
                        continue;

                    int newMovementCost = currentNode.gCost + GetDistance(currentNode, elm);
                    if (newMovementCost < elm.gCost || !openSet.Contains(elm))
                    {
                        elm.gCost = newMovementCost;
                        elm.hCost = GetDistance(elm, targetNode);
                        elm.parrent = currentNode;

                        if (!openSet.Contains(elm))
                            openSet.Add(elm);
                        else
                            openSet.UpdateItem(elm);
                    }
                }
            }
        }
        if (pathSuccess)
            UpdateNodes(startNode ,targetNode);

        return pathSuccess;
    }

    public bool Reverse(Node reverseNode)
    {
        Node targetNode = GetTargetNode(reverseNode);
        List<Node> correctNodes = GetAllParrents(reverseNode);
        for(int i = 0; i < correctNodes.Count; i++)
        {          
            correctNodes[i].end = false;
            correctNodes[i].parrent = null;
            if (correctNodes[i].walkAble)
            {
                correctNodes[i].child = null;
                correctNodes[i].parrents.Clear();
                correctNodes.RemoveAt(i);
                i--;
            }
        }
        for (int i = 0; i < correctNodes.Count-1; i++)
        {
            if (!correctNodes[correctNodes.Count - 1].end && !CreatePath(correctNodes[correctNodes.Count - 1].worldPosition, targetNode.worldPosition))
                return false;

            if (!correctNodes[i].end && !UpdateGrid(correctNodes[i], targetNode))
                return false;
        }
        return true;
    }

    private Node GetTargetNode(Node currentNode)
    {
        while(true)
        {
            if (currentNode.child != null)
                currentNode = currentNode.child;
            else
                break;
        }
        return currentNode;
    }

    private IEnumerator FindPath(Vector3 startPos, Vector3 targetpos)
    {

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetpos);

        if (!startNode.walkAble && !targetNode.walkAble)
        {
            MyHeap<Node> openSet = new MyHeap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //Success to find a path from A to B
                    pathSuccess = true;
                    break;
                }

                foreach (Node elm in grid.GetNeighbours(currentNode))
                {
                    //Cant walk on this node because of either bool or closedSet
                    if (elm.walkAble || closedSet.Contains(elm))
                        continue;

                    int newMovementCost = currentNode.gCost + GetDistance(currentNode, elm);
                    if (newMovementCost < elm.gCost || !openSet.Contains(elm))
                    {
                        elm.gCost = newMovementCost;
                        elm.hCost = GetDistance(elm, targetNode);
                        elm.parrent = currentNode;

                        if (!openSet.Contains(elm))
                            openSet.Add(elm);
                        else
                            openSet.UpdateItem(elm);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
           wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        path.Add(endNode);
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parrent;
        }
        Vector3[] wayPoints = SimplifyPath(path);
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    /// <summary>
    /// Set the Child Parrent relesions
    /// </summary>
    void UpdateNodes(Node startNode,Node endNode)
    {
        Node currentNode = endNode;
        while (true)
        {
            currentNode.end = true;
            
            currentNode.parrents.Add(currentNode.parrent);
            currentNode.parrents = currentNode.parrents.Distinct<Node>().ToList<Node>();

            for(int i = 0; i < currentNode.parrents.Count; i++)
            {
                if(currentNode.parrents[i].walkAble)
                {
                    currentNode.parrents[i].child = null;
                    currentNode.parrents[i].parrents.Clear();
                    currentNode.parrents.RemoveAt(i);
                    i--;
                    continue;
                }
                currentNode.parrents[i].child = currentNode;
            }
            currentNode = currentNode.parrent;
            currentNode.child.parrent = null;

            if (currentNode == startNode)
                break;
        }
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                wayPoints.Add(path[i-1].worldPosition);
            }
            directionOld = directionNew;
        }
        wayPoints.Add(path[path.Count - 1].worldPosition);
        return wayPoints.ToArray();
    }

    private int GetDistance(Node A, Node B)
    {
        int dstX = Mathf.Abs(A.gridX - B.gridX);
        int dstY = Mathf.Abs(A.gridY - B.gridY);
        if(dstX > dstY)
            return 14*dstY+10*(dstX-dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }

    private static List<Node> GetAllParrents(Node startNode)
    {
        List<Node> Resualt = new List<Node>();
        Resualt.Add(startNode);
        if (startNode.parrents.Count <= 0)
            return Resualt;
        

        for(int i = 0; i < Resualt.Count; i++) { 
            Resualt.AddRange(Resualt[i].parrents);
            if (i > 50)
                break;
        }
        return Resualt;
    }
}
