using System;
using System.Security.Cryptography;
using System.Text;

namespace HealthyJuices.Domain.Models.Users
{
    public static class PasswordHelper
    {
        public static string GenerateSalt()
        {
            var data = new byte[0x10];

            RandomNumberGenerator.Create().GetBytes(data);

            return Convert.ToBase64String(data);
        }

        public static string HashPassword(string password, string salt)
        {
            var bytes = Encoding.Unicode.GetBytes(password);
            var src = Convert.FromBase64String(salt);
            var dst = new byte[src.Length + bytes.Length];

            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);

            var algorithm = SHA256.Create();
            var inArray = algorithm.ComputeHash(dst);

            return Convert.ToBase64String(inArray);
        }
    }
}
