using ReflectiveECS.Examples.ECS;

namespace ReflectiveECS
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var systemsManager = new SystemsManager();
            var entitiesManager = new EntitiesManager();
            var systemsRunner = new SystemsRunner(systemsManager);


            //
            // Systems
            //

            var posAndRotSystem = new PosAndRotSys();
            var rotSystem = new RotSys();

            systemsManager.Register(posAndRotSystem);
            systemsManager.Register(rotSystem);


            //
            // Entities
            //

            var posAndRotEntity = new Entity();
            posAndRotEntity.Register(new PosComp());
            posAndRotEntity.Register(new RotComp());

            var rotEntity = new Entity();
            rotEntity.Register(new RotComp());

            entitiesManager.Register(posAndRotEntity);
            entitiesManager.Register(rotEntity);


            //
            // Execution
            //

            while (true)
            {
                systemsRunner.RunAll();
            }
        }
    }
}
