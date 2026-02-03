using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    public class AmmoSystem : MonoBehaviour
    {
        // 最大弹药上限
        [SerializeField] private int maxAmmo = 12;
        // 当前弹药（初始可在 Inspector 配置）
        [SerializeField] private int currentAmmo;

        // 弹药变化事件：当前弹药, 最大弹药
        public event Action<int, int> OnAmmoChanged;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => maxAmmo;

        private void Awake()
        {
            // 限制初始弹药在合法范围
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
        }

        private void Start()
        {
            // 如果未配置初始弹药，则默认满弹
            if (currentAmmo == 0)
            {
                currentAmmo = maxAmmo;
            }

            NotifyAmmoChanged();
        }

        public bool CanConsume(int amount)
        {
            // 是否有足够弹药进行消耗
            return currentAmmo >= amount;
        }

        public void ConsumeAmmo(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            currentAmmo = Mathf.Max(0, currentAmmo - amount);
            NotifyAmmoChanged();
        }

        public void AddAmmo(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
            NotifyAmmoChanged();
        }

        private void NotifyAmmoChanged()
        {
            // 通知 UI 或其他系统更新显示
            OnAmmoChanged?.Invoke(currentAmmo, maxAmmo);
        }
    }
}
