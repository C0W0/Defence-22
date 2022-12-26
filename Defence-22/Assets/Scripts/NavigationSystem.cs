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
	North, South, East, West, 
	NToW, EToN, SToE, WToS, NToE, WToN, SToW, EToS,
	Default
}

public class NavigationSystem : MonoBehaviour
{

	public static NavigationSystem Instance;

	public GridLayout gridLayout;
	public Tilemap mainTilemap;
	public TileBase navTileNorth, navTileSouth, navTileEast, navTileWest;
	// turing tiles
	public TileBase navTileNToW, navTileEToN, navTileSToE, navTileWToS;
	public TileBase navTileNToE, navTileWToN, navTileSToW, navTileEToS;


	private Dictionary<TileBase, NavDirection> _tileDirectionsStraight;
	private Dictionary<TileBase, NavDirection> _tileDirectionsTurn;

	private void Awake()
	{
		Instance = this;

		_tileDirectionsStraight = new Dictionary<TileBase, NavDirection>
		{
			{
				navTileNorth, NavDirection.North
			},
			{
				navTileSouth, NavDirection.South
			},
			{
				navTileEast, NavDirection.East
			},
			{
				navTileWest, NavDirection.West
			}
		};

		_tileDirectionsTurn = new Dictionary<TileBase, NavDirection>
		{
			// left turn
			{
				navTileNToW, NavDirection.NToW
			},
			{
				navTileEToN, NavDirection.EToN
			},
			{
				navTileSToE, NavDirection.SToE
			},
			{
				navTileWToS, NavDirection.WToS
			},
			// right turn
			{
				navTileNToE, NavDirection.NToE
			},
			{
				navTileWToN, NavDirection.WToN
			},
			{
				navTileSToW, NavDirection.SToW
			},
			{
				navTileEToS, NavDirection.EToS
			}
		};
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

		if (!tile)
		{
			return NavDirection.Default;
		}
		
		NavDirection direction;
		//straight direction
		if (_tileDirectionsStraight.TryGetValue(tile, out direction))
		{
			return direction;
		}
		// turning
		if (_tileDirectionsTurn.TryGetValue(tile, out direction))
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
