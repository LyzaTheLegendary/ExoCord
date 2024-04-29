using ClickableTransparentOverlay;
using Comet;
using GraphicalUserInterface.Windowing;
using ImGuiNET;
using System.Collections.Concurrent;
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

        bool show = true;
        Vector2 lastWindowSize = default;
        BlockingCollection<IWindow> windows = new();
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int key);

        public void AddWindow(IWindow window) => windows.Add(window);

        protected override void Render()
        {
            GlobalTime time = Application.GetTime();
            time.Tick();
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
            
            Vector2 windowSize = ImGui.GetWindowSize();
            if (windowSize != lastWindowSize)
            {
                OnResize(windowSize.Y, windowSize.X);
                lastWindowSize = windowSize;
            }

            OnUpdate(time);
            foreach (IWindow window in windows) {
                window.Render(time);
            }
        }

        private void OnUpdate(GlobalTime time)
        {
            foreach(IWindow window in windows) {
                window.Update(time);
            }
        }

        private void OnResize(float height, float width)
        {
            foreach (IWindow window in windows) {
                window.Resize(height, width);
            }
        }

    }
}
