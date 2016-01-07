using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace Nuaguil.Security.EntLib
{
    public sealed class CryptoProvider : ICryptoProvider
    {

        #region Singleton

        public static ICryptoProvider Instance 
        {             
            get { return Nested.Instance; }
        
        }

        private CryptoProvider()
        {
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly CryptoProvider Instance = new CryptoProvider();
        }

        #endregion

        private static string _symmetricInstanceName;

        public static string CurrentSymmetricInstanceName
        {
            get { return _symmetricInstanceName; }
            set { _symmetricInstanceName = value; }
        }

        #region Implementation of ICryptoProvider

        public string Decrypt(string cipherText)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<CryptographyManager>().DecryptSymmetric(_symmetricInstanceName, cipherText); 
        }

        public string Encrypt(string plainText)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<CryptographyManager>().EncryptSymmetric(_symmetricInstanceName, plainText);
        }

        #endregion

    }
}
