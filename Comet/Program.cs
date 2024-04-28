using Comet;
using Core;
using DataCore;
using System.Text;

internal class Program {
    public static void Main() {
        Logger.Init();
        DataStorage.Init("Resource");
        //byte[] imageBytes = File.ReadAllBytes("image.jpeg");
        //byte[] textBytes = File.ReadAllBytes("test.txt");

        //DataStorage.AddFile("image", imageBytes);
        //DataStorage.RemoveFile("image");

        //DataStorage.AddFile("text", textBytes);

        //Console.WriteLine(Encoding.UTF8.GetString(DataStorage.FetchFile("text")));

        Application.Run();
        Application.CleanUp();
        
    }
}