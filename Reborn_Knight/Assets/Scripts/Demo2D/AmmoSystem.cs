using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class AmmoSystem : MonoBehaviour
    {
        [SerializeField] private int maxAmmo = 12;
        [SerializeField] private int currentAmmo;

        public event Action<int, int> OnAmmoChanged;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => maxAmmo;

        private void Awake()
        {
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }

        private void Start()
        {
            if (currentAmmo == 0)
            {
                currentAmmo = maxAmmo;
            }

            NotifyAmmoChanged();
        }

        public bool CanConsume(int amount)
        {
            return currentAmmo >= amount;
        }

        public void ConsumeAmmo(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            currentAmmo = Mathf.Max(0, currentAmmo - amount);
            NotifyAmmoChanged();
        }

        public void AddAmmo(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
            NotifyAmmoChanged();
        }

        private void NotifyAmmoChanged()
        {
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }
    }
}
