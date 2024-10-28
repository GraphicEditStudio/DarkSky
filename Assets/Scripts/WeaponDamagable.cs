using UnityEngine;
using UnityEngine.Serialization;
using Utils;

public abstract class WeaponDamagable : MonoBehaviour
{
    public GameObject hitEffects;
    [SerializeField] private float startHealth;
    private HealthManager _healthManager;
    
    private void Awake()
    {
        _healthManager = new HealthManager();
        _healthManager.Initialize(startHealth);
        var hitboxes = GetComponentsInChildren<Hitbox>();
        foreach (var hitbox in hitboxes)
        {
            hitbox.OnHitListeners += OnHit;
        }
    }

    private void OnHit(BodyParts bodyPart, float damage, RaycastHit castHit)
    {
        if (_healthManager.IsAlive())
        {
            if (this._healthManager.TakeDamage(damage))
            {
                OnHit();
            }
            else
            {
                OnDeath();
            }
        }
        var instance = Instantiate(hitEffects);
        instance.transform.position = castHit.point;
        instance.transform.forward = castHit.normal;  
    }

    protected abstract void OnDeath();

    protected abstract void OnHit();
}