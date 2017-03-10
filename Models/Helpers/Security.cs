using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;

namespace Swertres.Web.Models.Helpers
{
    public class Security
    {
        public static string GenerateSaltedHash(string secret)
        {
            return Crypto.HashPassword(secret);
        }

        public static bool VerifySaltedHash(string hashedValue, string secret)
        {
            return Crypto.VerifyHashedPassword(hashedValue, secret);
        }

        public static string GenerateRandomPassword(int lenght, int numOfAlphaNumericChars)
        {
            return Membership.GeneratePassword(lenght, numOfAlphaNumericChars);
        }

        public static string GenerateControlNumber(long num = 0)
        {
            long generated = 0;
            if (num == 0)
                generated = 1000;
            else
                generated = num + 1;

            return generated.ToString();
        }
    }
}