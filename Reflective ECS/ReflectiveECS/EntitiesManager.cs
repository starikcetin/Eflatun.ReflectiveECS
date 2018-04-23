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
    }
}
