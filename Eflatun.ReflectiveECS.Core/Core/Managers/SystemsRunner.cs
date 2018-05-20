using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Eflatun.ReflectiveECS.Core.Core.ECS;
using Eflatun.ReflectiveECS.Core.Core.Managers.Caching;
using Eflatun.ReflectiveECS.Core.Optimization.FastInvoke;

namespace Eflatun.ReflectiveECS.Core.Core.Managers
{
    public class SystemsRunner
    {
        private readonly SystemsDatabase _systemsDatabase;
        private readonly EntitiesDatabase _entitiesDatabase;

        private readonly Dictionary<ISystem, SystemMetaData> _systemMetaDatas = new Dictionary<ISystem, SystemMetaData>();

        private List<Entity> _macthFillList = new List<Entity>();

        public SystemsRunner(SystemsDatabase systemsDatabase, EntitiesDatabase entitiesDatabase)
        {
            _systemsDatabase = systemsDatabase;
            _entitiesDatabase = entitiesDatabase;
        }

        public void Cache(ISystem system)
        {
            var executeMethodInfo = GetExecuteMethod(system);
            var fastInvokeHandler = FastInvokerGenerator.GenerateFastInvoker(executeMethodInfo);
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
            FillMatchingEntities(metaData.ComponentParameterTypes);

            var parameterArray = new object[parametersTypes.Length];

            for (var i = 0; i < _macthFillList.Count; i++)
            {
                PrepareParameters(ref parameterArray, parametersTypes, _macthFillList[i]);
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

        private void FillMatchingEntities(Type[] componentTypes)
        {
            _macthFillList.Clear();
            _entitiesDatabase.FillMatchAll(ref _macthFillList, componentTypes);
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
