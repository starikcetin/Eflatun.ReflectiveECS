﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eflatun.ReflectiveECS.Core.ECS;

namespace Eflatun.ReflectiveECS.Core.Managers
{
    public class SystemsDatabase
    {
        private readonly List<ISystem> _registeredSystems = new List<ISystem>();
        public ReadOnlyCollection<ISystem> RegisteredSystems => _registeredSystems.AsReadOnly();

        public void Register(ISystem system)
        {
            _registeredSystems.Add(system);
        }

        public void Unregister(ISystem system)
        {
            _registeredSystems.Remove(system);
        }
    }
}
