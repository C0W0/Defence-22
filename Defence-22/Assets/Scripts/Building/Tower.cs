using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct TrackingLinkedListNode<T>
{
	public T prev;
	public T next;
	public TrackingLinkedListNode(T prev, T next)
	{
		this.prev = prev;
		this.next = next;
	}
}

public class Tower : BuildingPlaceable
{
	[SerializeField]
	protected int range, damage;

	private Dictionary<Monster, TrackingLinkedListNode<Monster>> _trackedMonsters;

	protected Monster _currentTarget;
	private Monster _lastTracked;

	public Vector3 towerPosition;

	public HashSet<Projectile> towerProjectiles;

	public override void Awake()
	{
		base.Awake();
		_trackedMonsters = new Dictionary<Monster, TrackingLinkedListNode<Monster>>();
		towerProjectiles = new HashSet<Projectile>();
	}

	// Start is called before the first frame update
	public virtual void Start()
	{
		towerPosition = transform.position;
	}

	// Update is called once per frame
	public override void Update()
	{
		base.Update();

		towerPosition = transform.position;

		if (!isPlaced)
		{
			return;
		}
		
		foreach (Monster monster in MonsterManager.Instance.allMonsters)
		{
			monster.damage = damage;
			bool isTracked = _trackedMonsters.ContainsKey(monster);
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
		Vector2 delta = targetPos - (Vector2)transform.position;
		float stretchFactor = NavigationSystem.StretchFactor;

		float inequality = delta.x * delta.x + stretchFactor * stretchFactor * delta.y * delta.y;

		return Mathf.Sqrt(inequality) <= range;
	}

	private void Track(Monster trackedEntity)
	{
		trackedEntity.trackingTowers.Add(this);
		_trackedMonsters.Add(trackedEntity, new TrackingLinkedListNode<Monster>(_lastTracked, null));
		
		if (_lastTracked)
		{
			var newLastTrackedNode = 
				new TrackingLinkedListNode<Monster>(_trackedMonsters[_lastTracked].prev, trackedEntity);
			_trackedMonsters[_lastTracked] = newLastTrackedNode;
		}

		if (!_currentTarget)
		{
			_currentTarget = trackedEntity;
		}
		
		_lastTracked = trackedEntity;
	}

	private void UnTrack(Monster trackedEntity)
	{
		trackedEntity.trackingTowers.Remove(this);
		RemoveFromTrackingList(trackedEntity);
	}

	public void RemoveFromTrackingList(Monster trackedEntity)
	{
		TrackingLinkedListNode<Monster> node = _trackedMonsters[trackedEntity];
		
		// removes the current node from the LinkedList
		if (trackedEntity == _lastTracked)
		{
			_lastTracked = node.prev;
		}
		else
		{
			if (node.next)
			{
				var newNodeForNext = new TrackingLinkedListNode<Monster>(node.prev, _trackedMonsters[node.next].next);
				_trackedMonsters[node.next] = newNodeForNext;
			}
		}
		
		if (trackedEntity == _currentTarget)
		{
			_currentTarget = node.next;
		}
		else
		{
			if (node.prev)
			{
				var newNodeForPrev = new TrackingLinkedListNode<Monster>(_trackedMonsters[node.prev].prev, node.next);
				_trackedMonsters[node.prev] = newNodeForPrev;
			}
		}
		
		_trackedMonsters.Remove(trackedEntity);
	}

	#region debug

	private void PrintLinkedList()
	{
		LinkedList<string> names = new LinkedList<string>();
		Monster curr = _currentTarget;
		while (curr)
		{
			names.AddLast(curr.name);
			curr = _trackedMonsters[curr].next;
		}
		print(String.Join(",", names));
	}
	#endregion
}

