using System.Collections.Concurrent;

namespace Core {

    public static class Settings {
        const string STRING = "str";
        const string INT = "i32";
        const string BOOL = "bool";

        private static ConcurrentDictionary<string, object> m_settingsMap = new();

        public static void Init(string path) {
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines) {
                if (line.StartsWith("//"))
                    continue;

                if (!line.Contains("=") || !line.Contains(':'))
                    continue;

                string type = line.Split(":")[0].Trim();
                string name = line.Split(':')[1].Split("=")[0].Trim();
                string value = line.Split("=")[1].Trim();

                switch (type){
                    case STRING:
                        m_settingsMap[name] = value;
                        break;
                    case INT:
                        m_settingsMap[name] = Int32.Parse(value);
                        break;
                    case BOOL:
                        m_settingsMap[name] = bool.Parse(value);
                        break;
                    default:
                        Console.WriteLine($"[WARN] Failed to read value type of {name}");
                        break;
                }
            }
        }

        public static T GetValue<T>(string key, T defaultValue) {
            if (!m_settingsMap.TryGetValue(key, out object? value)) {
                return defaultValue;
            }

            return (T)value;
        }
    }
}
