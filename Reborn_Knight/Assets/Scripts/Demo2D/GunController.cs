using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class GunController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float bulletSpeed = 12f;
        [SerializeField] private float fireCooldown = 0.2f;
        [SerializeField] private int ammoCost = 1;
        [SerializeField] private AmmoSystem ammoSystem;

        private float nextFireTime;

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                TryShoot();
            }
        }

        public void TryShoot()
        {
            if (Time.time < nextFireTime)
            {
                return;
            }

            if (ammoSystem == null || !ammoSystem.CanConsume(ammoCost))
            {
                return;
            }

            if (bulletPrefab == null || firePoint == null)
            {
                return;
            }

            nextFireTime = Time.time + fireCooldown;
            ammoSystem.ConsumeAmmo(ammoCost);

            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            if (bullet.TryGetComponent(out Bullet bulletComponent))
            {
                bulletComponent.Launch(firePoint.up * bulletSpeed);
            }
            else if (bullet.TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = firePoint.up * bulletSpeed;
            }
        }
    }
}
