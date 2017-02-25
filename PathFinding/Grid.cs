using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Grid : MonoBehaviour {

    public bool DesplayGrid;
    public Vector2 gridSize;
    public float cubeSize;
    public float cubeHieght;
    public Vector3 StartPosition;
    //public int startOffsetX, startOffsetY, startOffSetZ;

    public LayerMask unWalkAble, unBuildAble;
    public Node[,] grid;
    public bool[,] orignalWalk, orignalBuild;

    private ProjecterGrid projecter;
    private int gridSizeX, gridSizeY;
    private static Grid gridClass;

    public int GridX
    {
        get { return gridSizeX; }
    }
    public int GridY
    {
        get { return gridSizeY; }
    }

    public int MaxSize
    {
        get { return gridSizeX * gridSizeY; }
    }
    public Vector3 CubeSize{
        get { return new Vector3(cubeSize, cubeHieght, cubeSize); }
    }
    public float startOffsetX { get { return (StartPosition.x); } }
    public float startOffsetY { get { return (StartPosition.y); } }
    public float startOffSetZ { get { return (StartPosition.z); } }

    public static Grid instance
    {
        get
        {
            return gridClass;
        }
    }

    void Awake()
    {
        gridClass = this;
        gridSizeX = Mathf.RoundToInt( gridSize.x / cubeSize);
        gridSizeY = Mathf.RoundToInt(gridSize.y / cubeSize);
        grid = CreatGrid();
        CreateOrignalbool();
        projecter = GetComponent<ProjecterGrid>();
    }

    void OnDrawGizmos()
    {
        if(grid != null && DesplayGrid)
        {
            foreach(Node elm in grid)
            {
                Gizmos.color = Color.green;
                Gizmos.color = (elm.walkAble) ? Color.red : Gizmos.color;
                Gizmos.color = (elm.buildAble) ? Color.blue : Gizmos.color;
                Gizmos.color = (elm.buildAble && elm.walkAble) ? Color.yellow : Gizmos.color;
                Gizmos.DrawWireCube(elm.worldPosition, CubeSize*0.95f);
                Gizmos.color = Color.black;
                /*if (elm.end && elm.child != null)
                {
                    Gizmos.DrawLine(elm.worldPosition, elm.child.worldPosition);
                    Gizmos.DrawSphere(elm.child.worldPosition, 0.5f);
                }/*
                Gizmos.color = Color.black;
                if (elm.parrents.Count > 0)
                {
                    foreach(Node elm1 in elm.parrents)
                    {
                        Gizmos.DrawLine(elm.worldPosition, elm1.worldPosition);
                        Gizmos.DrawSphere(elm1.worldPosition, 0.5f);
                    }
                }*/

            }
        }
        
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        if (grid == null)
            Debug.Log("Update grid to create it");
        int x = Mathf.FloorToInt((worldPosition.x - startOffsetX) / cubeSize);
        int y = Mathf.FloorToInt((worldPosition.z - startOffSetZ) / cubeSize);
        if (x < gridSizeX && y < gridSizeY && x >= 0 && y >= 0)
            return grid[x, y];
        else return null;
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition, Node[,] grid, Vector3 startPosition, float cubeSize, float gridSizeX, float gridSizeY)
    {
        int x = Mathf.FloorToInt((worldPosition.x - startOffsetX) / cubeSize);
        int y = Mathf.FloorToInt((worldPosition.z - startOffSetZ) / cubeSize);
        
        if (x < gridSizeX && y < gridSizeY && x >= 0 && y >= 0)
            return grid[x, y];
        else return null;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for(int x = -1; x <=1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    if(Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                    {
                        if (grid[checkX-x, checkY].walkAble || grid[checkX - x, checkY].buildAble || grid[checkX, checkY-y].walkAble || grid[checkX, checkY - y].buildAble)
                            continue;
                    }                                                              
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }
    public static List<Node> GetSqueareNodes(Vector3 position, int brushSize)
    {
        List<Node> brushNodes = new List<Node>();
        Vector3 newPosition = position + (new Vector3(-1,0,-1) * ((Mathf.Sin(Mathf.PI / 4) * Grid.instance.cubeSize / 2) * brushSize - 1));
        Node cornerNode = Grid.instance.NodeFromWorldPoint(newPosition);
        for(int x = 0; x < brushSize; x++)
        {
            for(int y = 0; y < brushSize; y++)
            {
                brushNodes.Add(Grid.instance.grid[x + cornerNode.gridX, y + cornerNode.gridY]);
            }
        }
        return brushNodes;
    }

    public Node[,] CreatGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 temp = new Vector3(startOffsetX, startOffsetY, startOffSetZ) + Vector3.right * (x * cubeSize + (cubeSize / 2)) + Vector3.forward * (y * cubeSize + (cubeSize / 2));
                float height = GetNodeHeight(temp);

                Vector3 worldPoint = temp+ Vector3.up*height;
                bool walkAble = (Physics.CheckBox(worldPoint, CubeSize / 2, Quaternion.identity, unWalkAble));
                bool buidAble = (Physics.CheckBox(worldPoint, CubeSize / 2, Quaternion.identity, unBuildAble));
                grid[x, y] = new Node(worldPoint, walkAble, buidAble, x,y);
            }
        }
        return grid;
    }

    public Node[,] CreatGrid(int gridX, int gridY, Vector3 startPos, float Size, Vector3 Dimesion, LayerMask unWalk, LayerMask unBuild)
    {
        grid = new Node[gridX, gridY];
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 worldPos = startPos + Vector3.right * (x * Size + (Size / 2)) + Vector3.forward * (y * Size + (Size / 2));
                bool walkAble = (Physics.CheckBox(worldPos, Dimesion / 2, Quaternion.identity, unWalk));
                bool buidAble = (Physics.CheckBox(worldPos, Dimesion / 2, Quaternion.identity, unBuild));
                grid[x, y] = new Node(worldPos, walkAble, buidAble, x, y);
            }
        }
        return grid;
    }

    public void UpdateNode(int x, int y, bool walkAble, bool buildAble)
    {
        grid[x, y].walkAble = walkAble;
        grid[x, y].buildAble = buildAble;
        if (projecter != null)
            projecter.UpdateGridPixel(x, y);
    }

    public void ReverseNodes(List<Node> tempNodes)
    {
        for (int i = 0; i < tempNodes.Count; i++)
        {
            tempNodes[i].walkAble = orignalWalk[tempNodes[i].gridX, tempNodes[i].gridY];
            tempNodes[i].buildAble = orignalBuild[tempNodes[i].gridX, tempNodes[i].gridY];
        }
    }

    private void CreateOrignalbool()
    {
        orignalBuild = new bool[gridSizeX, gridSizeY];
        orignalWalk = new bool[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                orignalBuild[x, y] = grid[x, y].buildAble;
                orignalWalk[x, y] = grid[x, y].walkAble;
            }
        }
    }

    private float GetNodeHeight(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position+Vector3.up*50, -Vector3.up, out hit, Mathf.Infinity, LayerMask.GetMask("BuildLayer")))
            return hit.point.y;
        return 0f;
    }
}
