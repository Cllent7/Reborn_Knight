using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class GunController : MonoBehaviour
    {
        // 子弹生成位置
        [SerializeField] private Transform firePoint;
        // 子弹预制体（带 Bullet 脚本）
        [SerializeField] private GameObject bulletPrefab;
        // 子弹速度
        [SerializeField] private float bulletSpeed = 12f;
        // 射击冷却
        [SerializeField] private float fireCooldown = 0.2f;
        // 单次射击消耗弹药
        [SerializeField] private int ammoCost = 1;
        // 弹药系统引用（必须绑定）
        [SerializeField] private AmmoSystem ammoSystem;

        private float nextFireTime;

        private void Update()
        {
            // 左键持续射击（按住）
            if (Input.GetButton("Fire1"))
            {
                TryShoot();
            }
        }

        public void TryShoot()
        {
            // 冷却中则禁止射击
            if (Time.time < nextFireTime)
            {
                return;
            }

            // 弹药不足或未绑定弹药系统 -> 禁止射击
            if (ammoSystem == null || !ammoSystem.CanConsume(ammoCost))
            {
                return;
            }

            // 未配置子弹或枪口 -> 直接返回，避免空引用
            if (bulletPrefab == null || firePoint == null)
            {
                return;
            }

            nextFireTime = Time.time + fireCooldown;
            ammoSystem.ConsumeAmmo(ammoCost);

            // 生成子弹并赋予初速度
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
