using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
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


	private Dictionary<TileBase, Vector2> _tileDirsStraight;
	private Dictionary<TileBase, Vector2[]> _tileDirsTurn;

	private void Awake()
	{
		Instance = this;
		
		float xMovement = gridLayout.cellSize.x;
		float yMovement = gridLayout.cellSize.y;

		_tileDirsStraight = new Dictionary<TileBase, Vector2>
		{
			{
				navTileNorth, new Vector2(xMovement, yMovement)
			},
			{
				navTileSouth, new Vector2(-xMovement, -yMovement)
			},
			{
				navTileEast, new Vector2(xMovement, -yMovement)
			},
			{
				navTileWest, new Vector2(-xMovement, yMovement)
			}
		};
		
		_tileDirsTurn = new Dictionary<TileBase, Vector2[]>
		{
			// left turn
			{
				navTileNToW, new []{_tileDirsStraight[navTileNorth], _tileDirsStraight[navTileWest]}
			},
			{
				navTileEToN, new []{_tileDirsStraight[navTileEast], _tileDirsStraight[navTileNorth]}
			},
			{
				navTileSToE, new []{_tileDirsStraight[navTileSouth], _tileDirsStraight[navTileEast]}
			},
			{
				navTileWToS, new []{_tileDirsStraight[navTileWest], _tileDirsStraight[navTileSouth]}
			},
			// right turn
			{
				navTileNToE, new []{_tileDirsStraight[navTileNorth], _tileDirsStraight[navTileEast]}
			},
			{
				navTileWToN, new []{_tileDirsStraight[navTileWest], _tileDirsStraight[navTileNorth]}
			},
			{
				navTileSToW, new []{_tileDirsStraight[navTileSouth], _tileDirsStraight[navTileWest]}
			},
			{
				navTileEToS, new []{_tileDirsStraight[navTileEast], _tileDirsStraight[navTileSouth]}
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
	
	public Vector2 GetTileNavDirection(Monster caller, Vector2 objectPos)
	{
		float speedFactor = Monster.SpeedMultiplier * caller.speed;
		
		Vector3Int gridCellPos = gridLayout.LocalToCell(objectPos);
		
		//Get the tile at that location.
		TileBase tile = mainTilemap.GetTile(new Vector3Int(gridCellPos.x, gridCellPos.y, 0));

		if (!tile)
		{
			print($"Monster {caller} stepped on a non-nav tile");
			caller.directionStatus = DirectionStatus.Straight;
			return Vector2.zero;
		}

		//straight direction
		if (_tileDirsStraight.TryGetValue(tile, out Vector2 direction))
		{
			caller.directionStatus = DirectionStatus.Straight;
			return direction * speedFactor;
		}
		
		// turning
		if (_tileDirsTurn.TryGetValue(tile, out Vector2[] directions))
		{
			if (caller.directionStatus == DirectionStatus.Straight)
			{
				caller.directionStatus = DirectionStatus.WillTurn;
			}
				
			Vector2 tileCentrePos = gridLayout.CellToLocalInterpolated(gridCellPos);
			// center the y-coordinate
			tileCentrePos += new Vector2(0, gridLayout.cellSize.y / 2);

			// new position if we use directions[0]
			Vector2 newPos0 = objectPos + directions[0] * speedFactor;
			float distance0 = (tileCentrePos - newPos0).magnitude;
			// new position if we use directions[0]
			Vector2 newPos1 = objectPos + directions[1] * speedFactor;
			float distance1 = (tileCentrePos - newPos1).magnitude;

			if (caller.directionStatus == DirectionStatus.WillTurn)
			{
				// direction before turn is better than direction after turn & the status is WillTurn:
				// it should continue with direction before turn until that leads to overshot
				if (distance0 < distance1)
				{
					return directions[0] * speedFactor;
				}
				// direction after turn is better than direction before turn & the status is WillTurn:
				// it should change the direction to after turn (otherwise it will overshoot)
				else
				{
					caller.directionStatus = DirectionStatus.HasTurned;
					return directions[1] * speedFactor;
				}
			}

			if (caller.directionStatus == DirectionStatus.HasTurned)
			{
				// if the monster will step foot on a different tile, reset the directionStatus
				if (mainTilemap.GetTile(gridLayout.LocalToCell(newPos1)) != tile)
				{
					caller.directionStatus = DirectionStatus.Straight;
				}
				return directions[1] * speedFactor;
			}
			
			Assert.IsTrue(false); // this line should never be reached
			return Vector2.zero;
		}

		print($"Monster {caller} stepped on a non-nav tile");
		caller.directionStatus = DirectionStatus.Straight;
		return Vector2.zero;
	}
}
