using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : BuildingPlaceable
{
	[SerializeField]
	private int range, damage;

	private HashSet<Monster> _trackedMonsters;

	public override void Awake()
	{
		base.Awake();
		_trackedMonsters = new HashSet<Monster>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();

		if (!isPlaced)
		{
			return;
		}
		
		foreach (Monster monster in MonsterManager.Instance.allMonsters)
		{
			bool isTracked = _trackedMonsters.Contains(monster);
			bool isInRange = IsTargetInRange(monster.transform.position);

			if (!isTracked && isInRange)
			{
				Track(monster);
			}
			else if (isTracked && !isInRange)
			{
				UnTrack(monster);
			}
		}
	}

	private bool IsTargetInRange(Vector2 targetPos)
	{
		Vector2 towerPosition2D = new Vector2(transform.position.x, transform.position.y);
		Vector2 delta = targetPos - towerPosition2D;

		float unstretchedDimension = NavigationSystem.Instance.gridLayout.cellSize.x;
		float stretchedDimension = NavigationSystem.Instance.gridLayout.cellSize.y;
		float stretchFactor = unstretchedDimension / stretchedDimension;

		float inequality = delta.x * delta.x + stretchFactor * stretchFactor * delta.y * delta.y;

		return Mathf.Sqrt(inequality) <= range;
	}

	private void Track(Monster trackedEntity)
	{
		_trackedMonsters.Add(trackedEntity);
	}

	private void UnTrack(Monster trackedEntity)
	{
		_trackedMonsters.Remove(trackedEntity);
	}
}
