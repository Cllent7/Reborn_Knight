using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Combat")]
        [SerializeField] private float meleeRange = 1.2f;
        [SerializeField] private int meleeDamage = 1;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Health")]
        [SerializeField] private int maxHealth = 5;

        private int currentHealth;
        private Rigidbody2D rigidbody2D;
        private Vector2 moveInput;

        public event Action Died;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        private void Update()
        {
            ReadMovementInput();
            FaceMouseCursor();

            if (Input.GetButtonDown("Fire2"))
            {
                PerformMelee();
            }
        }

        private void FixedUpdate()
        {
            rigidbody2D.velocity = moveInput * moveSpeed;
        }

        private void ReadMovementInput()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(horizontal, vertical).normalized;
        }

        private void FaceMouseCursor()
        {
            var camera = Camera.main;
            if (camera == null)
            {
                return;
            }

            var mouseWorld = camera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mouseWorld - transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void PerformMelee()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, meleeRange, enemyLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out EnemySimpleAI enemy))
                {
                    enemy.TakeDamage(meleeDamage);
                }
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
                Died?.Invoke();
                gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, meleeRange);
        }
    }
}
