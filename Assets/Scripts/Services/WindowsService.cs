using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Game.UI;
using PlayerInput = Game.Input.PlayerInput;

namespace Game.Services
{
    public class WindowsService : MonoBehaviour
    {
        [SerializeField]
        private List<Window> windows;
        [SerializeField]
        private Transform pivot;

        private List<Window> activeWindows = new();

        public T Open<T>(object data = null) where T : Window
        {
            var windowPrefab = windows.FirstOrDefault(w => w is T);
            if (windowPrefab != null)
            {
                var instance = (T)Instantiate(windowPrefab, pivot, false);

                var rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.offsetMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                instance.Constract(this, DiContainerService.Instance.Resolve<PlayerInput>());
                instance.Init(data);
                activeWindows.Add(instance);
                return instance;
            }

            return null;
        }

        public bool IsActive(Window window)
        {
            return activeWindows.Last() == window;
        }

        public void Close(Window window)
        {
            activeWindows.Remove(window);
            window.OnClose();
            Destroy(window.gameObject);
        }
    }
}