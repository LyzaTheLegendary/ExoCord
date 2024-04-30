
using System.Numerics;

namespace GraphicalUserInterface.Components
{
    public interface IComponents
    {
        // should be based on a outside reference like a function SHOULD activate which returns a int which is their ID 
        //public int GetId();
        public void Resize(float height, float width);
        public void Update();
        public void Render();
        public Vector2 CalcCenter();
    }
}
