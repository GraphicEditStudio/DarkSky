using System;
using UnityEngine;

namespace Utils
{
    public class HealthManager
    {
        private float startHealth;
        private float health;
        
        public event Action<float, float,float> HealthUpdated;

        public HealthManager()
        {
        }

        public void Initialize(float startHealth)
        {
            this.startHealth = startHealth;
            this.health = startHealth;
            HealthUpdated?.Invoke(startHealth, startHealth, startHealth);
        }

        public bool ManuallySetHealth(int newHealth)
        {
            var previousHealth = health;
            this.health = Mathf.Clamp(newHealth, 0, startHealth);
            HealthUpdated?.Invoke(startHealth, previousHealth, health);
            return IsAlive();
        }

        

        public bool TakeDamage(float damage)
        {
            var previousHealth = health;
            health = Mathf.Clamp(health -= damage, 0, startHealth);
            HealthUpdated?.Invoke(startHealth, previousHealth, health);
            return IsAlive();
        }

        public float Heal(float addHealth)
        {
            var previousHealth = health;
            var left = (health + addHealth) - startHealth;
            health = Mathf.Clamp(health += addHealth, 0, startHealth);
            HealthUpdated?.Invoke(startHealth, previousHealth, health);
            return Mathf.Clamp(left, 0, left);
        }

        public float GetCurrentHealth()
        {
            return health;
        }
        
        public bool IsAlive()
        {
            return health > 0;
        }
        
        public float GetHealthPercentage()
        {
            return health / startHealth;
        }
    }
}