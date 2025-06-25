using System.Security.Cryptography;

namespace BankApp.Abstractions;

/// <summary>
/// Provides secure password hashing and verification functionality using PBKDF2.
/// </summary>
/// <remarks>
/// This utility class implements secure password hashing using the PBKDF2 (Password-Based
/// Key Derivation Function 2) algorithm with SHA256. It includes salt generation and
/// verification capabilities for secure password storage and authentication.
/// </remarks>
public class PasswordHasher
{
    /// <summary>
    /// Hashes a password using PBKDF2 with a random salt.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A base64-encoded string containing the salt and hash.</returns>
    /// <remarks>
    /// This method generates a cryptographically secure random salt and uses PBKDF2
    /// with 100,000 iterations to create a secure hash. The salt and hash are combined
    /// and returned as a base64-encoded string for storage.
    /// </remarks>
    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Hash the password
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000))
        {
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert to base64 string
            return Convert.ToBase64String(hashBytes);
        }
    }

    /// <summary>
    /// Verifies a password against a stored hash.
    /// </summary>
    /// <param name="password">The plain text password to verify.</param>
    /// <param name="hashedPassword">The stored hash to verify against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    /// <remarks>
    /// This method extracts the salt from the stored hash, applies the same PBKDF2
    /// algorithm to the provided password, and compares the resulting hash with
    /// the stored hash. Returns true only if they match exactly.
    /// </remarks>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Convert from base64 string
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Get the salt
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        // Hash the password
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000))
        {
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the hashes
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }
        }

        return true;
    }
}