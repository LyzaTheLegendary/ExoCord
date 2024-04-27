using Core.Extensions;
using Core.Files;
using System.Collections.Concurrent;
using System.Text;

namespace Core
{
    public static class Logger {

        private static BlockingCollection<byte[]> consoleBuff = new();
        const int LOGGED = 1;
        const int RED = 2;
        const int YELLOW = 4;
        const int BOLD = 8;
        const int GREEN = 16;
        public static void Init() {
            Task.Factory.StartNew(HandleConsoleMsgs, TaskCreationOptions.LongRunning);
        }
        private static void HandleConsoleMsgs() {
            foreach (byte[] consoleEntry in consoleBuff.GetConsumingEnumerable()) {
                Thread.Yield();

                int flags = consoleEntry.Take(sizeof(int)).ToArray().BitCast<int>();
                string text = Encoding.UTF8.GetString(consoleEntry.Skip(sizeof(int)).ToArray());

                if ((flags & LOGGED) != 0) {
                    try {
                        string directory = Path.Combine("logs");
                        FileHelper.CreateIfNotExistDirectory(directory);

                        string fileName = $"logs-{DateTime.Now.ToString("yyyy-MM-dd-HH")}.txt";

                        string path = Path.Combine(directory, fileName);
                        if (!File.Exists(path)) {
                            File.Create(path).Close();
                        }
                        File.AppendAllText(path, text + "\r\n");
                    } catch (Exception e) {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"\u001b[1m{e.Message}\u001b[0m");
                    }
                }

                if ((flags & RED) != 0) {
                    Console.ForegroundColor = ConsoleColor.Red;
                } else if ((flags & YELLOW) != 0) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }

                if ((flags & BOLD) != 0 ) {
                    Console.WriteLine($"\u001b[1m{text}\u001b[0m");
                }else {
                    Console.WriteLine(text);
                }
                
                Console.ResetColor();
            }
        }

        public static void WriteInfo(string msg, bool logged = false) {
            int flags = 0;

            if (logged) {
                flags += LOGGED;
            }

            Write("[INFO] " + msg, flags);
        }

        public static void WriteWarn(string msg) 
            => Write("[WARN] " + msg, LOGGED + YELLOW + BOLD);


        public static void WriteErr(string msg) 
            => Write("[ERR] " + msg, BOLD + RED + LOGGED);

        private static void Write(string msg, int flags) {
            int threadId = Thread.GetCurrentProcessorId();
            string currentDate = DateTime.Now.ToString("HH:mm:ss");

            List<byte> bytes =
                [
                    .. flags.BitCast(),
                    .. Encoding.UTF8.GetBytes($"[{currentDate}] [Thread/{threadId}]  {msg}"),
                ];

            consoleBuff.Add(bytes.ToArray()); ;
        }
    }
}
