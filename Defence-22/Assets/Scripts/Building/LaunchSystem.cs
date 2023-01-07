using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSystem : Tower
{
    [SerializeField]
    private float coolDown;

    private float _timeSinceLastAttack;

    public GameObject ammo;

    public override void Awake()
    {
        base.Awake();
        _timeSinceLastAttack = coolDown;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        _timeSinceLastAttack += Time.deltaTime;

        if (_currentTarget && _timeSinceLastAttack >= coolDown)
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
        towerProjectiles.Add(projectile);
    }

    public void Attack()
    {
        SpawnProjectile(ammo, towerPosition, _currentTarget);
    }
}
