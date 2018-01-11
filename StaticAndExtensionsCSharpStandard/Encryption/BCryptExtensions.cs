using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Encryption
{
    public static class BCryptExtensions
    {
        /// <summary>
        /// Used for Hashing Security Factor in BCrypt encryption.
        /// </summary>
        private const int ConstantBCryptSecurityFactor = 10;

        /// <summary>
        /// Create a secure hash based on the input string.
        /// </summary>
        /// <param name="strToBCrypt">
        /// String to hash.
        /// </param>
        /// <param name="securityFactor">
        /// he cost factor for bcrypt is exponential, or rather, a cost factor of 10 means 2^10 rounds (1024), 
        /// a cost factor of 16 would mean 2^16 rounds (65536). It's natural then that it would take 5-10 seconds. 
        /// It should take about 64 times as long as a cost factor of 10 does.
        /// </param>
        /// <returns>
        /// Hash for this string.
        /// </returns>
        public static string GetSecureHash_BCrypt(this string strToBCrypt, int securityFactor = ConstantBCryptSecurityFactor) =>
            BCrypt.Net.BCrypt.HashPassword(strToBCrypt, BCrypt.Net.BCrypt.GenerateSalt(securityFactor));

        /// <summary>
        /// Compare true passwords.
        /// </summary>
        /// <param name="plainTextPassword">The plain text password.</param>
        /// <param name="hashedPassword">The Encrypted password, normal stored on DB.</param>
        /// <returns></returns>
        public static bool ComparePassword_BCrypt(this string plainTextPassword, string hashedPassword) =>
            BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
    }
}
