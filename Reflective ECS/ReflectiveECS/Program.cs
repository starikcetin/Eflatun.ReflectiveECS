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
            var systemsDatabase = new SystemsDatabase();
            var entitiesDatabase = new EntitiesDatabase();
            var systemsRunner = new SystemsRunner(systemsDatabase, entitiesDatabase);
            var entityIdManager = new EntityIdManager();


            //
            // Systems
            //

            systemsDatabase.Register(new PosAndRotSys());
            systemsDatabase.Register(new RotSys());
            systemsDatabase.Register(new DebugEntitiesSystem());


            //
            // Entities
            //

            var posAndRotEntity = new Entity(entityIdManager.GetUniqueId());
            posAndRotEntity.Register(new PosComp { X = 10, Y = 20 });
            posAndRotEntity.Register(new RotComp { Angle = 100 });

            var rotEntity = new Entity(entityIdManager.GetUniqueId());
            rotEntity.Register(new RotComp { Angle = 200 });

            entitiesDatabase.Register(posAndRotEntity);
            entitiesDatabase.Register(rotEntity);


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
