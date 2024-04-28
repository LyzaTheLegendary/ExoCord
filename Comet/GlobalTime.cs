
using System.Diagnostics;

namespace Comet {
    public class GlobalTime {
        // This will calculate the time within the game

        double secondsPerCount;

        long prevCount = 0;
        long curCount;
        long baseCount;
        public float TotalSeconds { get => (float)((curCount - baseCount) * secondsPerCount); }
        public GlobalTime() {
            secondsPerCount = 1.0 / Stopwatch.Frequency;
            baseCount = Stopwatch.GetTimestamp();
        }

        public void Tick() {
            curCount = Stopwatch.GetTimestamp();
            prevCount = curCount;
        }

        public float GetDelta() => (float)((curCount - prevCount) / secondsPerCount);
    }
}
