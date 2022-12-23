using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlaceable : MonoBehaviour
{
    [HideInInspector]
    public bool isPlaced;

    private Bounds _bounds;
    private bool _isSelected;

    private void Awake()
    {
        isPlaced = false;
        _isSelected = false;
        
        transform.position = Vector3.zero;
        
        Vector2 centre = transform.position;
        _bounds = new Bounds(centre, GetComponent<SpriteRenderer>().sprite.bounds.size);
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
            
            if (_isSelected)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePos;
            }
            
            return;
        }
    }

    private void UpdateSelected()
    {
        if (_isSelected && Input.GetMouseButtonUp(0))
        {
            _isSelected = false;
            
            Vector2 centre = transform.position;
            _bounds = new Bounds(centre, _bounds.size);
            
            return;
        }

        if (!_isSelected && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (_bounds.Contains(mousePos))
            {
                _isSelected = true;
            }
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
