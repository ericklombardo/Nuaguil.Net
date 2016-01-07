using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using IHashProvider = Nuaguil.Security.EntLib.IHashProvider;

namespace Nuaguil.Security.EntLib
{
    public sealed class HashProvider : IHashProvider
    {

        #region Singleton

        public static IHashProvider Instance 
        {             
            get { return Nested.Instance; }
        
        }

        private HashProvider()
        {
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly HashProvider Instance = new HashProvider();
        }

        #endregion

        private static string _hashInstanceName;

        public static string CurrentHashInstanceName
        {
            get { return _hashInstanceName; }
            set { _hashInstanceName = value; }
        }

        #region Implementation of IHashProvider

        public string CreateHash(string plainText)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<CryptographyManager>().CreateHash(_hashInstanceName,
                                                                                                    plainText);
        }

        public bool IsHashEqual(string plainText, string hashText)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<CryptographyManager>().CompareHash(_hashInstanceName,
                                                                                                     plainText, hashText);
        }

        #endregion

    }
}
