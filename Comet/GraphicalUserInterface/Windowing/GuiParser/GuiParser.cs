using Core.Extensions;
using System.Numerics;

namespace GraphicalUserInterface.GuiParser {
    public class GuiParser : IDisposable {
        private MemoryStream inputStream;
        private GuiWindow header;
        private List<GuiElement> elements = new();
        // What if elements call their function based on a ID 
        // a window would have a map[int] => values? whenever it's called the map simple retrieves the id :D 
        public void ReadGui(byte[] guiFile) {
            inputStream = new MemoryStream(guiFile);
            //TODO turn into a struct read call
            int version = inputStream.ReadStruct<int>();
            string title = inputStream.ReadString();
            Vector2 windowSize = inputStream.ReadStruct<Vector2>();
            int flags = inputStream.ReadStruct<int>();

            header = new GuiWindow(version, title, windowSize, flags);
            int elementCount = inputStream.ReadStruct<int>();

            for(int i = 0; i < elementCount; i++ ) {
                GuiElement element = default( GuiElement );
                element.id = inputStream.ReadStruct<int>();
                element.index = inputStream.ReadStruct<int>();
                element.offset = inputStream.ReadStruct<Vector2>();
                element.elementType = inputStream.ReadStruct<int>();
                element.text = inputStream.ReadString();

                elements.Add(element);
            }
                

            //elements = elements.OrderBy(e => e.index).ToList();
        }

        public GuiWindow GetHeader() => header;

        public void Dispose() {
            inputStream.Dispose();
        }
    }
}
