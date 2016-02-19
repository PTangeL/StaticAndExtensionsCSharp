using System.Text;

namespace Library.Bytes
{
    public static class BytesExntesions
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
        /// Compare two hashes. 
        /// </summary>
        /// <param name="currentHash">
        /// First hash to compare. 
        /// </param>
        /// <param name="arrHash2">
        /// Second hash to compare.
        /// </param>
        /// <returns>
        /// True if equal, false if not. 
        /// </returns>
        public static bool CompareHash(this byte[] currentHash, byte[] hashToCompare)
        {
            int intOffset;

            if (currentHash.Length == hashToCompare.Length)
            {
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
            }

            return true;
        }
    }
}
