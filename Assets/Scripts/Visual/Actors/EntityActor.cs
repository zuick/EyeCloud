﻿using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class EntityActor : MonoBehaviour
    {
        public IntPoint EntityPosition => entity.Position;
        [SerializeField] private GameObject DestroyFX;
        [SerializeField] private VisualResolver visualResolver; // TODO: inject via Zenject
        private Entity entity;

        public virtual void Init(Entity entity)
        {
            this.entity = entity;
            transform.localPosition = VisualConfig.ToLevelLocal(entity.Position);

            entity.AbilityApplied += OnAbilityApplied;
            entity.StatsChanged += OnStatsChanged;

            RefreshScale();
        }

        protected virtual void OnAbilityApplied(AbilityApplyData applyData)
        {
            if (applyData.Ability is Ability abilitySO)
            {
                if (visualResolver.TryGet(abilitySO, out var visual))
                {
                    visual.Perform(this, applyData);
                }
            }
        }

        protected virtual void OnStatsChanged(EntityStats oldStats, EntityStats newStats)
        {
            RefreshScale();
            if (entity.IsDestroyed)
            {
                Destroy(gameObject);
                if (DestroyFX != null)
                {
                    Instantiate(DestroyFX, transform.position, transform.rotation);
                }
            }
        }

        protected void RefreshScale()
        {
            var hpFullness = entity.Stats.MaxHP > 0 ? (float)entity.Stats.HP / (float)entity.Stats.MaxHP : 0f;
            transform.localScale = Vector3.one * (0.5f + hpFullness * 0.5f);
        }

        protected virtual void OnDestroy()
        {
            entity.AbilityApplied -= OnAbilityApplied;
            entity.StatsChanged -= OnStatsChanged;
        }
    }
}
