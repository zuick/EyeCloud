using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class GameHUDWindow : Window
    {
        [SerializeField] private TMP_Text levelDescription;

        public void SetLevelDescription(string description)
        {
            levelDescription.text = description;
        }
    }
}