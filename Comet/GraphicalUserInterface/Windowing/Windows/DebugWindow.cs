using Comet;
using Core;
using GraphicalUserInterface.Windowing;
using ImGuiNET;
using Network;
namespace Comet.GraphicalUserInterface.Windowing
{
    public class DebugWindow : IWindow
    {
        public static volatile bool connected = false;
        float elapsedSeconds = 0;
        int framecount = 0;
        int framesPerSecond = 0;
        public bool Enabled(GlobalTime time)
        {
            return true;
        }

        public void Render(GlobalTime time)
        {
            ImGui.Begin("DebugWindow");
            WindowStyle();
            ImGui.Text("fps: " + framesPerSecond);
            ImGui.SameLine();
            ImGui.Text($"connected: {connected}");
            ImGui.Text($"address: {Connection.GetAddress()}");
#if DEBUG
            ImGui.Text("version: DEBUG");
#else
            ImGui.Text("version: RELEASE");
#endif
            ImGui.End();
        }

        public void Resize(float height, float width)
        {
            // Do nothing.
        }

        public void Update(GlobalTime time)
        {
            CalcFps(time);
        }
        private void WindowStyle()
        {
            ImGuiStylePtr style = ImGui.GetStyle();
            style.WindowRounding = 8;
        }
        private void CalcFps(GlobalTime time)
        {
            framecount++;
            if (time.TotalSeconds - elapsedSeconds >= 1.0f)
            {
                elapsedSeconds = time.TotalSeconds;

                framesPerSecond = framecount;
                framecount = 0;
            }
        }
    }
}
