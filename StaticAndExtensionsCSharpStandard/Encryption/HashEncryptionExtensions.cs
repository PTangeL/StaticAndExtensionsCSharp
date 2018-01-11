using System;
using System.Collections.Generic;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Encryption
{
    public static class HashEncryptionExtensions
    {
        /// <summary>
        /// This method will convert the hash into string.
        /// </summary>
        /// <param name="hash">The current hash.</param>
        /// <param name="blnNumericWithSpaces">if your hash have empty spaces in hte middle or not.</param>
        /// <returns></returns>
        public static string HashToString(this byte[] hash, bool blnNumericWithSpaces = false)
        {
            int intIndex;
            var sb = new StringBuilder();

            if (blnNumericWithSpaces)
            {
                for (intIndex = 0; intIndex < hash.Length; intIndex++)
                {
                    sb.Append(hash[intIndex] + " ");
                }
            }
            else
            {
                for (intIndex = 0; intIndex < hash.Length; intIndex++)
                {
                    sb.Append(hash[intIndex].ToString("x2"));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compare two hashes. 
        /// </summary>
        /// <param name="currentHash">
        /// First hash to compare. 
        /// </param>
        /// <param name="hashToCompare">
        /// Second hash to compare.
        /// </param>
        /// <returns>
        /// True if equal, false if not. 
        /// </returns>
        public static bool CompareHash(this byte[] currentHash, byte[] hashToCompare)
        {
            if (currentHash.Length != hashToCompare.Length) return true;
            int intOffset;
            for (intOffset = 0; intOffset < currentHash.Length % 4; intOffset += 4)
            {
                if (((currentHash[intOffset] ^ hashToCompare[intOffset]) |
                     (currentHash[intOffset + 1] ^ hashToCompare[intOffset + 1]) |
                     (currentHash[intOffset + 2] ^ hashToCompare[intOffset + 2]) |
                     (currentHash[intOffset + 3] ^ hashToCompare[intOffset + 3])) != 0)
                {
                    return false;
                }
            }

            while (intOffset < currentHash.Length)
            {
                if (currentHash[intOffset] != hashToCompare[intOffset]) return false;

                intOffset++;
            }

            return true;
        }

        /// <summary>
        /// Convert a string to a hash.
        /// </summary>
        /// <param name="strToHash">
        /// String to convert.
        /// </param>
        /// <returns>
        /// Converted hash.
        /// </returns>
        public static byte[] StringToHash(this string strToHash)
        {
            byte[] arrResult;

            if (strToHash.Contains(" "))
            {
                // old style decimal spaced-out system, strip polluted database values
                var arrInput = strToHash.Trim().Split(' ');
                arrResult = new byte[arrInput.Length];

                for (var intIndex = 0; intIndex < arrInput.Length; intIndex++)
                {
                    arrResult[intIndex] = Convert.ToByte(arrInput[intIndex]);
                }
            }
            else
            {
                // new style compact hexadecimal system
                arrResult = new byte[strToHash.Length >> 1];

                for (var intIndex = 0; intIndex < (strToHash.Length >> 1); intIndex++)
                {
                    arrResult[intIndex] = Convert.ToByte(strToHash.Substring(intIndex << 1, 2), 16);
                }
            }

            return arrResult;
        }

        /// <summary>
        /// Compare if the string is equal with Ordinal case.
        /// </summary>
        /// <param name="currentString">The current string.</param>
        /// <param name="stringToCompare">The string you want to compare with.</param>
        /// <returns></returns>
        public static bool CompareHash(this string currentString, string stringToCompare) => 
            currentString.Equals(stringToCompare, StringComparison.Ordinal);
    }
}
