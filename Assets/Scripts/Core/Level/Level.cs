using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Level
    {
        public List<Entity> Entities = new();

        public void Add(Entity entity)
        {
            Entities.Add(entity);
        }

        public Entity GetAt(IntPoint position)
        {
            return Entities.FirstOrDefault(e => e.Position.Equals(position));
        }
    }
}

