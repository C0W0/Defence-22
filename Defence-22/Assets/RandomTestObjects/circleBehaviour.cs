using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class circleBehaviour : MonoBehaviour
{

    public Rigidbody2D folaRigidbody;

    public static circleBehaviour Instance;

    private void Awake()
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0.06f, 0));
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Translate(new Vector3(-0.06f, 0.06f, 0));
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Translate(new Vector3(0.06f, 0.06f, 0));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -0.06f, 0));
            if (Input.GetKey(KeyCode.LeftArrow))
                transform.Translate(new Vector3(-0.06f, -0.06f, 0));
            else if (Input.GetKey(KeyCode.RightArrow))
                transform.Translate(new Vector3(0.06f, -0.06f, 0));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.Translate(new Vector3(-0.06f, 0, 0));
        else if (Input.GetKey(KeyCode.RightArrow))
            transform.Translate(new Vector3(0.06f, 0, 0));
    }
}
/* When you add a script, the script exists as a component.
 * When you want to reference it, it gets the component on the 
 * game object.
 * Navigation system Instance for example. 
 * Game object is the object you are referering to.
 * If it's a global singleton we can do Class.Instance. and get the objects info
 * A system is attached
 * 
 * Track some specific components. You'll have to do a for loop.
 * If that distance is smaller than. 
 * 
 * Initialization of instance and in the awake function, instance = this;
 */