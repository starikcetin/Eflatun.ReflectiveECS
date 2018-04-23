using System;

namespace ReflectiveECS
{
    public class SystemsRunner
    {
        private readonly SystemsManager _systemsManager;

        public SystemsRunner(SystemsManager systemsManager)
        {
            _systemsManager = systemsManager;
        }

        public void RunAll()
        {
            foreach (var system in _systemsManager.RegisteredSystems)
            {
                RunSystem(system);
            }
        }

        private void RunSystem(ISystem system)
        {
            throw new NotImplementedException();
        }
    }
}
