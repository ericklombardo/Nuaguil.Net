using System;
using System.Text;
using System.Security.Cryptography;

namespace Nuaguil.Security
{
    public class SaltedHashPassword
    {

        private string _password = string.Empty;
        public string Password
        {
            set
            {
                _password = value;
            }
            get { return _password; }
        }

        public SaltedHashPassword(string password)
        {
            _password = password;
        }
        public byte[] CreatePasswordHash()
        {

            string password = _password;
            // Convert the string password value to a byte array
            byte[] passwordData = UnicodeEncoding.ASCII.GetBytes(password);


            // Create a 4-byte salt using a cryptographically secure random number generator
            byte[] saltData = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltData);


            // Append the salt to the end of the password
            byte[] saltedPasswordData = new byte[passwordData.Length + saltData.Length];
            Array.Copy(passwordData, 0, saltedPasswordData, 0, passwordData.Length);
            Array.Copy(saltData, 0, saltedPasswordData, passwordData.Length, saltData.Length);


            // Create a new SHA-1 instance and compute the hash 
            SHA1Managed sha = new SHA1Managed();
            byte[] hashData = sha.ComputeHash(saltedPasswordData);


            // Optional - add salt bytes onto end of the password hash for storage
            bool APPEND_SALT_TO_HASH = true;


            if (APPEND_SALT_TO_HASH)
            {
                byte[] hashSaltData = new byte[hashData.Length + saltData.Length];
                Array.Copy(hashData, 0, hashSaltData, 0, hashData.Length);
                Array.Copy(saltData, 0, hashSaltData, hashData.Length, saltData.Length);
                return hashSaltData;
            }
            else
            {
                return hashData;
            }
        }

        public bool ComparePasswordToHash(byte[] hashData)
        {
            string password = _password; 
            // First, pluck the four-byte salt off of the end of the hash
            byte[] saltData = new byte[4];
            Array.Copy(hashData, hashData.Length - saltData.Length, saltData, 0, saltData.Length);


            // Convert Password to bytes
            byte[] passwordData = UnicodeEncoding.ASCII.GetBytes(password);


            // Append the salt to the end of the password
            byte[] saltedPasswordData = new byte[passwordData.Length + saltData.Length];
            Array.Copy(passwordData, 0, saltedPasswordData, 0, passwordData.Length);
            Array.Copy(saltData, 0, saltedPasswordData, passwordData.Length, saltData.Length);


            // Create a new SHA-1 instance and compute the hash 
            SHA1Managed sha = new SHA1Managed();
            byte[] newHashData = sha.ComputeHash(saltedPasswordData);


            // Add salt bytes onto end of the password hash for storage
            byte[] newHashSaltData = new byte[newHashData.Length + saltData.Length];
            Array.Copy(newHashData, 0, newHashSaltData, 0, newHashData.Length);
            Array.Copy(saltData, 0, newHashSaltData, newHashData.Length, saltData.Length);

            // Compare and return
            return (Convert.ToBase64String(hashData).Equals(Convert.ToBase64String(newHashSaltData)));
        }


    }
}
