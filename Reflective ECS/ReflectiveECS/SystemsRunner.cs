using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflectiveECS
{
    public class SystemsRunner
    {
        private readonly SystemsManager _systemsManager;
        private readonly EntitiesManager _entitiesManager;

        public SystemsRunner(SystemsManager systemsManager, EntitiesManager entitiesManager)
        {
            _systemsManager = systemsManager;
            _entitiesManager = entitiesManager;
        }

        public void RunAll()
        {
            foreach (var system in _systemsManager.RegisteredSystems)
            {
                Run(system);
            }
        }

        private void Run(ISystem system)
        {
            throw new NotImplementedException();
        }
    }
}
