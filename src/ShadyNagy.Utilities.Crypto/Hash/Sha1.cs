using System;
using System.Security.Cryptography;
using System.Text;

namespace ShadyNagy.Utilities.Crypto.Hash
{
    public class Sha1
    {
        public static string HashToString(string input, string secret = null)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input + secret));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public static byte[] HashToBytes(string input, string secret = null)
        {
            using (var sha1 = new SHA1Managed())
            {
                return sha1.ComputeHash(Encoding.UTF8.GetBytes(input + secret));
            }
        }

        public static string HashToBase64(string input, string secret = null)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hashed = sha1.ComputeHash(Encoding.UTF8.GetBytes(input + secret));

                return Convert.ToBase64String(hashed);
            }
        }
    }
}
