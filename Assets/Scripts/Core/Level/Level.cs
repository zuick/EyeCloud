using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace Game.Core
{
    public class Level
    {
        public SortedList<int, Entity> Entities = new(); // TODO: private?
        private MapCellType[,] map;

        public Level(MapCellType[,] map)
        {
            this.map = map;
        }

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

        public bool HasOppositeEntities(int fraction)
        {
            return Entities.Values.Any(e => e.FractionId != fraction);
        }

        public bool HasEntities(int fraction)
        {
            return Entities.Values.Any(e => e.FractionId == fraction);
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
            return Entities.Values.FirstOrDefault(e => e.Position.Equals(position) && !e.Stats.IsInvisible);
        }

        public bool IsFree(IntPoint position)
        {
            if (position.X < 0 || position.X >= map.GetLength(0))
            {
                return false;
            }
            else if (position.Y < 0 || position.Y >= map.GetLength(1))
            {
                return false;
            }

            return map[position.Y, position.X] == MapCellType.Empty;
        }

        public IntPoint[] GetFreePositionsAround(IntPoint position)
        {
            var positionsAround = new IntPoint[]
            {
                position + new IntPoint(0, 1),
                position + new IntPoint(0, -1),
                position + new IntPoint(1, 0),
                position + new IntPoint(-1, 0),
            };

            return positionsAround.Where(IsFree).ToArray();
        }

        public IntPoint GetRandomPosition()
        {
            IntPoint candidate;
            do
            {
                var rx = UnityEngine.Random.Range(0, map.GetLength(0));
                var ry = UnityEngine.Random.Range(0, map.GetLength(1));
                candidate = new IntPoint(rx, ry);
            } while (!IsFree(candidate));

            return candidate;
        }

        public Entity Get(Func<Entity, bool> predicate)
        {
            return Entities.Values.Where(e => !e.Stats.IsInvisible).FirstOrDefault(predicate);
        }
    }
}

