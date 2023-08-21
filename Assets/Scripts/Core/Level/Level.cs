using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace Game.Core
{
    public class Level
    {
        public SortedList<int, Entity> Entities = new();

        public void Add(Entity entity)
        {
            if (!Entities.ContainsKey(entity.Id))
            {
                Entities.Add(entity.Id, entity);
            }
            else
            {
                Debug.LogError($"Level already has entity with id: {entity.Id}");
            }
        }

        public void Remove(int id)
        {
            Entities.Remove(id);
        }

        public void RemoveDestroyed()
        {
            Entities.Values
                .Where(e => e.IsDestroyed)
                .Select(e => e.Id)
                .ToArray()
                .ForEach(Remove);
        }

        public Entity GetNext(Entity source)
        {
            var first = Entities.First().Value;

            if (source == null)
            {
                return first;
            }

            foreach (var pair in Entities)
            {
                if (!pair.Value.IsDestroyed && pair.Key > source.Id)
                {
                    return pair.Value;
                }
            }

            return first;
        }

        public Entity GetAt(IntPoint position)
        {
            return Entities.Values.FirstOrDefault(e => e.Position.Equals(position));
        }
    }
}

