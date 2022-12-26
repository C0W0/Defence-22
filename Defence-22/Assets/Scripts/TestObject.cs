using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*Input.GetKeyDown(KeyCode.k) returns true if a key is pressed during a frame.
         * KeyCode.Key is useful to determine the key.
         * 
         */
       
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(NavigationSystem.Instance.GetTileNavDirection(transform.position));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            BuildingSystem.Instance.RemoveBuilding(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            BuildingSystem.Instance.PlaceBuilding(transform.position);
        }


    }
}
