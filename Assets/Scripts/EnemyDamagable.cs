using System;
using UnityEngine;

public class EnemyDamagable : WeaponDamagable
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    protected override void OnHit()
    {
        
    }
}