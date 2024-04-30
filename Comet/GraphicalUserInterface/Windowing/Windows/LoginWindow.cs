using GraphicalUserInterface.Components;
using GraphicalUserInterface.Windowing;
using ImGuiNET;
using System.Numerics;

namespace Comet.GraphicalUserInterface.Windowing
{
    public class LoginWindow : IWindow
    {
        public static bool loggedIn = true;
        public Input username = new("username", 20, ImGuiInputTextFlags.None);
        public Input password = new("password", 20, ImGuiInputTextFlags.Password);
        public Button button;

        public LoginWindow(Action<string, string> onLogin)
        {
            button = new("Login", () =>
            {
                Task.Factory.StartNew(() => onLogin.Invoke(username.text, password.text));
            });
        }

        public bool Enabled(GlobalTime time)
        {
            return loggedIn;
        }

        public void Render(GlobalTime time)
        {
            ImGui.Begin("Comet Login", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoDocking);
            ImGui.SetWindowSize(new Vector2(250, 100));
            username.Render();
            password.Render();
            button.Render();
            ImGui.End();
        }

        public void Resize(float height, float width)
        {
            //throw new NotImplementedException();
        }

        public void Update(GlobalTime time)
        {
            //throw new NotImplementedException();
        }
    }
}
