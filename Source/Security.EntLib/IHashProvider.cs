namespace Nuaguil.Security.EntLib
{
    public interface IHashProvider
    {
        string CreateHash(string plainText);
        bool IsHashEqual(string plainText,string hashText);
    }
}
