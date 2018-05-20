using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eflatun.ReflectiveECS.Core.ECS;

namespace Eflatun.ReflectiveECS.Core.Managers
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

        public void FillMatchAll(ref List<Entity> fillList, params Type[] componentTypes)
        {
            for (var i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[i];

                if (entity.HasAll(componentTypes))
                {
                    fillList.Add(entity);
                }
            }
        }
    }
}
