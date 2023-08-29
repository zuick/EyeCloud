using Game.Data;
using Game.Core;

namespace Game.Messages
{
    public class SpawnEntityMessage
    {
        public IntPoint Position;
        public EntityData EntityData;

        public SpawnEntityMessage(IntPoint position, EntityData entityData)
        {
            Position = position;
            EntityData = entityData;
        }
    }
}