using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    // 简单敌人 AI：追击玩家 + 近距离接触伤害
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemySimpleAI : MonoBehaviour
    {
        // 最大生命值
        [SerializeField] private int maxHealth = 3;
        // 移动速度
        [SerializeField] private float moveSpeed = 2f;
        // 接触伤害
        [SerializeField] private int contactDamage = 1;
        // 接触伤害冷却（避免每帧扣血）
        [SerializeField] private float contactCooldown = 0.5f;

        private int currentHealth;
        private Rigidbody2D rigidbody2D;
        private Transform target;
        private float nextDamageTime;

        public event Action<EnemySimpleAI> Died;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            // 自动寻找玩家（简化做法）
            var player = FindObjectOfType<PlayerController2D>();
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void FixedUpdate()
        {
            // 没有目标则不移动
            if (target == null)
            {
                return;
            }

            // 朝玩家移动
            var direction = (target.position - transform.position).normalized;
            rigidbody2D.velocity = direction * moveSpeed;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // 伤害冷却中则忽略
            if (Time.time < nextDamageTime)
            {
                return;
            }

            // 与玩家持续碰撞则造成伤害
            if (collision.collider.TryGetComponent(out PlayerController2D player))
            {
                nextDamageTime = Time.time + contactCooldown;
                player.TakeDamage(contactDamage);
            }
        }

        public void TakeDamage(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            currentHealth = Mathf.Max(0, currentHealth - amount);
            if (currentHealth == 0)
            {
                // 触发死亡事件供房间统计
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
