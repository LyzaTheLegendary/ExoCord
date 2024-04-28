using GraphicalUserInterface;

namespace Comet
{
    public static class Application {
        private static WindowManager windowManager = new();
        private static GlobalTime time = new();
        public static void DoTick() => time.Tick();
        public static GlobalTime GetTime() => time;
        public static void Run() {
            windowManager.Start().GetAwaiter().GetResult();
        }
        public static void CleanUp() {

        }
    }
}
