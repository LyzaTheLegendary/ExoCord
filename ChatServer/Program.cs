using ChatServer;
using Core;
using Network;

internal class Program {
    static void Main() {
        Settings.Init("settings.txt");

        Server server = new(new XAddr(Settings.GetValue<string>("server.address", (object)"127.0.0.1:6629")));

    }
}