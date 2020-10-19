using System;
using System.IO;

using vtbulk.Interfaces;

namespace vtbulk.InterfaceImplementations.HashList
{
    public class TextFile : IHashList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        public string[] GetHashes(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(nameof(argument));
            }

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