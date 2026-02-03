using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class Chest : MonoBehaviour
    {
        [SerializeField] private int ammoReward = 6;
        [SerializeField] private KeyCode openKey = KeyCode.E;

        private bool opened;

        public event Action<Chest> Opened;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (opened)
            {
                return;
            }

            if (!other.TryGetComponent(out AmmoSystem ammoSystem))
            {
                return;
            }

            if (Input.GetKeyDown(openKey))
            {
                opened = true;
                ammoSystem.AddAmmo(ammoReward);
                Opened?.Invoke(this);
            }
        }
    }
}
