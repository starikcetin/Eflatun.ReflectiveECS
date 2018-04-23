using System;
using System.Collections.Generic;

namespace ReflectiveECS
{
    public class EntitiesManager
    {
        private readonly List<Entity> _entities;

        public EntitiesManager()
        {
            _entities = new List<Entity>();
        }

        public void Register(Entity entity)
        {
            _entities.Add(entity);
        }

        public void Unregister(Entity entity)
        {
            _entities.Remove(entity);
        }

        public IEnumerable<Entity> GetMatchAll(params Type[] componentTypes)
        {
            foreach (var entity in _entities)
            {
                if (entity.HasAll(componentTypes))
                {
                    yield return entity;
                }
            }
        }
    }
}
