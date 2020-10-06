using System;

namespace vtbulk.Objects
{
    public class CommandLineArgumentsItem
    {
        public string OutputFilePath { get; set; }

        public string InputHashFile { get; set; }
    
        public string VTKey { get; set; }

        public CommandLineArgumentsItem()
        {
            OutputFilePath = AppContext.BaseDirectory;
        }
    }
}