using System.Collections.Concurrent;
using System.Text;

namespace Core {
    static public class Translations {
        private static ConcurrentDictionary<string, string> translationMap = new();
        static public void Init(byte[] translationFile) {
            string[] lines = Encoding.UTF8.GetString(translationFile).Split('\n');

            foreach (string line in lines) {
                if (!line.Contains('='))
                    continue;

                string[] kvp = line.Split('=');

                string key = kvp[0].Trim();
                string value = kvp[1].Trim();

                translationMap[key] = value;
            }
        }

        public static string GetTranslation(string key) {
            if (!translationMap.TryGetValue(key, out string? translation)) {
                return $"Missing translation: {key}";
            }
            return translation;
        }
    }
}
