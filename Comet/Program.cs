using Comet;
using Comet.GraphicalUserInterface.Windowing;
using Core;
using Core.Extensions;
using DataCore;
using Network;
using Packets;

internal class Program {
    // Idea smart cache, maybe load in the X amount most used resources
    public static void Main() {

        Header header = default(Header);
        byte[] headerBytes = header.BitCast();
        Header newHeader = headerBytes.BitCast<Header>();

        Logger.Init();
        DataStorage.Init("Resource");
        Settings.Init(null, DataStorage.FetchFile("settings"));
        Translations.Init(DataStorage.FetchFile("translations"));
        

#if DEBUG
        Application.AddWindow(new DebugWindow());
#endif
        Application.AddWindow(new LoginWindow((username, password) => {
            //Console.WriteLine(username + ":" + password);
            ResultCode result = Connection.Connect(username, password);
            Console.WriteLine(result.ToString());
        }));

        Application.Run();
        Application.CleanUp();
        
    }
}