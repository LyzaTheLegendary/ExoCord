using ChatServer;
using Core;
using Network;
using System.Diagnostics;

internal class Program {
    static void Main() {
        Logger.Init();
        Settings.Init("settings.txt");

        Server server = new(new XAddr(Settings.GetValue<string>("server.address", "127.0.0.1:6629")));


        Process.GetCurrentProcess().WaitForExit();

    }
}