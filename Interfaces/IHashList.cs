namespace vtbulk.Interfaces
{
    public interface IHashList
    {
        string Name { get; }

        bool ArgumentRequired { get; }

        string[] GetHashes(string argument);
    }
}