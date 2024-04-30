using ImGuiNET;
using System.Numerics;

namespace GraphicalUserInterface.Components {
    public class Button : IComponents {
        public string name;
        public Action onClick;
        public Button(string name, Action onClick) {
            this.name = name;
            this.onClick = onClick;
        }

        public Vector2 CalcCenter() {
            var windowSize = ImGui.GetWindowSize();
            var textSize = ImGui.CalcTextSize(name);
            return (windowSize - textSize) * 0.5f;
        }

        public void Render() {
            if (ImGui.Button(name)) {
                // Button throttle?
                onClick?.Invoke();
            }

        }

        public void Resize(float height, float width) {
            // do nothing
        }

        public void Update() {
            // do nothing
        }
    }
}
