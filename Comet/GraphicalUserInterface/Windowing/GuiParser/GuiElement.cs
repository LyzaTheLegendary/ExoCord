using System.Numerics;
using System.Runtime.InteropServices;
namespace GraphicalUserInterface.GuiParser {
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GuiElement {
        public int id;
        public int index;
        public Vector2 offset;
        public int elementType;
        public string text;
    }
}
