using System;
using System.Reflection;

namespace ReflectiveECS.Core.Managers.Caching
{
    internal class SystemMetaData
    {
        public readonly MethodInfo ExecuteMethodInfo;
        public readonly Type[] ParameterTypes;
        public readonly Type[] ComponentParameterTypes;

        public SystemMetaData(MethodInfo executeMethodInfo, Type[] parameterTypes, Type[] componentParameterTypes)
        {
            ExecuteMethodInfo = executeMethodInfo;
            ParameterTypes = parameterTypes;
            ComponentParameterTypes = componentParameterTypes;
        }
    }
}
