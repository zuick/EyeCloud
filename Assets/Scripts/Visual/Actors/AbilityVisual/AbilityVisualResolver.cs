using System;
using UnityEngine;
using Game.Core;

namespace Game.Visual
{
    public class AbilityVisualResolver : MonoBehaviour
    {
        [SerializeField] private AbilityVisual moveTo;
        [SerializeField] private AbilityVisual pass;

        public static AbilityVisualResolver Instance;

        private void Awake()
        {
            Instance = this;
        }

        public bool TryGet(IAbility ability, out AbilityVisual visual)
        {
            visual = null;

            if (ability is MoveTo)
            {
                visual = moveTo;
                return true;
            }
            else if (ability is PassTurn)
            {
                visual = pass;
                return true;
            }

            return false;
        }
    }
}
