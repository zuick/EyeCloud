using UnityEngine;
using Game.Services;
using PlayerInput = Game.Input.PlayerInput;

namespace Game.UI
{
    public abstract class Window : MonoBehaviour
    {
        protected WindowsService windowsService;
        protected PlayerInput input;

        public bool IsActive => windowsService.IsActive(this);

        public void Constract(WindowsService windowsService, PlayerInput input)
        {
            this.windowsService = windowsService;
            this.input = input;
        }

        public virtual void Init(object data)
        {
        }

        public virtual void OnClose()
        {

        }

        public void CloseSelf()
        {
            windowsService.Close(this);
        }
    }
}