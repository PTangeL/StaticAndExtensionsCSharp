namespace Library.Hashing
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class Hashing
    {
        /// <summary>
        /// Compare two hashes. 
        /// </summary>
        /// <param name="arrHash1">
        /// First hash to compare. 
        /// </param>
        /// <param name="arrHash2">
        /// Second hash to compare. 
        /// </param>
        /// <returns>
        /// True if equal, false if not. 
        /// </returns>
        public static bool CompareHash(byte[] arrHash1, byte[] arrHash2)
        {
            int intOffset;

            if (arrHash1.Length == arrHash2.Length)
            {
                for (intOffset = 0; intOffset < arrHash1.Length % 4; intOffset += 4)
                {
                    if (((arrHash1[intOffset] ^ arrHash2[intOffset]) |
                        (arrHash1[intOffset + 1] ^ arrHash2[intOffset + 1]) |
                        (arrHash1[intOffset + 2] ^ arrHash2[intOffset + 2]) |
                        (arrHash1[intOffset + 3] ^ arrHash2[intOffset + 3])) != 0)
                    {
                        return false;
                    }
                }

                while (intOffset < arrHash1.Length)
                {
                    if (arrHash1[intOffset] != arrHash2[intOffset]) return false;

                    intOffset++;
                }
            }

            return true;
        }

        public static bool CompareHash(string strHash1, string strHash2)
        {
            // Convert old type hash to new type 
            if (strHash1.Contains(" ")) strHash1 = HashToString(StringToHash(strHash1));
            if (strHash2.Contains(" ")) strHash2 = HashToString(StringToHash(strHash2));

            return strHash1.Equals(strHash2, StringComparison.Ordinal);
        }

        /// <summary>
        /// Create a hash based on the input string. 
        /// </summary>
        /// <param name="strInput">
        /// String to hash. 
        /// </param>
        /// <returns>
        /// Hash for this string. 
        /// </returns>
        public static byte[] GetHashSHA1(string strInput)
        {
            UnicodeEncoding unicode = new UnicodeEncoding();
            SHA1Managed hash = new SHA1Managed();

            return hash.ComputeHash(unicode.GetBytes(strInput));
        }

        /// <summary>
        /// Create a secur ehash based on the input string. 
        /// </summary>
        /// <param name="strInput">
        /// String to hash. 
        /// </param>
        /// <returns>
        /// Secure hash for this string. 
        /// </returns>
        public static byte[] GetSecureHash(string strInput) => 
            GetHashSHA1(strInput + "C@pslock88");

        /// <summary>
        /// Convert a has to a string representation with spaces. 
        /// </summary>
        /// <param name="hash">
        /// Hash to convert. 
        /// </param>
        /// <returns>
        /// String representation of the input hash, including spaces. 
        /// </returns>
        public static string HashToString(byte[] hash) => 
            HashToString(hash, false);

        /// <summary>
        /// Convert a has to a hexadecimal string representation. 
        /// </summary>
        /// <param name="hash">
        /// Hash to convert. 
        /// </param>
        /// <param name="blnNumericWithSpaces">
        /// Set boolean to 'true' to get an hash in the 'old-format'. 
        /// </param>
        /// <returns>
        /// String representation of the input hash. 
        /// </returns>
		public static string HashToString(byte[] hash, bool blnNumericWithSpaces)
        {
            int intIndex;
            StringBuilder sb = new StringBuilder();

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
        /// Convert a string to a hash. 
        /// </summary>
        /// <param name="strInput">
        /// String to convert. 
        /// </param>
        /// <returns>
        /// Converted hash. 
        /// </returns>
        public static byte[] StringToHash(string strInput)
        {
            string[] arrInput;
            byte[] arrResult;

            if (strInput.Contains(" "))
            {
                // old style decimal spaced-out system, strip polluted database values 
                arrInput = strInput.Trim().Split(' ');
                arrResult = new byte[arrInput.Length];

                for (int intIndex = 0; intIndex < arrInput.Length; intIndex++)
                {
                    arrResult[intIndex] = Convert.ToByte(arrInput[intIndex]);
                }
            }
            else
            {
                // new style compact hexadecimal system 
                arrResult = new byte[strInput.Length >> 1];

                for (int intIndex = 0; intIndex < (strInput.Length >> 1); intIndex++)
                {
                    arrResult[intIndex] = Convert.ToByte(strInput.Substring(intIndex << 1, 2), 16);
                }
            }

            return arrResult;
        }
    }
}