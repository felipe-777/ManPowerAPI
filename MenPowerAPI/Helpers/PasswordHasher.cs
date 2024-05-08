using System.Security.Cryptography;

namespace MenPowerAPI.Helpers
{
    public static class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        public static string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var hashBytes = new Rfc2898DeriveBytes(password, salt, Iterations).GetBytes(HashSize);

            var hashPassword = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashPassword, 0, SaltSize);
            Array.Copy(hashBytes, 0, hashPassword, SaltSize, HashSize);

            return Convert.ToBase64String(hashPassword);
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hashPassword = Convert.FromBase64String(base64Hash);
            var salt = new byte[SaltSize];
            Array.Copy(hashPassword, 0, salt, 0, SaltSize);

            var hashBytes = new Rfc2898DeriveBytes(password, salt, Iterations).GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashPassword[i + SaltSize] != hashBytes[i])
                    return false;
            }

            return true;
        }
    }
}
