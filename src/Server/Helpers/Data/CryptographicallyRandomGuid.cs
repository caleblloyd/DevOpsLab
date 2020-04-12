using System;
using System.Security.Cryptography;

namespace DevOpsLab.Server.Helpers.Data
{
    public static class CryptographicallyRandomGuid
    {
        private static readonly RNGCryptoServiceProvider Rng = new RNGCryptoServiceProvider();

        public static Guid Next()
        {
            var randomBytes = new byte[16];
            Rng.GetBytes(randomBytes);
            return new Guid(randomBytes);
        }
    }
}
