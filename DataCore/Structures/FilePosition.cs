using System.Runtime.InteropServices;

namespace DataCore.Structures {

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FilePosition {
        [MarshalAs(UnmanagedType.I4)]
        private readonly int start;
        [MarshalAs(UnmanagedType.I4)]
        private readonly int end;

        public FilePosition(int start, int end) {
            this.start = start;
            this.end = end;
        }
        public int GetStart() => start;
        public int GetEnd() => end;
        public int CalcSize() => end - start;
    }
}
