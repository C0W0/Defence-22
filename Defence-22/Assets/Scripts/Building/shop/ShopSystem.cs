using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public static ShopSystem Instance;

	void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // move this to a separate shop system
    public void CreateBuildingDrag(GameObject prefab)
    {
        GameObject newBuilding = Instantiate(prefab);
    }
}
