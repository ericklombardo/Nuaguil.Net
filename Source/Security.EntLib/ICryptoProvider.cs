namespace Nuaguil.Security.EntLib
{
    public interface ICryptoProvider
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}
