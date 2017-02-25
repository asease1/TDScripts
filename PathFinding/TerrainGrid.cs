using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainGrid : MonoBehaviour
{
    public float yOffset = 0.5f;
    public Material cellMaterialValid;
    public Material cellMaterialInvalid;

    private Grid gridClass;
    private GameObject[] _cells;
    private float[] _heights;
    private Transform parrent;

    void Start()
    {
        parrent = new GameObject("TerrainGridParrent").transform;
        gridClass = GetComponent<Grid>();
        _cells = new GameObject[gridClass.MaxSize];
        _heights = new float[(gridClass.GridX + 1) * (gridClass.GridY + 1)];

        for (int z = 0; z < gridClass.GridX; z++)
        {
            for (int x = 0; x < gridClass.GridY; x++)
            {
                _cells[z * gridClass.GridY + x] = CreateChild();
            }
        }
        UpdatePosition();
        UpdateHeights();
        UpdateCells();
    }

    void Update()
    {
        //UpdateSize();
        
    }

    GameObject CreateChild()
    {
        GameObject go = new GameObject();

        go.name = "Grid Cell";
        go.transform.parent = parrent;
        go.transform.localPosition = Vector3.zero;
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>().mesh = CreateMesh();

        return go;
    }

    void UpdateSize()
    {
        int newSize = gridClass.GridX * gridClass.GridY;
        int oldSize = _cells.Length;

        if (newSize == oldSize)
            return;

        GameObject[] oldCells = _cells;
        _cells = new GameObject[newSize];

        if (newSize < oldSize)
        {
            for (int i = 0; i < newSize; i++)
            {
                _cells[i] = oldCells[i];
            }

            for (int i = newSize; i < oldSize; i++)
            {
                Destroy(oldCells[i]);
            }
        }
        else if (newSize > oldSize)
        {
            for (int i = 0; i < oldSize; i++)
            {
                _cells[i] = oldCells[i];
            }

            for (int i = oldSize; i < newSize; i++)
            {
                _cells[i] = CreateChild();
            }
        }

        _heights = new float[(gridClass.GridX + 1) * (gridClass.GridY + 1)];
    }

    void UpdatePosition()
    {
        /*RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, LayerMask.GetMask("BuildLayer"));
        Vector3 position = hitInfo.point;

        position.x -=  (hitInfo.point.x % gridClass.cubeSize + gridClass.GridY * gridClass.cubeSize / 2);
        position.z -=  (hitInfo.point.z % gridClass.cubeSize + gridClass.GridX * gridClass.cubeSize / 2);
        position.y = 0;*/

        parrent.position = gridClass.StartPosition;
    }

    void UpdateHeights()
    {
        RaycastHit hitInfo;
        Vector3 origin;

        for (int z = 0; z < gridClass.GridX + 1; z++)
        {
            for (int x = 0; x < gridClass.GridY + 1; x++)
            {
                origin = new Vector3(x * gridClass.cubeSize, 200, z * gridClass.cubeSize);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("BuildLayer"));

                _heights[z * (gridClass.GridY + 1) + x] = hitInfo.point.y;
            }
        }
    }

    void UpdateCells()
    {
        for (int z = 0; z < gridClass.GridX; z++)
        {
            for (int x = 0; x < gridClass.GridY; x++)
            {
                GameObject cell = _cells[z * gridClass.GridY + x];
                MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = cell.GetComponent<MeshFilter>();

                meshRenderer.material = IsCellValid(x, z) ? cellMaterialValid : cellMaterialInvalid;
                UpdateMesh(meshFilter.mesh, x, z);
            }
        }
    }

    bool IsCellValid(int x, int z)
    {
        /*RaycastHit hitInfo;
        Vector3 origin = new Vector3(x * gridClass.cubeSize + gridClass.cubeSize / 2, 200, z * gridClass.cubeSize + gridClass.cubeSize / 2);
        Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("unBuildAble"));*/

        //return hitInfo.collider == null;
        return !gridClass.grid[x, z].buildAble;
    }

    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.name = "Grid Cell";
        mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };

        return mesh;
    }

    void UpdateMesh(Mesh mesh, int x, int z)
    {
        mesh.vertices = new Vector3[] {
            MeshVertex(x, z),
            MeshVertex(x, z + 1),
            MeshVertex(x + 1, z),
            MeshVertex(x + 1, z + 1),
        };
    }

    Vector3 MeshVertex(int x, int z)
    {
        return new Vector3(x * gridClass.cubeSize, _heights[z * (gridClass.GridY + 1) + x] + yOffset, z * gridClass.cubeSize);
    }
}