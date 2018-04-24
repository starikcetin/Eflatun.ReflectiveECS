using System;
using System.Diagnostics;
using Development.ECS;
using ReflectiveECS.Core.ECS;
using ReflectiveECS.Core.Managers;

namespace Development.Testing
{
    public class PerformanceTester
    {
        private readonly int _frameCount;
        private readonly int _systemCount;
        private readonly int _entityCount;
        private readonly int _componentPerEntity;
        private readonly bool _clearConsoleBeforeFinal;
        private readonly bool _beepOnEnd;

        private readonly SystemsDatabase _systemsDatabase;
        private readonly EntitiesDatabase _entitiesDatabase;
        private readonly SystemsRunner _systemsRunner;
        private readonly EntityIdManager _entityIdManager;
        private readonly EntityGrouper _entityGrouper;

        public PerformanceTester(int frameCount, int systemCount, int entityCount, int componentPerEntity, bool clearConsoleBeforeFinal, bool beepOnEnd)
        {
            _beepOnEnd = beepOnEnd;
            _clearConsoleBeforeFinal = clearConsoleBeforeFinal;
            _componentPerEntity = componentPerEntity;
            _entityCount = entityCount;
            _systemCount = systemCount;
            _frameCount = frameCount;
            _systemsDatabase = new SystemsDatabase();
            _entitiesDatabase = new EntitiesDatabase();
            _entityGrouper = new EntityGrouper();
            _systemsRunner = new SystemsRunner(_systemsDatabase, _entitiesDatabase, _entityGrouper);
            _entityIdManager = new EntityIdManager();
        }

        public void Run()
        {
            InitializeSystems();
            InitializeEntities();
            ExecuteTest();
        }

        private void ExecuteTest()
        {
            long totalTicks = MeasureTotalTicks();

            double frameAvgTicks = totalTicks / (double) _frameCount;
            double framesPerTick = (double) _frameCount / totalTicks;
            double framePerSec = framesPerTick * TimeSpan.TicksPerSecond;

            if (_clearConsoleBeforeFinal)
            {
                Console.Clear();
            }

            Console.WriteLine("------------");
            Console.WriteLine($"Total        {totalTicks:N0} ticks");
            Console.WriteLine($"Frame avg    {frameAvgTicks:N0} ticks");
            Console.WriteLine($"FPS          {framePerSec:N3}");
            Console.WriteLine();
            Console.WriteLine($"Entities     {_entityCount:N0}");
            Console.WriteLine($"Comp per Ent {_componentPerEntity:N0}");
            Console.WriteLine($"Systems      {_systemCount:N0}");
            Console.WriteLine();
            Console.WriteLine($"Frames       {_frameCount:N0}");
            Console.WriteLine("------------");

            if (_beepOnEnd)
            {
                Console.Beep();
            }
        }

        private void InitializeSystems()
        {
            for (var i = 0; i < _systemCount; i++)
            {
                var pars0 = new PosAndRotSys();
                _systemsDatabase.Register(pars0);
                _systemsRunner.Cache(pars0);
            }
        }

        private void InitializeEntities()
        {
            for (var i = 0; i < _entityCount; i++)
            {
                var entity = new Entity(_entityIdManager.GetUniqueId());

                for (var j = 0; j < _componentPerEntity; j++)
                {
                    entity.Register(new PosComp {X = 10, Y = 20});
                    _entityGrouper.Register(entity, typeof(PosComp));
                    entity.Register(new RotComp());
                    _entityGrouper.Register(entity, typeof(RotComp));
                }

                _entitiesDatabase.Register(entity);
            }

            var rotEntity = new Entity(_entityIdManager.GetUniqueId());
            rotEntity.Register(new RotComp {Angle = 200});
            _entitiesDatabase.Register(rotEntity);
        }

        private long MeasureTotalTicks()
        {
            long totalTicks = 0;

            for (var i = 0; i < _frameCount; i++)
            {
                var frameTicks = MeasureSingleFrameTicks();
                totalTicks += frameTicks;
                Console.WriteLine($"Frame #{i} - {frameTicks} ticks");
            }

            return totalTicks;
        }

        private long MeasureSingleFrameTicks()
        {
            var sw = new Stopwatch();
            sw.Start();

            _systemsRunner.RunAll();

            sw.Stop();
            return sw.ElapsedTicks;
        }
    }
}
