using ImGuiNET;
using System.Numerics;

namespace GraphicalUserInterface.GuiParser {
    public readonly struct GuiWindow {
        public readonly int version;
        public readonly string title;
        public readonly Vector2 windowSize;
        public readonly int flags;

        public GuiWindow(int version, string title, Vector2 windowSize, int flags) {
            this.version = version;
            this.title = title;
            this.windowSize = windowSize;
            this.flags = flags;
        }
    }
}
