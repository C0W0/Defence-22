using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapUtils
{
	public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
	{
		TileBase[] tileBlock = new TileBase[area.size.x * area.size.y];
		int i = 0;

		foreach (var v in area.allPositionsWithin)
		{
			tileBlock[i] = tilemap.GetTile(new Vector3Int(v.x, v.y, 0));
			i++;
		}

		return tileBlock;
	}

	public static void SetTilesBlock(BoundsInt area, Tilemap tilemap, TileBase tileBase)
	{
		TileBase[] tileBlock = new TileBase[area.size.x * area.size.y];
		CreateTileBlock(tileBlock, tileBase);
		tilemap.SetTilesBlock(area, tileBlock);
	}

	public static void CreateTileBlock(TileBase[] outTileBlock, TileBase tileBase)
	{
		for (int i = 0; i < outTileBlock.Length; i++)
			outTileBlock[i] = tileBase;
	}

	public static void ClearArea(BoundsInt area, Tilemap tilemap)
	{
		SetTilesBlock(area, tilemap, null);
	}
}

public enum NavDirection
{
	North, South, East, West, Default
}

public class MainGridSystem : MonoBehaviour
{

	public static MainGridSystem Instance;

	public GridLayout gridLayout;
	public Tilemap mainTilemap;
	public TileBase navTileNorth, navTileSouth, navTileEast, navTileWest;


	private Dictionary<TileBase, NavDirection> _tileDirections;

	private void Awake()
	{
		Instance = this;

		_tileDirections = new Dictionary<TileBase, NavDirection>();
		_tileDirections.Add(navTileNorth, NavDirection.North);
		_tileDirections.Add(navTileSouth, NavDirection.South);
		_tileDirections.Add(navTileEast, NavDirection.East);
		_tileDirections.Add(navTileWest, NavDirection.West);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{


	}

	public NavDirection GetTileNavDirection(Vector3Int gridCellPos)
	{
		//GetTile gives you the tile at that location.
		TileBase tile = mainTilemap.GetTile(new Vector3Int(gridCellPos.x, gridCellPos.y, 0));
		
		NavDirection direction;
		//maps tile to direction.
		if (_tileDirections.TryGetValue(tile, out direction))
		{
			return direction;
		}

		return NavDirection.Default;
	}

	
	public NavDirection GetTileNavDirection(Vector2 objectPos)
	{
		Vector3Int gridCellPos = gridLayout.LocalToCell(objectPos);
		return GetTileNavDirection(gridCellPos);
	}
}
