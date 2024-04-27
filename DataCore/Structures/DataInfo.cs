namespace DataCore.Structures {
    public struct DataInfo {
        private readonly int flags;
        private readonly string filename;
        private readonly List<FilePosition> filePositions;
        public DataInfo(int flags, string filename, List<FilePosition> positions) {
            this.flags = flags;
            this.filename = filename.Trim();
            filePositions = positions;
        }
        public string GetFilename() => filename;
        public int FragmentCount() => filePositions.Count;
        public FilePosition[] GetFragments() => filePositions.ToArray();
        public int GetFlags() => flags;
    }
}
