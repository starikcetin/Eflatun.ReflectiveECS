using System.Collections.Generic;

namespace ReflectiveECS
{
    public class EntitiesManager
    {
        private List<Entity> _entities;

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
