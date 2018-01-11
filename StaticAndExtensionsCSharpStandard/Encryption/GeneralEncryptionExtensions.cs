using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace StaticAndExtensionsCSharpStandard.Encryption
{
    public static class GeneralEncryptionExtensions
    {
        /// <summary>
        /// Converts a string into a "SecureString"
        /// </summary>
        /// <param name="str">Input String</param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string str)
        {
            SecureString secureString = new SecureString();
            foreach (char c in str)
                secureString.AppendChar(c);

            return secureString;
        }
    }
}
