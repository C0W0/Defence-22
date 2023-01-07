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
	public Collider2D monsterCollider;

	public const float SpeedMultiplier = 0.001f;

	public float speed;
	public float health;
	public float armor;
	public float damage;

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
	 * Deregister the monster from all references except for MonsterManager and destroy the entity
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


	public void TakeDamage()
	{
		health -= damage * 1/armor;
		if (health <= 0)
		{
			MonsterManager.Instance.DeSpawnMonster(this);
		}
	}

	private void OnTriggerEnter2D()
	{
		TakeDamage();
	}
}
