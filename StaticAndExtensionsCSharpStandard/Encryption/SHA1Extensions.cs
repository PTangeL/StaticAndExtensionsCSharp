using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Encryption
{
    public static class Sha1Extensions
    {
        /// <summary>
        /// Used in Hashing / Security section.
        /// </summary>
        private const string ConstantSalt = "SecureSalt123@";

        /// <summary>
        /// Create a hash based on the input string.
        /// </summary>
        /// <param name="strToSha1">
        /// String to hash.
        /// </param>
        /// <returns>
        /// Hash for this string.
        /// </returns>
        public static byte[] GetHash_SHA1(this string strToSha1) => new SHA1Managed().ComputeHash(new UnicodeEncoding().GetBytes(strToSha1));

        /// <summary>
        /// Create a secure ehash based on the input string.
        /// </summary>
        /// <param name="strToHash">
        /// String to hash.
        /// </param>
        /// <param name="salt">The salt you wanna use, normally should be a constant in your code + another from your DB.</param>
        /// <returns>
        /// Secure hash for this string.
        /// </returns>
        public static byte[] GetSecureHash_SHA1(this string strToHash, string salt = ConstantSalt) => 
            GetHash_SHA1(strToHash + salt);
    }
}
