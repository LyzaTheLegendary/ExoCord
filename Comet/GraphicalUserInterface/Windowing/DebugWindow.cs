using Comet;
using ImGuiNET;
namespace GraphicalUserInterface.Windowing {
    public class DebugWindow : IWindow {
        float elapsedSeconds = 0;
        int framecount = 0;
        int framesPerSecond = 0;

        public void Destroy(GlobalTime time) {
            // Do nothing.
        }

        public void Render(GlobalTime time) {
            ImGui.Begin("DebugWindow");
            WindowStyle();
            ImGui.SameLine();
            ImGui.Text("fps: " + framesPerSecond);
            ImGui.SameLine();
            ImGui.Text("ver: DEBUG");
            ImGui.End();
        }

        public void Resize(float height, float width) {
            // Do nothing.
        }

        public void Update(GlobalTime time) {
            CalcFps(time);
        }
        private void WindowStyle() {
            ImGuiStylePtr style = ImGui.GetStyle();
            style.WindowRounding = 8;
        }
        private void CalcFps(GlobalTime time) {
            framecount++;
            if (time.TotalSeconds - elapsedSeconds >= 1.0f) {
                elapsedSeconds = time.TotalSeconds;

                framesPerSecond = framecount;
                framecount = 0;
            }
        }
    }
}
