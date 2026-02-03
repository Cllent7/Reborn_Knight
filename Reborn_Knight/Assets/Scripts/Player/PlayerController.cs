using System;
using UnityEngine;

namespace RebornKnight.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Ammo")]
        [SerializeField] private int maxAmmo = 12;
        [SerializeField] private int ammo;

        [Header("Combat")]
        [SerializeField] private RebornKnight.Combat.Weapon weapon;
        [SerializeField] private float meleeRange = 1.2f;
        [SerializeField] private int meleeDamage = 1;
        [SerializeField] private LayerMask enemyLayer;

        public event Action<int, int> AmmoChanged;

        public int Ammo => ammo;
        public int MaxAmmo => maxAmmo;

        private void Awake()
        {
            ammo = Mathf.Clamp(ammo, 0, maxAmmo);
        }

        private void Start()
        {
            if (ammo == 0)
            {
                ammo = maxAmmo;
            }

            NotifyAmmoChanged();
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                weapon.TryShoot(this);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                PerformMelee();
            }
        }

        public bool CanShoot(int cost)
        {
            return ammo >= cost;
        }

        public void ConsumeAmmo(int cost)
        {
            ammo = Mathf.Max(0, ammo - cost);
            NotifyAmmoChanged();
        }

        public void GainAmmo(int amount)
        {
            ammo = Mathf.Min(maxAmmo, ammo + amount);
            NotifyAmmoChanged();
        }

        private void PerformMelee()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out RebornKnight.World.Enemy enemy))
                {
                    enemy.TakeDamage(meleeDamage);
                }
            }
        }

        private void NotifyAmmoChanged()
        {
            AmmoChanged?.Invoke(ammo, maxAmmo);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }
    }
}
