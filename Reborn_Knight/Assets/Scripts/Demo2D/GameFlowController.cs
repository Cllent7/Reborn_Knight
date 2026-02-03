using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RebornKnight.Demo2D
{
    // 游戏流程控制：房间顺序、宝箱开启后进下一房、玩家死亡重开
    // 使用说明：
    // 1) rooms 按顺序填入 Room1 -> Room2 -> Room3 -> BossRoom。
    // 2) player 绑定玩家对象，用于监听死亡事件。
    public class GameFlowController : MonoBehaviour
    {
        // 按顺序摆放房间（Room1 -> Room2 -> Room3 -> BossRoom）
        [SerializeField] private List<RoomController> rooms = new List<RoomController>();
        // 玩家引用（用于监听死亡事件）
        [SerializeField] private PlayerController2D player;

        private int currentRoomIndex;

        private void Awake()
        {
            // 监听玩家死亡，重置本局
            if (player != null)
            {
                player.Died += HandlePlayerDied;
            }

            // 依次注册房间事件，并只激活第一个房间
            for (var i = 0; i < rooms.Count; i++)
            {
                var room = rooms[i];
                if (room == null)
                {
                    continue;
                }

                room.RoomCleared += HandleRoomCleared;
                room.ChestSpawned += HandleChestSpawned;
                room.ActivateRoom(i == 0);
            }
        }

        private void OnDestroy()
        {
            if (player != null)
            {
                player.Died -= HandlePlayerDied;
            }

            foreach (var room in rooms)
            {
                if (room == null)
                {
                    continue;
                }

                room.RoomCleared -= HandleRoomCleared;
                room.ChestSpawned -= HandleChestSpawned;
            }
        }

        private void HandleRoomCleared(RoomController room)
        {
            // 房间清理完成时的扩展点（开门、提示等）
            // Hook for room clear feedback (doors, VFX, etc.)
        }

        private void HandleChestSpawned(RoomController room, Chest chest)
        {
            // 监听宝箱开启事件
            if (chest != null)
            {
                chest.Opened += HandleChestOpened;
            }
        }

        private void HandleChestOpened(Chest chest)
        {
            // 宝箱只处理一次
            if (chest != null)
            {
                chest.Opened -= HandleChestOpened;
            }

            AdvanceToNextRoom();
        }

        private void AdvanceToNextRoom()
        {
            // 关闭当前房间
            if (currentRoomIndex < rooms.Count)
            {
                rooms[currentRoomIndex].ActivateRoom(false);
            }

            // 切换到下一个房间
            currentRoomIndex++;

            if (currentRoomIndex >= rooms.Count)
            {
                return;
            }

            rooms[currentRoomIndex].ActivateRoom(true);
        }

        private void HandlePlayerDied()
        {
            // 直接重载当前场景，重开本局
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
