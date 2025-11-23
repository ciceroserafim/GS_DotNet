using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SkillUp.Api.Utils
{
    public static class HashingUtils
    {
        public static string HashPassword(string password)
        {
            // PBKDF2 simple example — in production use a proven lib (BCrypt/Argon2)
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(salt);

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // store salt + hash
            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        public static bool VerifyPassword(string password, string stored)
        {
            if (string.IsNullOrEmpty(stored)) return false;
            var parts = stored.Split('.', 2);
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var expected = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(expected), Convert.FromBase64String(hashed));
        }
    }
}
