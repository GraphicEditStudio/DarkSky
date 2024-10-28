using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    [SerializeField] private BodyParts Part;
    public event Action<BodyParts, float, RaycastHit> OnHitListeners;
    
    public void OnHit(float damage, RaycastHit hit)
    {
        OnHitListeners?.Invoke(Part, damage, hit);
    }
}

public enum BodyParts
{
    Head,
    Body,
    UpperArmLeft,
    LowerArmLeft,
    UpperLegLeft,
    LowerLegLeft,
    UpperArmRight,
    LowerArmRight,
    UpperLegRight,
    LowerLegRight,
    Random1,
    Random2
}