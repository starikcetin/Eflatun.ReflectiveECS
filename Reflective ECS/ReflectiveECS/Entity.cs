using System.Collections.Generic;

namespace ReflectiveECS
{
    public class Entity
    {
        private readonly List<IComponent> _components;

        public Entity()
        {
            _components = new List<IComponent>();
        }

        public void Register(IComponent component)
        {
            _components.Add(component);
        }

        public void Deregister(IComponent component)
        {
            _components.Remove(component);
        }
    }
}
