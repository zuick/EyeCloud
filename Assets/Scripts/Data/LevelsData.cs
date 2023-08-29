using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "Data/LevelsData", order = 1)]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] private LevelData[] levels;

        public bool TryGetNext(LevelData current, out LevelData next)
        {
            next = null;
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] == current && i < levels.Length - 1)
                {
                    next = levels[i + 1];
                    return true;
                }
            }

            return false;
        }

        public LevelData First => levels.Length > 0 ? levels[0] : null;
    }
}
