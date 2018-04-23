using System;
using ReflectiveECS.Core.ECS;
using ReflectiveECS.Core.Managers;
using ReflectiveECS.Development.ECS;

namespace ReflectiveECS
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var systemsManager = new SystemsDatabase();
            var entitiesManager = new EntitiesDatabase();
            var systemsRunner = new SystemsRunner(systemsManager, entitiesManager);
            var entityIdManager = new EntityIdManager();


            //
            // Systems
            //

            systemsManager.Register(new PosAndRotSys());
            systemsManager.Register(new RotSys());
            systemsManager.Register(new DebugEntitiesSystem());


            //
            // Entities
            //

            var posAndRotEntity = new Entity(entityIdManager.GetUniqueId());
            posAndRotEntity.Register(new PosComp { X = 10, Y = 20 });
            posAndRotEntity.Register(new RotComp { Angle = 100 });

            var rotEntity = new Entity(entityIdManager.GetUniqueId());
            rotEntity.Register(new RotComp { Angle = 200 });

            entitiesManager.Register(posAndRotEntity);
            entitiesManager.Register(rotEntity);


            //
            // Execution
            //

            Console.WriteLine("======= RUN 0 ========");
            systemsRunner.RunAll();
            Console.WriteLine("======= RUN 1 ========");
            systemsRunner.RunAll();
            Console.WriteLine("======= RUN 2 ========");
            systemsRunner.RunAll();
            Console.WriteLine("======= RUN 3 ========");
            systemsRunner.RunAll();

            Console.ReadLine();
        }
    }
}
