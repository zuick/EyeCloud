using Game.Data;
using Game.Core;
using UnityEngine;

namespace Game.Visual
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private GameObject mapCellPrefab;
        [SerializeField] private Transform mapCellsPivot;

        public void Init(LevelData levelData)
        {
            var scale = VisualConfig.Instance.GridScale;

            for (var i = 0; i < levelData.MapSize.Y; i++)
            {
                for (var j = 0; j < levelData.MapSize.X; j++)
                {
                    if (levelData.Map[i, j] == MapCellType.Block)
                        continue;

                    var position = new Vector3(j * scale, transform.position.y, i * scale);

                    var instance = Instantiate(mapCellPrefab, position, transform.rotation);
                    instance.transform.SetParent(mapCellsPivot, false);
                }
            }

            var gap = scale - 1f;
            transform.position -= new Vector3(
                (levelData.MapSize.X + gap * (levelData.MapSize.X - 1)) * 0.5f - 0.5f * scale,
                0f,
                (levelData.MapSize.Y + gap * (levelData.MapSize.Y - 1)) * 0.5f - 0.5f * scale
            );
        }
    }
}
