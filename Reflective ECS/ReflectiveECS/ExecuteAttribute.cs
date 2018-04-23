using System;

namespace ReflectiveECS
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteAttribute : Attribute
    {
        public bool GetEntityItself;

        public ExecuteAttribute(bool getEntityItself = false)
        {
            GetEntityItself = getEntityItself;
        }
    }
}
