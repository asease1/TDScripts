using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Projector))]
public class ProjecterGrid : MonoBehaviour {

    Projector projecter;
    public Material projecterMaterial;
    public Color mainColor, unWalkColor, unBuildColor, unWalkBuildColor;
    public LayerMask layerMask;
    private Grid gridClass;
    private Texture2D textureGrid;

	void Start () {
        gridClass = GetComponent<Grid>();
        projecter = GetComponent<Projector>();
        projecter.material = projecterMaterial;
        textureGrid = projecter.material.GetTexture("_ShadowTex") as Texture2D;
        projecter.orthographic = true;
        projecter.orthographicSize = gridClass.gridSize.x / 2 + (gridClass.gridSize.x / (gridClass.gridSize.x / gridClass.cubeSize));
        projecter.ignoreLayers = layerMask;
        transform.eulerAngles = new Vector3(90, 0, 0);
        transform.position = new Vector3(gridClass.startOffsetX + (gridClass.gridSize.x / 2), gridClass.startOffsetY+50, gridClass.startOffSetZ + (gridClass.gridSize.y / 2));
        SetTextureColors();
    }
	
	void SetTextureColors () {
        for (int y = 1; y < textureGrid.height-1; y++)
        {
            for (int x = 1; x < textureGrid.width-1; x++)
            {
                UpdateGridPixel(x - 1, y - 1);
            }
        }
        textureGrid.Apply();
    }

    public void UpdateGridPixel(int x, int y)
    {
        Color color = mainColor;
        color = (gridClass.grid[x , y ].walkAble) ? unWalkColor : color;
        color = (gridClass.grid[x , y ].buildAble) ? unBuildColor : color;
        color = (gridClass.grid[x , y ].buildAble && gridClass.grid[x , y ].walkAble) ? unWalkBuildColor : color;
        textureGrid.SetPixel(x+1, y+1, color);
        textureGrid.Apply();
    }
}
