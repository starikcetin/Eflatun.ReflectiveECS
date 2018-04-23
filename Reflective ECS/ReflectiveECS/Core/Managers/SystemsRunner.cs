using System;
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
            var executeMethod = GetExecuteMethod(system);
            var shouldGetEntityItself = GetShouldGetEntityItself(executeMethod);
            var componentParameterTypes = GetTypesOfComponentParameters(executeMethod, shouldGetEntityItself);
            var matchedEntities = GetMatchingEntities(componentParameterTypes);

            foreach (var entity in matchedEntities)
            {
                var parameters = PrepareParameters(componentParameterTypes, entity, shouldGetEntityItself).ToArray();
                executeMethod.Invoke(system, parameters);
            }
        }

        private MethodInfo GetExecuteMethod(ISystem system)
        {
            // execute method is marked with [Execute] attribute
            var systemType = system.GetType();
            return systemType.GetMethods()
                .Single(m => m.GetCustomAttributes(typeof(ExecuteAttribute), false).Length > 0);
        }

        private bool GetShouldGetEntityItself(MethodInfo executeMethod)
        {
            var executeAttribute = (ExecuteAttribute) executeMethod.GetCustomAttribute(typeof(ExecuteAttribute));
            return executeAttribute.GetEntityItself;
        }

        private Type[] GetTypesOfComponentParameters(MethodInfo executeMethod, bool skipFirstParameter)
        {
            var parameters = executeMethod.GetParameters();
            return (skipFirstParameter ? parameters.Skip(1): parameters)
                .Select(pi => pi.ParameterType).ToArray();
        }

        private IEnumerable<Entity> GetMatchingEntities(Type[] componentTypes)
        {
            return _entitiesDatabase.GetMatchAll(componentTypes);
        }

        private IEnumerable<object> PrepareParameters(Type[] componentTypes, Entity entity,
            bool shouldGetEntityItself)
        {
            if (shouldGetEntityItself) yield return entity;

            foreach (var componentType in componentTypes)
            {
                yield return entity.Get(componentType);
            }
        }
    }
}
