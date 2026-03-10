using BCrypt.Net;

namespace AlAshmar.Domain.Commons;

/// <summary>
/// Static singleton service for password hashing and validation.
/// Uses BCrypt for secure password hashing with built-in salting.
/// </summary>
public static class PasswordHasher
{
    /// <summary>
    /// Hashes a password using BCrypt with automatic salt generation.
    /// </summary>
    /// <param name="password">The plain text password to hash.</param>
    /// <returns>A BCrypt hashed password string.</returns>
    public static string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
        }

        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Validates a password against a stored BCrypt hash.
    /// </summary>
    /// <param name="password">The plain text password to validate.</param>
    /// <param name="hash">The stored BCrypt hash.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public static bool Verify(string password, string hash)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
        {
            return false;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}
