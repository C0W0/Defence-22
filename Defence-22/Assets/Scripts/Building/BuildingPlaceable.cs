using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceable : MonoBehaviour
{
	[HideInInspector]
	public bool isPlaced;

	private Bounds _bounds;
	private bool _isDragging;

	private void Awake()
	{
		isPlaced = false;
		_isDragging = false;

		transform.position = Vector3.zero;

		Vector2 centre = transform.position;
		_bounds = new Bounds(centre, GetComponentInChildren<SpriteRenderer>().sprite.bounds.size);

		BuildingTooltip.Instance.ShowTooltip(this);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (!isPlaced)
		{
			UpdateSelected();

			if (_isDragging)
			{
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				// do a world-to-cell conversion to get the grid location
				// then do a cell-to-local to convert the grid location back to world to snap the building to a grid
				Vector3Int cellPos = BuildingSystem.Instance.gridLayout.WorldToCell(mousePos);
				transform.position = BuildingSystem.Instance.gridLayout.CellToLocalInterpolated(cellPos);
			}

			return;
		}
	}

	private void UpdateSelected()
	{
		if (_isDragging && Input.GetMouseButtonUp(0))
		{
			_isDragging = false;

			// when mouse up, move the bounds to the location of the building
			Vector2 centre = transform.position;
			_bounds = new Bounds(centre, _bounds.size);

			// show tooltip when the building not being dragged
			BuildingTooltip.Instance.ShowTooltip(this);

			return;
		}

		if (!_isDragging && Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (_bounds.Contains(mousePos))
			{
				_isDragging = true;

				// hide tooltip when dragging the building
				BuildingTooltip.Instance.HideTooltip();
			}
		}
	}

	public void Place()
	{
		if (TryPlaceBuilding())
		{
			BuildingTooltip.Instance.HideTooltip();
		}
		else
		{
			print("Cannot deploy here");
		}
	}

	private bool TryPlaceBuilding()
	{
		if (BuildingSystem.Instance.IsTaken(transform.position))
		{
			return false;
		}

		BuildingSystem.Instance.PlaceBuilding(transform.position);
		isPlaced = true;

		return true;
	}
}
