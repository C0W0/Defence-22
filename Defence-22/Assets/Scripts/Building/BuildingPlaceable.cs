using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceable : MonoBehaviour
{
	[HideInInspector]
	public bool isPlaced;
	public int cost;
	protected Bounds _bounds;
	private bool _isDragging;

	public virtual void Awake()
	{
		isPlaced = false;
		_isDragging = false;

		transform.position = Vector3.zero;

		Vector2 centre = transform.position;
		_bounds = new Bounds(centre, GetComponentInChildren<SpriteRenderer>().sprite.bounds.size);

		BuildingTooltip.Instance.ShowTooltip(this);
	}

	// Update is called once per frame
	public virtual void Update()
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
			CurrencySystem.Instance.UpdateCurrency(-cost);
			BuildingManager.Instance.TrackTower(this);
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

	public bool IsInBound(Vector2 position)
	{
		return (_bounds.Contains(position));
	}
	
}
