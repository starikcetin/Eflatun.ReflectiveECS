using ReflectiveECS.Core;
using ReflectiveECS.Core.ECS;

namespace ReflectiveECS.Development.ECS
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
