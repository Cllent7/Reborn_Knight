using System;
using System.Collections.Generic;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    // 房间控制：统计敌人数量，清房后生成宝箱
    // 使用说明：
    // 1) 挂在 Room 根物体上。
    // 2) room 下的 EnemySimpleAI 会自动被统计。
    // 3) 配置 chestPrefab 与 chestSpawnPoint。
    public class RoomController : MonoBehaviour
    {
        // 清房后生成的宝箱预制体
        [SerializeField] private Chest chestPrefab;
        // 宝箱生成点
        [SerializeField] private Transform chestSpawnPoint;

        private readonly List<EnemySimpleAI> enemies = new List<EnemySimpleAI>();
        private bool cleared;

        // 房间清空事件
        public event Action<RoomController> RoomCleared;
        // 生成宝箱事件
        public event Action<RoomController, Chest> ChestSpawned;

        private void Awake()
        {
            // 统计房间内已有敌人并监听死亡
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
            // 动态生成敌人可调用此方法注册
            if (enemy == null || enemies.Contains(enemy))
            {
                return;
            }

            enemies.Add(enemy);
            enemy.Died += HandleEnemyDied;
        }

        public void ActivateRoom(bool active)
        {
            // 简单控制房间整体激活/停用
            gameObject.SetActive(active);
        }

        private void HandleEnemyDied(EnemySimpleAI enemy)
        {
            enemy.Died -= HandleEnemyDied;
            enemies.Remove(enemy);

            // 敌人清空 -> 房间完成 -> 生成宝箱
            if (!cleared && enemies.Count == 0)
            {
                cleared = true;
                RoomCleared?.Invoke(this);
                SpawnChest();
            }
        }

        private void SpawnChest()
        {
            // 未配置宝箱则不生成
            if (chestPrefab == null)
            {
                return;
            }

            // 使用指定生成点，若无则用房间中心
            var spawnTransform = chestSpawnPoint != null ? chestSpawnPoint : transform;
            var chest = Instantiate(chestPrefab, spawnTransform.position, Quaternion.identity, transform);
            ChestSpawned?.Invoke(this, chest);
        }
    }
}
