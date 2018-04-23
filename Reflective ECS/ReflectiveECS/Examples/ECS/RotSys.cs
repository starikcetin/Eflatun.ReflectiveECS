namespace ReflectiveECS.Examples.ECS
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
