using System;
using System.Globalization;
using ShadyNagy.Utilities.Crypto.Hash;

namespace ShadyNagy.Utilities.Crypto.Random
{
    public class RandomManager
    {
        public static string CreateRandomString()
        {
            return Sha1.HashToString(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }
    }
}
