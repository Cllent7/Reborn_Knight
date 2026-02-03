using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    // 2D 玩家控制器：WASD 移动 + 朝向鼠标 + 近战攻击 + 受伤死亡事件
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController2D : MonoBehaviour
    {
        [Header("Movement")]
        // 移动速度（单位/秒）
        [SerializeField] private float moveSpeed = 5f;

        [Header("Combat")]
        // 近战检测半径（以玩家为圆心）
        [SerializeField] private float meleeRange = 1.2f;
        // 近战伤害值
        [SerializeField] private int meleeDamage = 1;
        // 近战命中的敌人 Layer
        [SerializeField] private LayerMask enemyLayer;

        [Header("Health")]
        // 最大生命值
        [SerializeField] private int maxHealth = 5;

        private int currentHealth;
        private Rigidbody2D rigidbody2D;
        private Vector2 moveInput;

        // 玩家死亡事件（给 GameFlow 重开本局用）
        public event Action Died;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            currentHealth = maxHealth;
        }

        private void Update()
        {
            // 读取键盘输入
            ReadMovementInput();
            // 面向鼠标位置
            FaceMouseCursor();

            // 右键触发近战
            if (Input.GetButtonDown("Fire2"))
            {
                PerformMelee();
            }
        }

        private void FixedUpdate()
        {
            // 通过刚体移动（物理帧）
            rigidbody2D.velocity = moveInput * moveSpeed;
        }

        private void ReadMovementInput()
        {
            // Unity 默认输入轴：WASD/方向键
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            moveInput = new Vector2(horizontal, vertical).normalized;
        }

        private void FaceMouseCursor()
        {
            // 使用主摄像机把鼠标屏幕坐标转换为世界坐标
            var camera = Camera.main;
            if (camera == null)
            {
                return;
            }

            var mouseWorld = camera.ScreenToWorldPoint(Input.mousePosition);
            var direction = mouseWorld - transform.position;
            // 让玩家朝向鼠标（朝向上方作为枪口方向）
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void PerformMelee()
        {
            // 近战范围内检测敌人并造成伤害
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
                // 触发死亡事件，让 GameFlow 重载场景
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
