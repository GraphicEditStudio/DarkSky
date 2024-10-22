using System;
using UnityEngine;
using Utils;

public class EnemyDamagable : WeaponDamagable
{
    [SerializeField] private RagdollController ragdollController;
    [SerializeField] private MoveDestination moveDestination;
    [SerializeField] private Collider[] damageColliders;
    protected override void OnDeath()
    {
        moveDestination.StopMoving();
        //moveDestination.enabled = false;
        ragdollController.EnableRagdoll();
        Debug.Log("Enemy Died");
        foreach (var damageCollider in damageColliders)
        {
            damageCollider.enabled = false;
        }
    }

    protected override void OnHit()
    {
        
    }
}