﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ReflectiveECS.Core.ECS
{
    public class Entity
    {
        public int Id { get; }

        private readonly List<IComponent> _components;
        public ReadOnlyCollection<IComponent> Components => _components.AsReadOnly();

        public Entity(int id)
        {
            Id = id;
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

        public T Get<T>() where T : IComponent
        {
            return _components.OfType<T>().Single();
        }

        public IComponent Get(Type type)
        {
            return _components.Single(c => c.GetType() == type);
        }

        public bool Has<T>() where T : IComponent
        {
            return _components.OfType<T>().Any();
        }

        public bool Has(Type type)
        {
            return _components.Exists(c => c.GetType() == type);
        }

        public bool HasAll(IEnumerable<Type> types)
        {
            return types.All(Has);
        }
    }
}