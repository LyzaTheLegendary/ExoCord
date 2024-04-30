using Core;
using GraphicalUserInterface;
using GraphicalUserInterface.Windowing;

namespace Comet
{
    static public class Application {
        static private WindowManager windowManager = new();
        static private GlobalTime time = new();
        static public void DoTick() => time.Tick();
        static public GlobalTime GetTime() => time;

        static public void AddWindow(IWindow window) => windowManager.AddWindow(window);
        static public void Run() {
            Logger.WriteInfo("Started WindowManager");
            windowManager.Start().GetAwaiter().GetResult();
        }
        static public void CleanUp() {

        }
    }
}
