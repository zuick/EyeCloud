using UnityEditor;
using UnityEngine;
using Game.Core;
using System.Linq;
using System.Collections.Generic;

namespace Game.Data
{
    [CustomEditor(typeof(LevelData))]
    public class LevelDataEditor : Editor
    {
        private const int intWidth = 40;
        private const int buttonFontSize = 8;
        private int initialButtonFontSize;

        public override void OnInspectorGUI()
        {
            var levelData = (LevelData)target;
            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }

            GUILayout.BeginHorizontal();
            var width = IntField("Width: ", levelData.MapSize.X);
            var height = IntField("Height: ", levelData.MapSize.Y);
            GUILayout.EndHorizontal();

            if (levelData.Map == null || width != levelData.MapSize.X || height != levelData.MapSize.Y)
            {
                levelData.MapSize = new IntPoint(width, height);
                levelData.Map = new MapCellType[height, width];
            }

            DrawMap(levelData);
            DrawEntities(levelData);
        }

        private void DrawEntities(LevelData levelData)
        {
            if (GUILayout.Button("Add Entity"))
            {
                if (levelData.Entities == null)
                    levelData.Entities = new List<EnityPositionData>();

                levelData.Entities.Add(new EnityPositionData());
            }

            var removeIndex = -1;
            for (var i = 0; i < levelData.Entities.Count; i++)
            {
                GUILayout.BeginHorizontal();

                var row = levelData.Entities[i];
                row.entityData = (EntityData)EditorGUILayout.ObjectField(
                    row.entityData,
                    typeof(EntityData),
                    false
                );
                var x = IntField("X:", row.position.X);
                var y = IntField("Y:", row.position.Y);
                row.position = new IntPoint(x, y);
                if (GUILayout.Button("Remove"))
                {
                    removeIndex = i;
                    break;
                }
                GUILayout.EndHorizontal();
            }

            if (removeIndex >= 0)
            {
                levelData.Entities.RemoveAt(removeIndex);
            }
        }

        private int IntField(string label, int value)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(label.Length * 10));
            var result = EditorGUILayout.IntField(value, GUILayout.Width(intWidth));
            GUILayout.EndHorizontal();
            return result;
        }

        private void DrawMap(LevelData levelData)
        {
            var oldColor = GUI.backgroundColor;

            GUILayout.BeginVertical();
            for (var i = levelData.MapSize.Y - 1; i >= 0; i--)
            {
                GUILayout.BeginHorizontal();
                for (var j = 0; j < levelData.MapSize.X; j++)
                {
                    var cellType = levelData.Map[i, j];
                    GUI.backgroundColor = cellType == MapCellType.Block
                        ? Color.grey
                        : Color.white;

                    var entitesOnCell = levelData.Entities
                        .Where(e => e.entityData != null && e.position.Y == i && e.position.X == j)
                        .Select(e => e.entityData.name);

                    var caption = string.Join("\n", entitesOnCell);
                    initialButtonFontSize = GUI.skin.button.fontSize;
                    GUI.skin.button.fontSize = buttonFontSize;
                    if (GUILayout.Button(caption, GUILayout.Width(40), GUILayout.Height(40)))
                    {
                        levelData.Map[i, j] = cellType == MapCellType.Block ? MapCellType.Empty : MapCellType.Block;
                    }
                    GUI.skin.button.fontSize = initialButtonFontSize;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            GUI.backgroundColor = oldColor;
        }
    }
}
