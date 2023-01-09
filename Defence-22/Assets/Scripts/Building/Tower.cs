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
	public string towerName;
	public Sprite icon;

	private Dictionary<Monster, TrackingLinkedListNode<Monster>> _trackedMonsters;

	protected Monster CurrentTarget;
	private Monster _lastTracked;

	[HideInInspector]
	public Vector3 towerPosition;

	public override void Awake()
	{
		base.Awake();
		_trackedMonsters = new Dictionary<Monster, TrackingLinkedListNode<Monster>>();
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
		
		foreach (Monster monster in MonsterManager.Instance.allSpawnedMonsters)
		{
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
		
		if (isPlaced)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (_bounds.Contains(mousePos) && Input.GetMouseButtonDown(0))
			{
				// if click, display sell/upgrade menu for building
				ModifyTooltip.Instance.ShowTooltip(this);
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

		if (!CurrentTarget)
		{
			CurrentTarget = trackedEntity;
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
		
		if (trackedEntity == CurrentTarget)
		{
			CurrentTarget = node.next;
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

	private string PrintLinkedList()
	{
		LinkedList<string> names = new LinkedList<string>();
		Monster curr = CurrentTarget;
		while (curr)
		{
			names.AddLast(curr.name);
			curr = _trackedMonsters[curr].next;
		}

		string list = String.Join(",", names);
		print(list);
		return list;
	}
	#endregion
}

