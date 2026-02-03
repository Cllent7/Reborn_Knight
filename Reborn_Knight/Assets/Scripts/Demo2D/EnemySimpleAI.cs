using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemySimpleAI : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 3;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private int contactDamage = 1;
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
            var player = FindObjectOfType<PlayerController2D>();
            if (player != null)
            {
                target = player.transform;
            }
        }

        private void FixedUpdate()
        {
            if (target == null)
            {
                return;
            }

            var direction = (target.position - transform.position).normalized;
            rigidbody2D.velocity = direction * moveSpeed;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (Time.time < nextDamageTime)
            {
                return;
            }

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
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
