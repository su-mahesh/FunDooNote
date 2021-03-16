using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace RepositoryLayer.Services
{
    /// <summary>
    /// Password Encryption class
    /// </summary>
    class PasswordEncryption
    {
        readonly byte[] salt = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70,
                 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70,  0x60, 0x70};
        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="Password">The password.</param>
        /// <returns></returns>
        public string EncryptPassword(String Password)
        {          
            Console.WriteLine(salt);
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: Password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }       
    }
}
