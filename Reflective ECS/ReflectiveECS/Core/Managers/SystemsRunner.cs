using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReflectiveECS.Core.ECS;
using ReflectiveECS.Core.Managers.Caching;
using ReflectiveECS.Optimization.FastInvoke;

namespace ReflectiveECS.Core.Managers
{
    public class SystemsRunner
    {
        private readonly SystemsDatabase _systemsDatabase;
        private readonly EntitiesDatabase _entitiesDatabase;

        private readonly Dictionary<ISystem, SystemMetaData> _systemMetaDatas = new Dictionary<ISystem, SystemMetaData>();

        public SystemsRunner(SystemsDatabase systemsDatabase, EntitiesDatabase entitiesDatabase)
        {
            _systemsDatabase = systemsDatabase;
            _entitiesDatabase = entitiesDatabase;
        }

        public void Cache(ISystem system)
        {
            var executeMethodInfo = GetExecuteMethod(system);
            var fastInvokeHandler = FastInvoker.GetMethodInvoker(executeMethodInfo);
            var parameterTypes = GetMethodParameterTypes(executeMethodInfo).ToArray();
            var componentParameterTypes = FilterOutEntity(parameterTypes).ToArray();

            var newSystemMeta = new SystemMetaData(fastInvokeHandler, parameterTypes, componentParameterTypes);

            _systemMetaDatas.Add(system, newSystemMeta);
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
            var metaData = _systemMetaDatas[system];

            var fastInvokeHandler = metaData.FastInvokeHandler;
            var parametersTypes = metaData.ParameterTypes;
            var matchedEntities = GetMatchingEntities(metaData.ComponentParameterTypes);

            var parameterArray = new object[parametersTypes.Length];

            foreach (var entity in matchedEntities)
            {
                PrepareParameters(ref parameterArray, parametersTypes, entity);
                fastInvokeHandler.Invoke(system, parameterArray);
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

        private void PrepareParameters(ref object[] fillArray, Type[] parameterTypes, Entity entity)
        {
            for (var i = 0; i < fillArray.Length; i++)
            {
                var parameterType = parameterTypes[i];

                if (parameterType.IsAssignableFrom(typeof(Entity)))
                {
                    fillArray[i] = entity;
                }
                else
                {
                    fillArray[i] = entity.Get(parameterType);
                }
            }
        }
    }
}
