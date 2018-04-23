using System.Collections.Generic;

namespace ReflectiveECS
{
    public class Entity
    {
        private List<IComponent> _components;

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
