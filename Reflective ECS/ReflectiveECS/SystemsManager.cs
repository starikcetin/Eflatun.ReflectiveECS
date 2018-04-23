using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReflectiveECS
{
    public class SystemsManager
    {
        private readonly List<ISystem> _registeredSystems;
        public ReadOnlyCollection<ISystem> RegisteredSystems => _registeredSystems.AsReadOnly();

        public SystemsManager()
        {
            _registeredSystems = new List<ISystem>();
        }

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
