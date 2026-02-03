using System.Collections.Generic;
using UnityEngine;

namespace RebornKnight.Core
{
    public class GameFlow : MonoBehaviour
    {
        [SerializeField] private List<RebornKnight.World.RoomController> rooms = new List<RebornKnight.World.RoomController>();

        private int currentRoomIndex;

        private void Awake()
        {
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

        private void HandleRoomCleared(RebornKnight.World.RoomController room)
        {
            // Hook for room clear feedback (UI, doors, etc.)
        }

        private void HandleChestSpawned(RebornKnight.World.RoomController room, RebornKnight.World.Chest chest)
        {
            if (chest != null)
            {
                chest.Opened += HandleChestOpened;
            }
        }

        private void HandleChestOpened(RebornKnight.World.Chest chest)
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
    }
}
