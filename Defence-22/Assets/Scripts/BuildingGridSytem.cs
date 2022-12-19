using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * If a grid tile is taken, then we label it as a taken tile.
 * If it's taken, inform player that it's taken.
 * 
 */

public class BuildingGridSytem : MonoBehaviour
{

    public static BuildingGridSytem Instance;

    public GridLayout gridLayout;
    public Tilemap buildingGridMap;
    public TileBase takenTile;
 
    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsTaken(Vector2 objPos)
    {
        Vector3Int position = gridLayout.LocalToCell(objPos);
        return takenTile == buildingGridMap.GetTile(position);
    }
}
