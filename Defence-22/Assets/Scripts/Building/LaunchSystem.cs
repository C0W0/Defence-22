using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSystem : Tower
{
    public GameObject ammo;
    
    [SerializeField]
    private float coolDown;

    private HashSet<Projectile> _towerProjectiles;
    private float _timeSinceLastAttack;

    public override void Awake()
    {
        base.Awake();
        _timeSinceLastAttack = coolDown;
        _towerProjectiles = new HashSet<Projectile>();
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        _timeSinceLastAttack += Time.deltaTime;

        if (CurrentTarget && _timeSinceLastAttack >= coolDown)
        {
            Attack();
            _timeSinceLastAttack = 0f;
        }
    }

    public void SpawnProjectile(GameObject projectilePrefab, Vector3 objPos, Monster target)
    {
        GameObject projectileObj = Instantiate(projectilePrefab);
        projectileObj.transform.position = objPos;

        //Manipulate game object through projectile.
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.target = target;

        //Add to the list of projectiles.
        _towerProjectiles.Add(projectile);
        
        projectile.Initialize(this, damage);
    }

    public void Attack()
    {
        SpawnProjectile(ammo, towerPosition, CurrentTarget);
    }

    public void RemoveProjectile(Projectile projectile)
    {
        _towerProjectiles.Remove(projectile);
    }
}
