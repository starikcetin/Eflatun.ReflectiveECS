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
            // get the execute method which is marked with [Execute] attribute
            var concrete = system.GetType();
            var executeMethod = concrete.GetMethods()
                .Single(m => m.GetCustomAttributes(typeof(ExecuteAttribute), false).Length > 0);

            // will the entity itself be passed?
            var executeAttribute = (ExecuteAttribute) executeMethod.GetCustomAttribute(typeof(ExecuteAttribute));
            var getEntityItself = executeAttribute.GetEntityItself;

            // get all parameters as components
            var parameters = executeMethod.GetParameters();
            var parameterTypes =
                (getEntityItself ? parameters.Skip(1) : parameters) // if the entity itself will be passed, skip the first parameter
                .Select(pi => pi.ParameterType).ToArray();

            // match parameter components
            var matchedEntities = _entitiesManager.GetMatchAll(parameterTypes);

            // execute for all matches
            foreach (var entity in matchedEntities)
            {
                // construct parameter array
                var parameterList = new List<object>();

                // if the entity itself will be passed, add it to the parameter array now
                if (getEntityItself) parameterList.Add(entity);

                foreach (var parameterType in parameterTypes)
                {
                    var componentInstance = entity.Get(parameterType);
                    parameterList.Add(componentInstance);
                }

                // invoke execute method of system
                executeMethod.Invoke(system, parameterList.ToArray());
            }
        }
    }
}
