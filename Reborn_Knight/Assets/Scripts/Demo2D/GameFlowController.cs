using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RebornKnight.Demo2D
{
    public class GameFlowController : MonoBehaviour
    {
        [SerializeField] private List<RoomController> rooms = new List<RoomController>();
        [SerializeField] private PlayerController2D player;

        private int currentRoomIndex;

        private void Awake()
        {
            if (player != null)
            {
                player.Died += HandlePlayerDied;
            }

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
            // Hook for room clear feedback (doors, VFX, etc.)
        }

        private void HandleChestSpawned(RoomController room, Chest chest)
        {
            if (chest != null)
            {
                chest.Opened += HandleChestOpened;
            }
        }

        private void HandleChestOpened(Chest chest)
        {
            if (chest != null)
            {
                chest.Opened -= HandleChestOpened;
            }

            AdvanceToNextRoom();
        }

        private void AdvanceToNextRoom()
        {
            if (currentRoomIndex < rooms.Count)
            {
                rooms[currentRoomIndex].ActivateRoom(false);
            }

            currentRoomIndex++;

            if (currentRoomIndex >= rooms.Count)
            {
                return;
            }

            rooms[currentRoomIndex].ActivateRoom(true);
        }

        private void HandlePlayerDied()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
    }
}
