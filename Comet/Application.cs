using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comet {
    public static class Application {
        private static MainWindow pipe = new();
        private static GlobalTime time = new();
        public static void DoTick() => time.Tick();
        public static GlobalTime GetTime() => time;
        public static void Run(string windowTitle) {
            pipe.Start().GetAwaiter().GetResult();
        }
    }
}
