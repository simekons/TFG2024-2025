using Telemetry;
using System;
namespace Test
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("¡Hola, mundo!");
            Telemetry.Tracker tracker = Telemetry.Tracker.Instance("Test",Telemetry.Persistance.PersistanceType.File, Telemetry.Serialization.SerializeType.JSON, "Prueba");
            tracker.init();
            tracker.startGame();
            tracker.AddGameEvent(new TestEvent());
            tracker.endGame();
            tracker.startGame();
            tracker.AddGameEvent(new TestEvent());
            tracker.AddGameEvent(new TestEvent());
            tracker.AddGameEvent(new TestEvent());
            tracker.endGame();
            tracker.endSession();
            tracker.init();
            tracker.startGame();
            tracker.AddGameEvent(new TestEvent());
            tracker.endGame();
            tracker.startGame();
            tracker.AddGameEvent(new TestEvent());
            tracker.AddGameEvent(new TestEvent());
            tracker.AddGameEvent(new TestEvent());
            tracker.endGame();
            tracker.endSession();
            tracker.end();
        }
    }
}