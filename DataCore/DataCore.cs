using Core.Extensions;
using Core.Files;
using DataCore.Structures;
using System.Collections.Concurrent;


namespace DataCore {
    public static class DataCore {
        const string MAP_FILENAME = "MAP";
        const string DATA_FILENAME = "DATA";
        private static string directory = "/";
        private static readonly ConcurrentDictionary<string, DataInfo> fileMap = new();
        private static readonly Queue<FilePosition> fragments = new();

        static public void Init(string directory) {
            FileHelper.CreateIfNotExistDirectory(directory);
            DataCore.directory = directory;
            fileMap.Clear();
            fragments.Clear();

            string filePath = Path.Combine(directory, MAP_FILENAME);
            if (!File.Exists(filePath))
                return;
            else if (new FileInfo(filePath).Length == 0)
                return;

            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) {
                PopulateFileMap(fs);
                PopulateFragmentsList(fs);
            }

        }
        
        static public byte[]? FetchFile(string name) {
            if (!fileMap.TryGetValue(name, out DataInfo data))
                return null;

            List<byte> dataBuffer = new List<byte>();

            using (FileStream fs = OpenDataReader()) {
                foreach (FilePosition fragment in data.GetFragments()) {
                    fs.Seek(fragment.GetStart(), SeekOrigin.Begin);

                    int fragSize = fragment.CalcSize();
                    byte[] buff = new byte[fragSize];

                    fs.ReadExactly(buff, 0, fragSize);

                    dataBuffer.AddRange(buff);
                }
            }
            return dataBuffer.ToArray();
        }

        static public void AddFile(string name, byte[] data) {
            //overwriting files should exist :moon:
        }

        static private FileStream OpenDataReader() => File.Open(Path.Combine(directory, DATA_FILENAME), FileMode.Open, FileAccess.Read, FileShare.Read)) {
        static private void SaveMap() {
            using (FileStream fs = File.Open(Path.Combine(directory, MAP_FILENAME), FileMode.Truncate, FileAccess.Write, FileShare.None)) {
                fs.WriteStruct<int>(fileMap.Count);

                foreach(KeyValuePair<string,DataInfo> kvp in fileMap) {
                    DataHeader header = new DataHeader {
                        fragments = (ushort)kvp.Value.FragmentCount(),
                        flags = kvp.Value.GetFlags(),
                    };

                    fs.WriteStruct<DataHeader>(header);
                }
            }
        }
        static private void PopulateFileMap(FileStream fs) {
            for (int i = 0; i < fs.ReadStruct<int>(); i++) {
                DataHeader header = fs.ReadStruct<DataHeader>();
                string filename = fs.ReadString();
                List<FilePosition> positions = new(header.fragments);

                for(ushort j = 0; j < header.fragments; j++)
                    positions.Add(fs.ReadStruct<FilePosition>());

                fileMap[filename] = new DataInfo(header.flags, filename, positions);
            }
        }

        static private void PopulateFragmentsList(FileStream fs) {
            for(int i = 0;i < fs.ReadStruct<int>(); i++) 
                fragments.Enqueue(fs.ReadStruct<FilePosition>());
            
        }
    }
}
