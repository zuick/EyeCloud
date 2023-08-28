using System;
using UnityEngine;
using Game.UI;
using Game.Core;

namespace Game.Visual
{
    public class EntityUI : MonoBehaviour
    {
        [SerializeField] private ProgressBar hpBar;
        [SerializeField] private Vector3 offset;

        public void RefreshStats(EntityStats stats)
        {
            hpBar.Refresh(stats.HP, stats.MaxHP);
        }

        public void RefreshPosition(Vector3 entityPosition)
        {
            transform.position = entityPosition + offset;
        }
    }
}
