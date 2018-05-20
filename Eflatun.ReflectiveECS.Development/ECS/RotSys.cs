using Eflatun.ReflectiveECS.Core.Core;
using Eflatun.ReflectiveECS.Core.Core.ECS;

namespace Eflatun.ReflectiveECS.Development.ECS
{
    public class RotSys : ISystem
    {
        [Execute]
        public void Execute(RotComp rotComp)
        {
            rotComp.Angle += 10;
        }
    }
}
