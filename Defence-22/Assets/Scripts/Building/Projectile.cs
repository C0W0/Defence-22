using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int speed;
    [SerializeField]
    private float travelRange;
    
    [HideInInspector]
    public Monster target;
    
    private int _damage;
    private float _stretchFactor, _speedMultiplier;
    private float _distance;
    private Vector3 _spawnPoint;
    private Vector2 _direction;
    private LaunchSystem _launcher;
    
    public void Awake()
    {
        _stretchFactor = NavigationSystem.StretchFactor;
        _speedMultiplier = speed*Monster.SpeedMultiplier;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Projectile is spawned when target is within range, so start with a direction.
        _direction = target.transform.position - transform.position;
    }

    public void Initialize(LaunchSystem launcher, int damage)
    {
        _launcher = launcher;
        _damage = damage;
        _spawnPoint = transform.position;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector2 delta = transform.position - _spawnPoint;
        delta.y *= _stretchFactor;
        _distance = delta.magnitude;

        if (target && _distance <= travelRange)
        {
            _direction = target.transform.position - transform.position;

            Move(_direction/_direction.magnitude);
        }
        else if(!target && _distance <= travelRange) { //Even with no targets, keep moving in the same direction.
            Move(_direction/_direction.magnitude);
        }
        else
        {
            DespawnProjectile();
        }
    }

    protected void Move(Vector2 direction)
    {
        Vector2 displacement = direction * _speedMultiplier;
        transform.Translate(displacement);
    }

    protected void DespawnProjectile()
    {
        _launcher.RemoveProjectile(this);
        Destroy(gameObject);
    }

    //Upon collider contact, despawn.
    private void OnTriggerEnter2D(Collider2D col)
    {
        Monster monster = col.gameObject.GetComponent<Monster>();
        if (monster)
        {
            monster.TakeDamage(_damage);
            DespawnProjectile();
        }
    }
}
