using UnityEngine;

namespace RebornKnight.Demo2D
{
    // 子弹：直线飞行 + 命中敌人造成伤害 + 生命周期销毁
    // 使用说明：
    // 1) prefab 必须带 Rigidbody2D + Collider2D（Trigger）。
    // 2) 由 GunController 生成并调用 Launch 设置速度。
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        // 生命周期（秒）
        [SerializeField] private float lifeTime = 2f;
        // 伤害
        [SerializeField] private int damage = 1;

        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // 到时间自动销毁，防止场景堆积
            Destroy(gameObject, lifeTime);
        }

        public void Launch(Vector2 velocity)
        {
            // 赋予刚体速度
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = velocity;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 命中敌人造成伤害
            if (other.TryGetComponent(out EnemySimpleAI enemy))
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
