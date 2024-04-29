using Comet;

namespace GraphicalUserInterface.Windowing {
    public interface IWindow {
        public void Resize(float height, float width);
        public void Update(GlobalTime time);
        public void Render(GlobalTime time);
        public void Destroy(GlobalTime time);
    }
}
