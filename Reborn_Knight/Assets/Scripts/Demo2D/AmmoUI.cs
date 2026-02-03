using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RebornKnight.Demo2D
{
    // 弹药 UI：显示 Ammo/MaxAmmo（支持 TMP 或 Unity UI Text）
    // 使用说明：
    // 1) 挂在 UI 文本对象上或空物体上。
    // 2) 绑定 ammoSystem + 其中一种文本组件即可。
    public class AmmoUI : MonoBehaviour
    {
        // 弹药系统引用
        [SerializeField] private AmmoSystem ammoSystem;
        // TextMeshPro UI 文本（可选）
        [SerializeField] private TMP_Text tmpText;
        // Unity UI Text（可选）
        [SerializeField] private Text uiText;

        private void OnEnable()
        {
            // 订阅弹药变化事件
            if (ammoSystem != null)
            {
                ammoSystem.OnAmmoChanged += UpdateAmmoText;
            }
        }

        private void Start()
        {
            // 初始化显示
            if (ammoSystem != null)
            {
                UpdateAmmoText(ammoSystem.CurrentAmmo, ammoSystem.MaxAmmo);
            }
        }

        private void OnDisable()
        {
            // 取消订阅，防止引用泄漏
            if (ammoSystem != null)
            {
                ammoSystem.OnAmmoChanged -= UpdateAmmoText;
            }
        }

        private void UpdateAmmoText(int ammo, int maxAmmo)
        {
            // 同时支持 TMP 与 Unity UI Text
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
