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

	[HideInInspector]
	public DirectionStatus directionStatus;

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
	 * Do not call this method unless you are 100% sure what you are doing
	 */
	public virtual void DeSpawnInternal()
	{
		Destroy(gameObject);
	}
}
