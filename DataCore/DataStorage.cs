using Core;
using Core.Extensions;
using Core.Files;
using DataCore.Structures;
using System.Collections.Concurrent;
//NOTE: It will corrupt data OR crash if a file is overwritten

namespace DataCore {
    public static class DataStorage {
        const string MAP_FILENAME = "MAP";
        const string DATA_FILENAME = "DATA";
        private static string directory = "/";
        private static readonly ConcurrentDictionary<string, DataInfo> fileMap = new();
        private static readonly Queue<FilePosition> fragments = new();

        static public void Init(string directory) {
            FileHelper.CreateIfNotExistDirectory(directory);
            DataStorage.directory = directory;
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

            Logger.WriteInfo($"Loaded files, {fileMap.Count} and {fragments.Count} fragments");
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

            //TODO:
            //1. Add a file singular fragment [DONE]
            //2. Add a fragmented file [UNTESTED]

            if(fragments.Count == 0) {
                using (FileStream fs = OpenDataWriter()) {
                    WriteSingleFile(fs, name, data);
                    SaveMap();
                    return;
                }
            }


            using(FileStream fs = OpenDataWriter()) {
                WriteSingleFileFragmented(fs, name, data);
            }
        }

        //Broken fix later lol
        static public void RemoveFile(string name) {
            if (!fileMap.TryRemove(name, out DataInfo data)) {
                return;
            }

            foreach (FilePosition fragment in data.GetFragments())
                fragments.Enqueue(fragment);

            SaveMap();
        }

        static private FileStream OpenDataReader() => File.Open(Path.Combine(directory, DATA_FILENAME), FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
        static private FileStream OpenDataWriter() => File.Open(Path.Combine(directory, DATA_FILENAME), FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
        static private void SaveMap() {
            using (FileStream fs = File.Open(Path.Combine(directory, MAP_FILENAME), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
                fs.WriteStruct<int>(fileMap.Count);
                foreach (KeyValuePair<string, DataInfo> kvp in fileMap) {
                    DataHeader header = new DataHeader {
                        fragments = (ushort)kvp.Value.FragmentCount(),
                        flags = kvp.Value.GetFlags(),
                    };

                    fs.WriteStruct<DataHeader>(header);
                    fs.WriteString(kvp.Key);

                    foreach(FilePosition fragment in kvp.Value.GetFragments()) {
                        fs.WriteStruct<FilePosition>(fragment);
                    }
                }

                fs.WriteStruct<int>(fragments.Count);
                foreach (FilePosition position in fragments) {
                    fs.WriteStruct<FilePosition>(position);
                }

            }
        }
        static private void PopulateFileMap(FileStream fs) {
            int fileCount = fs.ReadStruct<int>();
            for (int i = 0; i < fileCount; i++) {
                DataHeader header = fs.ReadStruct<DataHeader>();
                string filename = fs.ReadString();
                List<FilePosition> positions = new(header.fragments);

                for(ushort j = 0; j < header.fragments; j++)
                    positions.Add(fs.ReadStruct<FilePosition>());

                fileMap[filename] = new DataInfo(header.flags, filename, positions);
            }
        }
        static private void PopulateFragmentsList(FileStream fs) {
            int fragmentCount = fs.ReadStruct<int>();
            for (int i = 0; i < fragmentCount; i++) 
                fragments.Enqueue(fs.ReadStruct<FilePosition>());
            
        }
        static private void WriteSingleFile(FileStream fs, string name, byte[] data) {
            fs.Seek(0, SeekOrigin.End);

            FilePosition position = new((int)fs.Position, (int)fs.Position + data.Length);
            DataInfo dataInfo = new DataInfo(0, name, new List<FilePosition>() { position });
            fs.Write(data, 0, data.Length);

            fileMap[name] = dataInfo;

        }
        static private void WriteSingleFileFragmented(FileStream fs, string name, byte[] data) {
            int fileDataRemaining = data.Length;
            int fileWritten = 0;

            List<FilePosition> positions = new List<FilePosition>();
            // If the reminaing data is bigger then the current chunk we simply write it at the end of the data file and leave the loop.
            while(fileDataRemaining > 0) {
                if(fragments.Count == 0) {
                    byte[] dataToWrite = data.Skip(fileWritten).ToArray();
                    fs.Seek(0, SeekOrigin.End);

                    FilePosition pos = new((int)fs.Position, dataToWrite.Length);
                    positions.Add(pos);

                    fs.Write(dataToWrite, 0, dataToWrite.Length);
                    fileDataRemaining = 0;
                    fileWritten += dataToWrite.Length;
                }


                FilePosition position = fragments.Dequeue();
                int size = position.CalcSize(); // 3290658
                // Calculate if the position bigger than data remaining and shrink the original position and slice it.
                if ( size > fileDataRemaining) {
                    // retard code need to fix
                    position.SetEnd(position.GetEnd() - fileDataRemaining);

                    FilePosition newPosition = new(position.GetEnd(), position.GetEnd() + fileDataRemaining);
                    size = newPosition.CalcSize();

                    fs.Seek(newPosition.GetStart(), SeekOrigin.Begin);
                    fs.Write(data.Skip(fileWritten).ToArray(), 0, size);

                    positions.Add(newPosition);
                    fragments.Enqueue(position);

                    fileDataRemaining -= size;
                    fileWritten += size;

                    continue;
                    //int newSize = size - fileDataRemaining;

                    //FilePosition newPos = new FilePosition(position.GetStart() + newSize, position.GetEnd());
                    //position.SetEnd(newPos.GetStart());

                    //fs.Seek(newPos.GetStart(), SeekOrigin.Begin);
                    //fs.Write(data.Skip(newSize).ToArray(), 0, newSize);


                    //fragments.Enqueue(position);
                    //positions.Add(newPos);

                    //fileDataRemaining -= newSize;
                    //fileWritten += newSize;
                    //continue;
                }
                
                // Here we fill a fragment as normal
                fs.Seek(position.GetStart(), SeekOrigin.Begin);
                fs.Write(data.Skip(fileWritten).ToArray(), 0, size);

                fileWritten += size;
                fileDataRemaining -= size;

                positions.Add(position);

            }

            DataInfo info = new(0, name, positions);
            fileMap[name] = info;
            SaveMap();
        }

    }
}
