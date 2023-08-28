using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class EntityActor : MonoBehaviour
    {
        public IntPoint EntityPosition => entity.Position;
        [SerializeField] private GameObject DestroyFX;
        [SerializeField] private bool hpAffectToScale = true;
        [SerializeField] private bool showUI = true;
        [SerializeField] private EntityUI entityUIPrefab;
        [SerializeField] private VisualResolver visualResolver; // TODO: inject via Zenject

        private Entity entity;
        private EntityUI entityUI;

        public virtual void Init(Entity entity)
        {
            this.entity = entity;
            transform.localPosition = VisualConfig.ToLevelLocal(entity.Position);

            entity.AbilityApplied += OnAbilityApplied;
            entity.StatsChanged += OnStatsChanged;

            if (showUI)
            {
                entityUI = Instantiate(entityUIPrefab, transform.position, entityUIPrefab.transform.rotation);
            }

            RefreshStats();
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
            RefreshStats();
            if (entity.IsDestroyed)
            {
                Destroy(gameObject);
                if (DestroyFX != null)
                {
                    Instantiate(DestroyFX, transform.position, transform.rotation);
                }
            }
        }

        protected virtual void RefreshStats()
        {
            RefreshScale();

            if (entityUI != null)
            {
                entityUI.RefreshStats(entity.Stats);
            }
        }

        protected void RefreshScale()
        {
            if (hpAffectToScale)
            {
                var hpFullness = entity.Stats.MaxHP > 0 ? (float)entity.Stats.HP / (float)entity.Stats.MaxHP : 0f;
                transform.localScale = Vector3.one * (0.5f + hpFullness * 0.5f);
            }
        }

        protected virtual void OnDestroy()
        {
            entity.AbilityApplied -= OnAbilityApplied;
            entity.StatsChanged -= OnStatsChanged;

            if (entityUI != null)
            {
                Destroy(entityUI.gameObject);
            }
        }


        private void Update()
        {
            if (entityUI != null)
            {
                entityUI.RefreshPosition(transform.position);
            }
        }
    }
}
