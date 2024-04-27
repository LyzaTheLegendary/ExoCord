using Core.Extensions;
using Core.Files;
using System.Collections.Concurrent;
using System.Text;

namespace Core
{
    public static class Logger {
        // Thank you ruby how very kind of totally not letting me steal code.
        public static String ANSI_RESET = "\u001B[0m";
        public static String ANSI_BLACK = "\u001B[30m";
        public static String ANSI_RED = "\u001B[31m";
        public static String ANSI_GREEN = "\u001B[32m";
        public static String ANSI_YELLOW = "\u001B[33m";
        public static String ANSI_BLUE = "\u001B[34m";
        public static String ANSI_PURPLE = "\u001B[35m";
        public static String ANSI_CYAN = "\u001B[36m";
        public static String ANSI_WHITE = "\u001B[37m";
        public static String ANSI_BLACK_BACKGROUND = "\u001B[40m";
        public static String ANSI_RED_BACKGROUND = "\u001B[41m";
        public static String ANSI_GREEN_BACKGROUND = "\u001B[42m";
        public static String ANSI_YELLOW_BACKGROUND = "\u001B[43m";
        public static String ANSI_BLUE_BACKGROUND = "\u001B[44m";
        public static String ANSI_PURPLE_BACKGROUND = "\u001B[45m";
        public static String ANSI_CYAN_BACKGROUND = "\u001B[46m";
        public static String ANSI_WHITE_BACKGROUND = "\u001B[47m";

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
