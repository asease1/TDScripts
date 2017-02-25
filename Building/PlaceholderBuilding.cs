using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlaceholderBuilding : MonoBehaviour {

    public LayerMask layerMask;
    public int buildingSize;
    public string[] buildPattern;
    public GameObject BuildTower;
    public float heightDiffrent;
    private bool placed;

    List<List<bool>> replaceGrid = new List<List<bool>>();

    void Start () {
        int xlist = 0;
        foreach(string x in buildPattern)
        {
            replaceGrid.Add(new List<bool>());
            foreach (char y in x.ToCharArray())
            {
                if (y == '1')            
                    replaceGrid[xlist].Add(true);              
                else
                    replaceGrid[xlist].Add(false);
            }
            xlist++;
        }
        replaceGrid.Reverse();
        foreach (List<bool> x in replaceGrid)
        {
            string temp = "";
            foreach (bool y in x)
            {
                temp += y.ToString();
            }
        }
    }

	void Update () {
        Ray screenToWorld = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (!placed && Physics.Raycast(screenToWorld.origin, screenToWorld.direction, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 direction = Vector3.zero;
            Node node = Grid.instance.NodeFromWorldPoint(hit.point);
            if (node != null)
            {

                if (hit.point.x > node.worldPosition.x && hit.point.z > node.worldPosition.z)
                    direction = new Vector3(1, 0, 1);
                else if (hit.point.x < node.worldPosition.x && hit.point.z > node.worldPosition.z)
                    direction = new Vector3(-1, 0, 1);
                else if (hit.point.x > node.worldPosition.x && hit.point.z < node.worldPosition.z)
                    direction = new Vector3(1, 0, -1);
                else
                    direction = new Vector3(-1, 0, -1);

                Vector3 position = node.worldPosition + direction * (Grid.instance.cubeSize / 2) * (buildingSize - 1);
                position.x = Mathf.Clamp(position.x, Grid.instance.startOffsetX - direction.x * Grid.instance.cubeSize * (buildingSize - 1), Grid.instance.gridSize.x - Grid.instance.startOffsetX + direction.x * Grid.instance.cubeSize  * (buildingSize - 1));
                position.z = Mathf.Clamp(position.z, Grid.instance.startOffSetZ - direction.z * Grid.instance.cubeSize * (buildingSize - 1), Grid.instance.gridSize.y - Grid.instance.startOffSetZ + direction.z * Grid.instance.cubeSize  * (buildingSize - 1));
                position.y = hit.point.y;

                transform.position = position;
            }
        }

        if (placed && Physics.Raycast(screenToWorld.origin, screenToWorld.direction, out hit, Mathf.Infinity, layerMask))
        {
            float x = transform.position.x - hit.point.x;
            float z = transform.position.z - hit.point.z;

            if (Mathf.Abs(x) < Mathf.Abs(z))
                transform.LookAt(transform.position + new Vector3(0, 0, z));
            else
                transform.LookAt(transform.position + new Vector3(x, 0, 0));
        }


        if (!placed && Input.GetMouseButtonDown(0))
        {
            placed = true;
        }
        else if (placed && Input.GetMouseButtonUp(0))
        {
            List<Node> tempNode = Grid.GetSqueareNodes(transform.position, buildingSize);

            List<List<bool>> replaceGridTemp = RotateMatrix<bool>(Mathf.RoundToInt(transform.eulerAngles.y), replaceGrid);
            if (CheckIfPlaceMent(replaceGridTemp, tempNode) || !CheckHeightDiffrent(replaceGridTemp, tempNode))
            {
                Destroy(gameObject);
                return;

            }
            
            UpdateGrid(replaceGridTemp, tempNode);

            bool foundPath = false;
            List<Node> checkNodes = CheckNodes(tempNode);

            if (checkNodes.Count > 0)
            {
                foundPath = !PathRequestManager.Instance.pathFinding.Reverse(checkNodes[0]);

                if (foundPath)
                {
                    Grid.instance.ReverseNodes(tempNode);
                    Destroy(gameObject);
                    return;
                }
            }
            Instantiate(BuildTower, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private List<List<T>> RotateMatrix<T>(int rotation, List<List<T>> matrix)
    {
        List<List<T>> newMatrix = new List<List<T>>();
        for(int i = 0; i < matrix.Count; i++)
        {
            newMatrix.Add(new List<T>());
        }

        if(rotation == 0)
        {
            return matrix;
        }
        else if (rotation == 180)
        {
            for (int x = 0; x < matrix.Count; x++)
            {
                matrix[x].Reverse();
            }
            matrix.Reverse();
            return matrix;
        }
        else if (rotation == 270)
        {          
            
            for(int x = 0; x < matrix.Count; x++)
            {
                int matrixX = 0;
                for(int y = matrix[x].Count; y > 0; y--)
                {
                    newMatrix[matrixX].Add(matrix[x][y-1]);
                    matrixX++;
                }
            }
        }
        else if (rotation == 90)
        {
            for (int x = matrix.Count; x > 0; x--)
            {
                int matrixX = 0;
                for (int y = 0; y < matrix[x-1].Count; y++)
                {
                    newMatrix[matrixX].Add(matrix[x-1][y]);
                    matrixX++;
                }
            }
        }
        return newMatrix;
    }

    private bool CheckIfPlaceMent(List<List<bool>> replaceGrid, List<Node> checkNodes)
    {
        int count = 0;
        for (int x = 0; x < replaceGrid.Count; x++)
        {
            for (int y = 0; y < replaceGrid[x].Count; y++)
            {
                if (replaceGrid[x][y] == true && checkNodes[count].buildAble == true)
                    return true;
                count++;
            }
        }
        return false;
    }

    private bool CheckHeightDiffrent(List<List<bool>> replaceGrid, List<Node> checkNodes)
    {
        float startHeight;
        RaycastHit hit;
        if (Physics.Raycast(checkNodes[0].worldPosition + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity, layerMask))
            startHeight = hit.point.y;
        else
            return false;
        
        int count = 0;
        for (int x = 0; x < replaceGrid.Count; x++)
        {
            for (int y = 0; y < replaceGrid[x].Count; y++)
            {
                if (replaceGrid[x][y] && Physics.Raycast(checkNodes[count].worldPosition + Vector3.up * 50, Vector3.down, out hit, Mathf.Infinity, layerMask))
                {
                    if (Mathf.Abs(hit.point.y - startHeight) > heightDiffrent)
                        return false;
                }                
                count++;
            }
        }
        return true;
    }

    private void UpdateGrid(List<List<bool>> replaceGridTemp, List<Node> tempNode)
    {
        int count = 0;
        for (int x = 0; x < replaceGridTemp.Count; x++)
        {
            for (int y = 0; y < replaceGridTemp[x].Count; y++)
            {
                if (replaceGridTemp[x][y])
                    Grid.instance.UpdateNode(tempNode[count].gridX, tempNode[count].gridY, replaceGridTemp[x][y], replaceGridTemp[x][y]);
                count++;
            }
        }
    }

    private List<Node> CheckNodes(List<Node> nodes)
    {
        List<Node> returnNodes = new List<Node>();

        foreach (Node elm in nodes)
        {
            if (elm.end)
            {
                returnNodes.Add(elm);
            }
        }
        for(int i = 0; i < returnNodes.Count; i++)
        {
            if (returnNodes.Contains(returnNodes[i].child))
            {
                returnNodes.RemoveAt(i);
                i--;
            }
        }

        return returnNodes;

    }
}
