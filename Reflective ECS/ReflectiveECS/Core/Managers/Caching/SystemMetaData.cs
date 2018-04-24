using System;
using System.Reflection;
using ReflectiveECS.Optimization.FastInvoke;

namespace ReflectiveECS.Core.Managers.Caching
{
    internal class SystemMetaData
    {
        public readonly FastInvokeHandler FastInvokeHandler;
        public readonly Type[] ParameterTypes;
        public readonly Type[] ComponentParameterTypes;

        public SystemMetaData(FastInvokeHandler fastInvokeHandler, Type[] parameterTypes, Type[] componentParameterTypes)
        {
            FastInvokeHandler = fastInvokeHandler;
            ParameterTypes = parameterTypes;
            ComponentParameterTypes = componentParameterTypes;
        }
    }
}
