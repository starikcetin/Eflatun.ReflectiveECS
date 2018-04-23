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

        private readonly Dictionary<ISystem, MethodInfo> _executeMethodsCache = new Dictionary<ISystem, MethodInfo>();

        public SystemsRunner(SystemsDatabase systemsDatabase, EntitiesDatabase entitiesDatabase)
        {
            _systemsDatabase = systemsDatabase;
            _entitiesDatabase = entitiesDatabase;
        }

        public void Cache(ISystem system)
        {
            var executeMethodInfo = GetExecuteMethod(system);
            _executeMethodsCache.Add(system, executeMethodInfo);
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
            var executeMethod = _executeMethodsCache[system];
            var parametersTypes = GetMethodParameterTypes(executeMethod).ToArray();
            var matchedEntities = GetMatchingEntities(FilterOutEntity(parametersTypes).ToArray());

            foreach (var entity in matchedEntities)
            {
                var parameters = PrepareParameters(parametersTypes, entity).ToArray();
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

        private IEnumerable<Type> GetMethodParameterTypes(MethodInfo executeMethod)
        {
            return executeMethod.GetParameters().Select(pi => pi.ParameterType);
        }

        private IEnumerable<Type> FilterOutEntity(IEnumerable<Type> types)
        {
            return types.Where(t => !t.IsAssignableFrom(typeof(Entity)));
        }

        private IEnumerable<Entity> GetMatchingEntities(Type[] componentTypes)
        {
            return _entitiesDatabase.GetMatchAll(componentTypes);
        }

        private IEnumerable<object> PrepareParameters(IEnumerable<Type> parameterTypes, Entity entity)
        {
            foreach (var parameterType in parameterTypes)
            {
                if (parameterType.IsAssignableFrom(typeof(Entity)))
                {
                    yield return entity;
                }
                else
                {
                    yield return entity.Get(parameterType);
                }
            }
        }
    }
}
