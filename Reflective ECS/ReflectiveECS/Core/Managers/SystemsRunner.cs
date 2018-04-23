using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReflectiveECS.Core.ECS;

namespace ReflectiveECS.Core.Managers
{
    public class SystemsRunner
    {
        private readonly SystemsDatabase _systemsDatabase;
        private readonly EntitiesDatabase _entitiesDatabase;

        public SystemsRunner(SystemsDatabase systemsDatabase, EntitiesDatabase entitiesDatabase)
        {
            _systemsDatabase = systemsDatabase;
            _entitiesDatabase = entitiesDatabase;
        }

        public void RunAll()
        {
            foreach (var system in _systemsDatabase.RegisteredSystems)
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
            var matchedEntities = _entitiesDatabase.GetMatchAll(parameterTypes);

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
