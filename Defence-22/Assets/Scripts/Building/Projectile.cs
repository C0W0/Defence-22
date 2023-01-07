using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Collider2D projectileCollider;
    public Rigidbody2D projectileRigidBody;

    public float stretchFactor;

    public float speedMultiplier;
    public Vector3 spawnPoint;
    public float distance;
    public float travelRange;

    public Monster target;
    public Vector2 targetPos;
    public Vector2 direction;
    public float magnitude;
    
    public void Awake()
    {
        stretchFactor = NavigationSystem.StretchFactor;
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;

        //Projectile is spawned when target is within range, so start with a direction.
        direction = target.transform.position - transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        LaunchProjectile();
    }

    public void Move(Vector2 direction)
    {
        transform.Translate(direction*speedMultiplier);
    }

    public void LaunchProjectile()
    {
        Vector2 delta = transform.position - spawnPoint;
        distance = Mathf.Sqrt(delta.x * delta.x + stretchFactor * stretchFactor * delta.y * delta.y);

        if (target && distance <= travelRange)
        {
            direction = target.transform.position - transform.position;

            Move(Unitize(direction));
        }
        else if(!target && distance <= travelRange) { //Even with no targets, keep moving in the same direction.
            Move(Unitize(direction));
        }
        else
        {
            DespawnProjectile();
        }
    }

    //Have the direction as a unit vector constantly.
    public Vector2 Unitize(Vector2 direction)
    {
        magnitude = Mathf.Sqrt(direction.x * direction.x + stretchFactor * stretchFactor * direction.y * direction.y);
        Vector2 unitVector = direction / magnitude;

        return unitVector;
    }

    public void DespawnProjectile()
    {
        Destroy(gameObject);
    }

    //Upon collider contact, despawn.
    private void OnTriggerEnter2D()
    {
        DespawnProjectile();
    }
}
