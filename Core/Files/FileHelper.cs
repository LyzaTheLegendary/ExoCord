﻿namespace Core.Files
{
    public static class FileHelper
    {

        static public void CreateIfNotExistDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                path = path.Replace("\\", "/");
                if (!path.Contains("/")) Directory.CreateDirectory(path);
                else foreach (var s in path.Split("/"))
                        Directory.CreateDirectory(s);
            }
        }
        static public void CreateFileIfnotExist(string path) {
            if (!File.Exists(path)) {
                File.Create(path).Close();
            }
        }
    }
}
