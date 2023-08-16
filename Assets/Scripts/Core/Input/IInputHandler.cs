using System;

namespace Game.Core
{
    public interface IInputHandler
    {
        public bool Right { get; }
        public bool Left { get; }
        public bool Up { get; }
        public bool Down { get; }
    }
}
