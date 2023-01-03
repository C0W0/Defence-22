using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum DirectionStatus
{
	Straight, WillTurn, HasTurned
}

public class Monster : MonoBehaviour
{
	public const float SpeedMultiplier = 0.001f;

	public int speed;
	public int health;
	public int armor;

	[HideInInspector]
	public DirectionStatus directionStatus;
	public HashSet<Tower> trackingTowers;

	public virtual void Awake()
	{
		trackingTowers = new HashSet<Tower>();
	}
	
	// Start is called before the first frame update
	public virtual void Start()
	{
		
	}

	// Update is called once per frame
	public virtual void Update()
	{
		Move();
	}

	protected void Move()
	{
		Vector2 direction = NavigationSystem.Instance.GetTileNavDirection(this, transform.position);
		transform.Translate(direction);
	}

	/**
	 * Erase the 
	 * Do not call this method unless you are 100% sure what you are doing
	 */
	public virtual void DeSpawnInternal()
	{
		foreach (Tower tower in trackingTowers)
		{
			tower.RemoveFromTrackingList(this);
		}
		
		Destroy(gameObject);
	}

	/**
	 * Call upon collision with projectile
	 */
	public void TakeDamage(int damage)
	{
		health -= damage * 1/armor;
		if (health <= 0)
		{
			MonsterManager.Instance.DeSpawnMonster(this);
		}
	}
}
