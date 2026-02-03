using UnityEngine;
using UnityEngine.UI;

namespace RebornKnight.UI
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private RebornKnight.Player.PlayerController player;
        [SerializeField] private Text ammoText;

        private void OnEnable()
        {
            if (player != null)
            {
                player.AmmoChanged += UpdateAmmoText;
            }
        }

        private void Start()
        {
            if (player != null)
            {
                UpdateAmmoText(player.Ammo, player.MaxAmmo);
            }
        }

        private void OnDisable()
        {
            if (player != null)
            {
                player.AmmoChanged -= UpdateAmmoText;
            }
        }

        private void UpdateAmmoText(int ammo, int maxAmmo)
        {
            if (ammoText != null)
            {
                ammoText.text = $"Ammo: {ammo}/{maxAmmo}";
            }
        }
    }
}
