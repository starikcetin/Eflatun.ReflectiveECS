using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReflectiveECS.Core.ECS
{
    public class Entity
    {
        public int Id { get; }

        private readonly List<Type> _componentTypes = new List<Type>();

        private readonly List<IComponent> _components = new List<IComponent>();
        public ReadOnlyCollection<IComponent> Components => _components.AsReadOnly();

        public Entity(int id)
        {
            Id = id;
        }

        public void Register(IComponent component)
        {
            _components.Add(component);
            _componentTypes.Add(component.GetType());
        }

        public void Deregister(IComponent component)
        {
            _components.Remove(component);
            _componentTypes.Remove(component.GetType());
        }

        public T Get<T>() where T : IComponent
        {
            return (T) _components[_componentTypes.IndexOf(typeof(T))];
        }

        public IComponent Get(Type type)
        {
            return _components[_componentTypes.IndexOf(type)];
        }

        public bool Has<T>() where T : IComponent
        {
            return _componentTypes.Contains(typeof(T));
        }

        public bool Has(Type type)
        {
            return _componentTypes.Contains(type);
        }

        public bool HasAll(IEnumerable<Type> types)
        {
            return types.All(Has);
        }

        [ContractInvariantMethod]
        protected void ListsInvariant()
        {
            Contract.Invariant(_componentTypes.Count == _components.Count);
        }
    }
}
