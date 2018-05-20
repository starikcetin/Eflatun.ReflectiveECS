using Eflatun.ReflectiveECS.Core;
using Eflatun.ReflectiveECS.Core.ECS;

namespace Eflatun.ReflectiveECS.Development.ECS
{
    public class PosAndRotSys : ISystem
    {
        [Execute]
        public void Execute(PosComp posComp, RotComp rotComp)
        {
            posComp.X++;
            posComp.Y++;
            rotComp.Angle++;
        }
    }
}
