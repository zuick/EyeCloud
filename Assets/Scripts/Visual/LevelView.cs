using Game.Data;
using Game.Core;
using UnityEngine;

namespace Game.Visual
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private GameObject mapCellPrefab;

        public void Init(LevelData levelData)
        {
            var scale = VisualConfig.Instance.GridScale;
            var excents = new Vector2(levelData.MapSize.X * scale / 2f, levelData.MapSize.Y * scale / 2f);

            for (var i = 0; i < levelData.MapSize.Y; i++)
            {
                for (var j = 0; j < levelData.MapSize.X; j++)
                {
                    if (levelData.Map[i, j] == MapCellType.Block)
                        continue;

                    var position = new Vector3(
                        j * VisualConfig.Instance.GridScale - excents.x,
                        transform.position.y,
                        i * VisualConfig.Instance.GridScale - excents.y
                    );

                    var instance = Instantiate(mapCellPrefab, position, transform.rotation);
                    instance.transform.SetParent(transform, false);
                }
            }
        }
    }
}
