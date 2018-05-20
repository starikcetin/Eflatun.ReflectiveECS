using System;
using Eflatun.ReflectiveECS.Core.Core.ECS;
using Eflatun.ReflectiveECS.Core.Core.Managers;
using Eflatun.ReflectiveECS.Development.ECS;

namespace Eflatun.ReflectiveECS.Development.Testing
{
    public class CorrectnessTester
    {
        private readonly SystemsDatabase _systemsDatabase = new SystemsDatabase();
        private readonly EntitiesDatabase _entitiesDatabase = new EntitiesDatabase();
        private readonly EntityIdManager _entityIdManager = new EntityIdManager();
        private readonly SystemsRunner _systemsRunner;

        public CorrectnessTester()
        {
            _systemsRunner = new SystemsRunner(_systemsDatabase, _entitiesDatabase);
        }

        public void Run()
        {
            InitializeSystems();
            InitializeEntities();
            ExecuteTest();
        }

        private void InitializeSystems()
        {
            var debugSystem = new DebugEntitiesSystem();
            _systemsDatabase.Register(debugSystem);
            _systemsRunner.Cache(debugSystem);

            var posAndRotSys = new PosAndRotSys();
            _systemsDatabase.Register(posAndRotSys);
            _systemsRunner.Cache(posAndRotSys);
        }

        private void InitializeEntities()
        {
            var e1 = new Entity(_entityIdManager.GetUniqueId());
            e1.Register(new PosComp());
            e1.Register(new RotComp());
            _entitiesDatabase.Register(e1);

            var e2 = new Entity(_entityIdManager.GetUniqueId());
            e2.Register(new PosComp());
            e2.Register(new RotComp());
            _entitiesDatabase.Register(e2);
        }

        private void ExecuteTest()
        {
            for (var i = 0; i < 5; i++)
            {
                Console.WriteLine($"----------------- RUN {i} -----------------");
                _systemsRunner.RunAll();
            }
        }
    }
}
