using System;

using vtbulk.Interfaces;

namespace vtbulk.Objects
{
    public class CommandLineArgumentsItem
    {
        public string OutputFilePath { get; set; }

        public string VTKey { get; set; }

        public bool VerboseOutput { get; set; }

        public bool EnableMultithreading { get; set; }

        public IHashList HashSource { get; set; }

        public string HashListSourceArgument { get; set; }

        public CommandLineArgumentsItem()
        {
            OutputFilePath = AppContext.BaseDirectory;
        }
    }
}