using ClickableTransparentOverlay;
using Comet;
using ImGuiNET;
using System.Numerics;
using System.Runtime.InteropServices;


//TODO:
// 1. Move away from this horrible overlay class
// 2. Create my own windowing class for both windows and linux
// 3. figure out a way to find out if a Fullscreen application is in focus and if it is to find it's window id to attach to it

namespace GraphicalUserInterface
{
    public class WindowManager : Overlay
    {
        float elapsedSeconds = 0;
        int framecount = 0;
        int framesPerSecond = 0;
        bool show = true;

        Vector2 lastWindowSize = default;

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int key);

        protected override void Render()
        {
            GlobalTime time = Application.GetTime();
            time.Tick();
            CalcFps(time);
            // Insert key
            if (GetAsyncKeyState(0x2D) != 0)
            {
                show = !show;
                Thread.Sleep(150); // should use global time for this
                return;
            }

            if (!show)
            {
                Thread.Sleep(600);
                return;
            }

            RenderDebugWindow();

            // This context should be in it's own window class!
            ImGui.Begin($"Comet DEBUG", ImGuiWindowFlags.NoCollapse);

            DefaultStyle();

            Vector2 windowSize = ImGui.GetWindowSize();
            if (windowSize != lastWindowSize)
            {
                OnResize(windowSize.Y, windowSize.X);
                lastWindowSize = windowSize;
            }


            OnUpdate(time);


            // Draw view



            ImGui.End();
        }
        private void OnUpdate(GlobalTime time)
        {
            // Update views components
        }

        private void OnResize(float height, float width)
        {
            Console.WriteLine($"{height}, {width}");

        }


        private void RenderDebugWindow()
        {
            ImGui.Begin("DebugWindow");
            ImGui.Text("fps: " + framesPerSecond);
            ImGui.End();
        }
        private void DefaultStyle()
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
