using UnityEngine;

namespace RebornKnight.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private int damage = 1;

        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        public void Launch(Vector2 velocity)
        {
            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = velocity;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RebornKnight.World.Enemy enemy))
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
