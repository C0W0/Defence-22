using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    // serializable class variable
    public int speed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        NavDirection direction = MainGridSystem.Instance.GetTileNavDirection(transform.position);
        float width = MainGridSystem.Instance.gridLayout.cellSize.x * speed * 0.01f;
        float height = MainGridSystem.Instance.gridLayout.cellSize.y * speed * 0.01f;
        
        switch (direction)
        {
            case NavDirection.North:
                transform.Translate(new Vector3(width, height, 0));
                break;
            case NavDirection.West:
                transform.Translate(new Vector3(-width, height, 0));
                break;
            case NavDirection.South:
                transform.Translate(new Vector3(-width, -height, 0));
                break;
            case NavDirection.East:
                transform.Translate(new Vector3(width, -height, 0));
                break;
        }
    }
}
