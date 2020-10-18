using System.IO;

using vtbulk.Interfaces;

namespace vtbulk.InterfaceImplementations.HashList
{
    public class TextFile : IHashList
    {
        public string[] GetHashes(string argument)
        {
            if (!File.Exists(argument))
            {
                throw new FileNotFoundException($"HASH File does not exist: {argument}");
            }

            return File.ReadAllLines(argument);
        }

        public string Name => "TextFile";

        public bool ArgumentRequired => true;
    }
}