using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RebornKnight.Demo2D
{
    public class AmmoUI : MonoBehaviour
    {
        [SerializeField] private AmmoSystem ammoSystem;
        [SerializeField] private TMP_Text tmpText;
        [SerializeField] private Text uiText;

        private void OnEnable()
        {
            if (ammoSystem != null)
            {
                ammoSystem.OnAmmoChanged += UpdateAmmoText;
            }
        }

        private void Start()
        {
            if (ammoSystem != null)
            {
                UpdateAmmoText(ammoSystem.CurrentAmmo, ammoSystem.MaxAmmo);
            }
        }

        private void OnDisable()
        {
            if (ammoSystem != null)
            {
                ammoSystem.OnAmmoChanged -= UpdateAmmoText;
            }
        }

        private void UpdateAmmoText(int ammo, int maxAmmo)
        {
            var textValue = $"Ammo: {ammo}/{maxAmmo}";
            if (tmpText != null)
            {
                tmpText.text = textValue;
            }

            if (uiText != null)
            {
                uiText.text = textValue;
            }
        }
    }
}
