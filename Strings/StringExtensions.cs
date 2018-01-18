namespace StaticAndExtensionsCSharp.Strings
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net.Mail;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// This class have all the extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Used in Hashing / Security section.
        /// </summary>
        private const string ConstantSalt = "SecureSalt123@";

        /// <summary>
        /// Used for Hashing Security Factor in BCrypt encryption.
        /// </summary>
        private const int ConstantBCryptSecurityFactor = 10;

        /// <summary>
        /// Add quotes to the current string.
        /// </summary>
        /// <param name="item">Current string.</param>
        /// <returns></returns>
        public static string AddQuotes(this string item) => '"' + item + '"';

        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }

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

        #region [ Rebuilding Strings ]

        /// <summary>
        /// Remove from the end of the string how many chars you want
        /// </summary>
        /// <param name="instr">The current string.</param>
        /// <param name="number">How many you want to remove from the end.</param>
        /// <returns></returns>
        public static string RemoveLast(this string instr, int number = 1) => instr.Substring(0, instr.Length - number);

        /// <summary>
        /// Remove from the start of the string how many chars you want
        /// </summary>
        /// <param name="instr">The current string.</param>
        /// <param name="number">How many you want to remove from the start.</param>
        /// <returns></returns>
        public static string RemoveFirst(this string instr, int number = 1) => instr.Substring(number);

        #endregion [ Rebuilding Strings ]

        #region [ Hashing / Security ]

        #region [ SHA-1 ]
        /// <summary>
        /// Create a hash based on the input string.
        /// </summary>
        /// <param name="strToSHA1">
        /// String to hash.
        /// </param>
        /// <returns>
        /// Hash for this string.
        /// </returns>
        public static byte[] GetHash_SHA1(this string strToSHA1) => new SHA1Managed().ComputeHash(new UnicodeEncoding().GetBytes(strToSHA1));

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
        public static byte[] GetSecureHash_SHA1(this string strToHash, string salt = ConstantSalt) => GetHash_SHA1(strToHash + salt);
        #endregion [ SHA-1 ]

        #region [ Bcrypt ]
        
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
            BCrypt.Net.BCrypt.CheckPassword(plainTextPassword, hashedPassword);
        #endregion [ Bcrypt ]

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
            string[] arrInput;
            byte[] arrResult;

            if (strToHash.Contains(" "))
            {
                // old style decimal spaced-out system, strip polluted database values
                arrInput = strToHash.Trim().Split(' ');
                arrResult = new byte[arrInput.Length];

                for (int intIndex = 0; intIndex < arrInput.Length; intIndex++)
                {
                    arrResult[intIndex] = Convert.ToByte(arrInput[intIndex]);
                }
            }
            else
            {
                // new style compact hexadecimal system
                arrResult = new byte[strToHash.Length >> 1];

                for (int intIndex = 0; intIndex < (strToHash.Length >> 1); intIndex++)
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
        public static bool CompareHash(this string currentString, string stringToCompare) => currentString.Equals(stringToCompare, StringComparison.Ordinal);

        #endregion [ Hashing / Security ]

        #region [ Reverse ]

        /// <summary>
        /// Reverse a String
        /// </summary>
        /// <param name="input">The string to Reverse</param>
        /// <returns>The reversed String</returns>
        public static string Reverse(this string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        #endregion [ Reverse ]

        #region [ XmlSerialize XmlDeserialize ]

        /// <summary>Serialises an object of type T in to an xml string</summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="objectToSerialise">Object to serialise</param>
        /// <returns>A string that represents Xml, empty oterwise</returns>
        public static string XmlSerialize<T>(this T objectToSerialise) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            string xml;
            using (var memStream = new MemoryStream())
            {
                using (var xmlWriter = new XmlTextWriter(memStream, Encoding.UTF8))
                {
                    serialiser.Serialize(xmlWriter, objectToSerialise);
                    xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                }
            }

            // ascii 60 = '<' and ascii 62 = '>'
            xml = xml.Substring(xml.IndexOf('<'));
            xml = xml.Substring(0, (xml.LastIndexOf('>') + 1));
            return xml;
        }

        /// <summary>Deserialises an xml string in to an object of Type T</summary>
        /// <typeparam name="T">Any class type</typeparam>
        /// <param name="xml">Xml as string to deserialise from</param>
        /// <returns>A new object of type T is successful, null if failed</returns>
        public static T XmlDeserialize<T>(this string xml) where T : class
        {
            var serialiser = new XmlSerializer(typeof(T));
            T newObject;

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = new XmlTextReader(stringReader))
                {
                    try
                    {
                        newObject = serialiser.Deserialize(xmlReader) as T;
                    }
                    catch (InvalidOperationException) // String passed is not Xml, return null
                    {
                        return null;
                    }
                }
            }

            return newObject;
        }

        #endregion [ XmlSerialize XmlDeserialize ]

        #region [ To X conversions ]

        /// <summary>
        /// Parses a string into an Enum
        /// </summary>
        /// <typeparam name="T">The type of the Enum</typeparam>
        /// <param name="value">String value to parse</param>
        /// <param name="ignorecase">Ignore the case of the string being parsed</param>
        /// <returns>The Enum corresponding to the stringExtensions</returns>
        public static T ToEnum<T>(this string value, bool ignorecase = false) where T : struct
        {
            if (string.IsNullOrEmpty(value))
                return default(T);

            value = value.Trim();

            var t = typeof(T);
            if (!t.IsEnum)
                throw new ArgumentException("Type provided must be an Enum.", "T");

            Enum.TryParse(value, ignorecase, out T result);

            return result;
        }

        /// <summary>
        /// Toes the integer.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static int ToInteger(this string value, int defaultvalue = 0) => (int)ToDouble(value, defaultvalue);

        /// <summary>
        /// Toes the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static double ToDouble(this string value, double defaultvalue = 0)
        {
            double result;
            if (double.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Toes the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultvalue">The defaultvalue.</param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultvalue = null)
        {
            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }
            else return defaultvalue;
        }

        /// <summary>
        /// Converts a string value to bool value, supports "T" and "F" conversions.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns>A bool based on the string value</returns>
        public static bool? ToBoolean(this string value)
        {
            if (string.Compare("T", value, true) == 0)
            {
                return true;
            }
            if (string.Compare("F", value, true) == 0)
            {
                return false;
            }
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            else return null;
        }

        #endregion [ To X conversions ]

        #region [ Encrypt Decrypt ]

        /// <summary>
        /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
        /// </summary>
        /// <param name="stringToEncrypt">String that must be encrypted.</param>
        /// <param name="key">Encryptionkey.</param>
        /// <returns>A string representing a byte array separated by a minus sign.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
        public static string Encrypt(this string stringToEncrypt, string key)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
            }

            CspParameters cspp = new CspParameters();
            cspp.KeyContainerName = key;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;

            byte[] bytes = rsa.Encrypt(Encoding.UTF8.GetBytes(stringToEncrypt), true);

            return BitConverter.ToString(bytes);
        }

        /// <summary>
        /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
        /// </summary>
        /// <param name="key">Decryptionkey.</param>
        /// <returns>The decrypted string or null if decryption failed.</returns>
        /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
        public static string Decrypt(this string stringToDecrypt, string key)
        {
            string result = null;

            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                throw new ArgumentException("An empty string value cannot be encrypted.");
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
            }

            try
            {
                CspParameters cspp = new CspParameters();
                cspp.KeyContainerName = key;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
                rsa.PersistKeyInCsp = true;

                string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
                byte[] decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));

                byte[] bytes = rsa.Decrypt(decryptByteArray, true);

                result = Encoding.UTF8.GetString(bytes);
            }
            finally
            {
                // no need for further processing
            }

            return result;
        }

        #endregion [ Encrypt Decrypt ]

        #region [ IsValidUrl ]

        /// <summary>
        /// Determines whether it is a valid URL.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidUrl(this string text) => new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?").IsMatch(text);

        #endregion [ IsValidUrl ]

        #region [ IsValidEmailAddress ]

        /// <summary>
        /// Determines whether it is a valid email address
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidEmailAddress(this string email)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(email);
        }

        #endregion [ IsValidEmailAddress ]

        #region [ Email ]

        /// <summary>
        /// Send an email using the supplied string.
        /// </summary>
        /// <param name="body">String that will be used i the body of the email.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="sender">The email address from which the message was sent.</param>
        /// <param name="recipient">The receiver of the email.</param>
        /// <param name="server">The server from which the email will be sent.</param>
        /// <returns>A boolean value indicating the success of the email send.</returns>
        public static bool Email(this string body, string subject, string sender, string recipient, string server)
        {
            try
            {
                // To
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(recipient);

                // From
                MailAddress mailAddress = new MailAddress(sender);
                mailMsg.From = mailAddress;

                // Subject and Body
                mailMsg.Subject = subject;
                mailMsg.Body = body;

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient(server);
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not send mail from: " + sender + " to: " + recipient + " thru smtp server: " + server + "\n\n" + ex.Message, ex);
            }

            return true;
        }

        #endregion [ Email ]

        #region [ Truncate ]

        /// <summary>
        /// Truncates the string to a specified length and replace the truncated to a ...
        /// </summary>
        /// <param name="maxLength">total length of characters to maintain before the truncate happens</param>
        /// <returns>truncated string</returns>
        public static string Truncate(this string text, int maxLength)
        {
            // replaces the truncated string to a ...
            const string suffix = "...";
            string truncatedString = text;

            if (maxLength <= 0) return truncatedString;
            int strLength = maxLength - suffix.Length;

            if (strLength <= 0) return truncatedString;

            if (text == null || text.Length <= maxLength) return truncatedString;

            truncatedString = text.Substring(0, strLength);
            truncatedString = truncatedString.TrimEnd();
            truncatedString += suffix;
            return truncatedString;
        }

        #endregion [ Truncate ]

        #region [ HTMLHelper ]

        /// <summary>
        /// Converts to a HTML-encoded string
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static string HtmlEncode(this string data) => HttpUtility.HtmlEncode(data);

        /// <summary>
        /// Converts the HTML-encoded string into a decoded string
        /// </summary>
        public static string HtmlDecode(this string data) => HttpUtility.HtmlDecode(data);

        /// <summary>
        /// Parses a query string into a System.Collections.Specialized.NameValueCollection
        /// using System.Text.Encoding.UTF8 encoding.
        /// </summary>
        public static NameValueCollection ParseQueryString(this string query) => HttpUtility.ParseQueryString(query);

        /// <summary>
        /// Encode an Url string
        /// </summary>
        public static string UrlEncode(this string url) => HttpUtility.UrlEncode(url);

        /// <summary>
        /// Converts a string that has been encoded for transmission in a URL into a
        /// decoded string.
        /// </summary>
        public static string UrlDecode(this string url) => HttpUtility.UrlDecode(url);

        /// <summary>
        /// Encodes the path portion of a URL string for reliable HTTP transmission from
        /// the Web server to a client.
        /// </summary>
        public static string UrlPathEncode(this string url) => HttpUtility.UrlPathEncode(url);

        #endregion [ HTMLHelper ]
    }
}