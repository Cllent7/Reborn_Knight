using UnityEngine;

namespace RebornKnight.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 12f;
        [SerializeField] private float fireCooldown = 0.2f;
        [SerializeField] private int ammoCost = 1;

        private float nextFireTime;

        public void TryShoot(RebornKnight.Player.PlayerController player)
        {
            if (Time.time < nextFireTime)
            {
                return;
            }

            if (!player.CanShoot(ammoCost))
            {
                return;
            }

            nextFireTime = Time.time + fireCooldown;
            player.ConsumeAmmo(ammoCost);

            if (bulletPrefab == null || firePoint == null)
            {
                return;
            }

            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent(out Projectile projectile))
            {
                projectile.Launch(firePoint.up * bulletSpeed);
            }
            else if (bullet.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = firePoint.up * bulletSpeed;
            }
        }
    }
}
