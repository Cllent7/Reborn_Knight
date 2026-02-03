using System;
using UnityEngine;

namespace RebornKnight.Demo2D
{
    // 宝箱：按 E 交互，固定补弹 +6，并触发 Opened 事件
    // 使用说明：
    // 1) 挂在 Chest 物体上（Collider2D 需设为 Trigger）。
    // 2) 玩家身上必须有 AmmoSystem 才能开箱。
    public class Chest : MonoBehaviour
    {
        // 固定弹药奖励
        [SerializeField] private int ammoReward = 6;
        // 交互按键
        [SerializeField] private KeyCode openKey = KeyCode.E;

        private bool opened;

        public event Action<Chest> Opened;

        private void OnTriggerStay2D(Collider2D other)
        {
            // 已开启则不再处理
            if (opened)
            {
                return;
            }

            // 玩家身上挂 AmmoSystem 才允许开箱
            if (!other.TryGetComponent(out AmmoSystem ammoSystem))
            {
                return;
            }

            // 按下 E 进行开箱
            if (Input.GetKeyDown(openKey))
            {
                opened = true;
                ammoSystem.AddAmmo(ammoReward);
                // 触发打开事件（GameFlow 用于进入下一房间）
                Opened?.Invoke(this);
            }
        }
    }
}
