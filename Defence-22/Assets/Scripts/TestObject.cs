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
            print(MainGridSystem.Instance.GetTileNavDirection(transform.position));
            transform.Translate(new Vector3(0, 0.1f, 0));

        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(BuildingGridSytem.Instance.IsTaken(transform.position));
        }


    }
}
