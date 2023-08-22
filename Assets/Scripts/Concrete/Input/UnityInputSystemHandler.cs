using System;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Game.Core
{
    public class UnityInputSystemHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField] private InputActionReference leftAction;
        [SerializeField] private InputActionReference rightAction;
        [SerializeField] private InputActionReference upAction;
        [SerializeField] private InputActionReference downAction;
        [SerializeField] private InputActionReference passTurnAction;

        public bool Right => rightAction.action.IsPressed();
        public bool Left => leftAction.action.IsPressed();
        public bool Up => upAction.action.IsPressed();
        public bool Down => downAction.action.IsPressed();
        public bool PassTurn => passTurnAction.action.IsPressed();
    }
}
