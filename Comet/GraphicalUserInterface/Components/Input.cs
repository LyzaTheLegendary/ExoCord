using ImGuiNET;
using System.Linq;
using System.Numerics;

namespace GraphicalUserInterface.Components {
    public class Input : IComponents {
        public string label;
        public string text;
        public uint maxLen;
        public ImGuiInputTextFlags flags;

        public Input(string label, uint maxLen, ImGuiInputTextFlags flags = ImGuiInputTextFlags.None) {
            this.label = label;
            this.text = string.Empty;
            this.maxLen = maxLen;
            this.flags = flags;
        }

        public Vector2 CalcCenter() {
            var windowSize = ImGui.GetWindowSize();
            var textSize = ImGui.CalcTextSize(text);
            return (windowSize - textSize) * 0.5f;
        }

        public void Render() {
            ImGui.InputText(label, ref text, maxLen, flags);
        }

        public void Resize(float height, float width) {
            // do nothing;
        }

        public void Update() {
            // do nothing
        }
    }
}
