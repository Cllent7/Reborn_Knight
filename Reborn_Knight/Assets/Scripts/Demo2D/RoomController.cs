using System;
using System.Collections.Generic;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class RoomController : MonoBehaviour
    {
        [SerializeField] private Chest chestPrefab;
        [SerializeField] private Transform chestSpawnPoint;

        private readonly List<EnemySimpleAI> enemies = new List<EnemySimpleAI>();
        private bool cleared;

        public event Action<RoomController> RoomCleared;
        public event Action<RoomController, Chest> ChestSpawned;

        private void Awake()
        {
            enemies.Clear();
            GetComponentsInChildren(true, enemies);
            foreach (var enemy in enemies)
            {
                enemy.Died += HandleEnemyDied;
            }
        }

        private void OnDestroy()
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.Died -= HandleEnemyDied;
                }
            }
        }

        public void RegisterEnemy(EnemySimpleAI enemy)
        {
            if (enemy == null || enemies.Contains(enemy))
            {
                return;
            }

            enemies.Add(enemy);
            enemy.Died += HandleEnemyDied;
        }

        public void ActivateRoom(bool active)
        {
            gameObject.SetActive(active);
        }

        private void HandleEnemyDied(EnemySimpleAI enemy)
        {
            enemy.Died -= HandleEnemyDied;
            enemies.Remove(enemy);

            if (!cleared && enemies.Count == 0)
            {
                cleared = true;
                RoomCleared?.Invoke(this);
                SpawnChest();
            }
        }

        private void SpawnChest()
        {
            if (chestPrefab == null)
            {
                return;
            }

            var spawnTransform = chestSpawnPoint != null ? chestSpawnPoint : transform;
            var chest = Instantiate(chestPrefab, spawnTransform.position, Quaternion.identity, transform);
            ChestSpawned?.Invoke(this, chest);
        }
    }
}
