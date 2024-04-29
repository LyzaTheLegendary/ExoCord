using Comet;
using Core;
using DataCore;
using GraphicalUserInterface.Windowing;
using System.Text;

internal class Program {
    // Idea smart cache, maybe load in the X amount most used resources
    public static void Main() {
        Logger.Init();
        DataStorage.Init("Resource");
#if DEBUG
        Application.AddWindow(new DebugWindow());
#endif
        Application.Run();
        Application.CleanUp();
        
    }
}