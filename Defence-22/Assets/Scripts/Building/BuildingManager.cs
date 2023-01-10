using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    private HashSet<BuildingPlaceable> activeTowers;
    void Awake()
    {
        Instance = this;
        activeTowers = new HashSet<BuildingPlaceable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ModifyTooltip.Instance.HideTooltip();
            CheckMouseOnTower();
        }
    }

    private void CheckMouseOnTower()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (BuildingPlaceable tower in activeTowers)
        {
            if (tower.IsInBound(mousePos) && Input.GetMouseButtonDown(0))
            {
                ModifyTooltip.Instance.ShowTooltip(tower);
            }

        }

    }

    public void TrackTower(BuildingPlaceable tower)
    {
        activeTowers.Add(tower);
    }

    public void UntrackTower(BuildingPlaceable tower)
    {
        activeTowers.Remove(tower);
    }
    
}
