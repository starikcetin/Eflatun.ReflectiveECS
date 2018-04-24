using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ReflectiveECS.Core.ECS;

namespace ReflectiveECS.Core.Managers
{
    public class EntitiesDatabase
    {
        private readonly List<Entity> _entities = new List<Entity>();
        public ReadOnlyCollection<Entity> Entities => _entities.AsReadOnly();

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
