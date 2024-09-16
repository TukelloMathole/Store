using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;

namespace MyApp.Helpers
{
    public class PasswordHasher
    {
        // You can use a static salt or better generate a unique salt for each password
        private const int SaltSize = 16; // Size of salt in bytes
        private const int HashSize = 32; // Size of hash in bytes
        private const int Iterations = 10000; // Number of PBKDF2 iterations

        // Method to hash the password
        public string HashPassword(string password, byte[] salt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (salt == null) throw new ArgumentNullException(nameof(salt));

            // Generate the hash
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            // Combine salt and hash into one array
            var saltAndHash = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, saltAndHash, 0, SaltSize);
            Array.Copy(hash, 0, saltAndHash, SaltSize, HashSize);

            return Convert.ToBase64String(saltAndHash);
        }

        // Method to verify the password
        public bool VerifyPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (password == null) throw new ArgumentNullException(nameof(password));

            // Extract the salt from the hashed password
            var saltAndHash = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            var hash = new byte[HashSize];
            Array.Copy(saltAndHash, 0, salt, 0, SaltSize);
            Array.Copy(saltAndHash, SaltSize, hash, 0, HashSize);

            // Generate the hash from the password and salt
            var computedHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            // Check if the computed hash matches the stored hash
            return CryptographicEquals(hash, computedHash);
        }

        // Constant-time comparison to prevent timing attacks
        private static bool CryptographicEquals(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            var isEqual = true;
            for (var i = 0; i < a.Length; i++)
            {
                isEqual &= (a[i] == b[i]);
            }
            return isEqual;
        }

        // Method to generate a random salt
        public byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
