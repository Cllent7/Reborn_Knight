using System;
using UnityEngine;

namespace RebornKnight.World
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        private int currentHealth;

        public event Action<Enemy> Died;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0)
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth - amount);
            if (currentHealth == 0)
            {
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
