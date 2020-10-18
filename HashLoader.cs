using System.IO;

namespace vtbulk
{
    public static class HashLoader
    {
        public  static string[] GetHashes(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"HASH File does not exist: {fileName}");
            }

            return File.ReadAllLines(fileName);
        }
    }
}