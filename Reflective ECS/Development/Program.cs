using System;
using System.Collections.Generic;
using System.Diagnostics;
using Development.ECS;
using ReflectiveECS.Core.ECS;
using ReflectiveECS.Core.Managers;

namespace Development
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
            systemsDatabase.Register(new PosAndRotSys());
            systemsDatabase.Register(new RotSys());
            systemsDatabase.Register(new PosAndRotSys());
            systemsDatabase.Register(new RotSys());
            systemsDatabase.Register(new PosAndRotSys());
            systemsDatabase.Register(new RotSys());
            systemsDatabase.Register(new PosAndRotSys());
            systemsDatabase.Register(new RotSys());
            //systemsDatabase.Register(new DebugEntitiesSystem());


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

            PerformanceTest(systemsRunner);
        }

        private static void PerformanceTest(SystemsRunner systemsRunner)
        {
            const int batchCount = 50;
            const int batchSize = 500;

            var elapses = new List<long>();
            var sw = new Stopwatch();

            for (var j = 0; j < batchCount; j++)
            {
                sw.Start();
                for (var i = 0; i < batchSize; i++)
                {
                    systemsRunner.RunAll();
                }

                sw.Stop();
                elapses.Add(sw.ElapsedMilliseconds);
                sw.Reset();
            }

            Console.Clear();
            Console.Beep();

            long total = 0;
            for (var i = 0; i < elapses.Count; i++)
            {
                var e = elapses[i];
                Console.WriteLine($"Batch: {i} | Elapsed: {e} ms | Average: {e / (double) batchSize} ms");
                total += e;
            }

            Console.WriteLine($"Total Elapsed: {total} ms |" +
                              $" Batch Total Average: {total / (double) batchCount} ms |" +
                              $" Overall Average: {total / (double) (batchCount * batchSize)} ms");
        }
    }
}
