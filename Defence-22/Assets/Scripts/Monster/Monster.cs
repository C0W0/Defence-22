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

	// serializable class variable
	public int speed;

	[HideInInspector]
	public DirectionStatus directionStatus;

	// Start is called before the first frame update
	void Start()
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
		// NavDirection direction = NavigationSystem.Instance.GetTileNavDirection(transform.position);
		// float width = NavigationSystem.Instance.gridLayout.cellSize.x * speed * SpeedMultiplier;
		// float height = NavigationSystem.Instance.gridLayout.cellSize.y * speed * SpeedMultiplier;
		//
		// switch (direction)
		// {
		//     case NavDirection.North:
		//         transform.Translate(new Vector3(width, height, 0));
		//         break;
		//     case NavDirection.West:
		//         transform.Translate(new Vector3(-width, height, 0));
		//         break;
		//     case NavDirection.South:
		//         transform.Translate(new Vector3(-width, -height, 0));
		//         break;
		//     case NavDirection.East:
		//         transform.Translate(new Vector3(width, -height, 0));
		//         break;
		// }
	}
}
