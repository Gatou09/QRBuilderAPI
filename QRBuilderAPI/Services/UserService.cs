using QRBuilderAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace QRBuilderAPI.Services;

public class UserService
{
    private readonly List<User> _users = new();

    public UserService()
    {
        // Compte test
        Register("test@qr.io", "Test123!");
    }

    public bool Register(string email, string password)
    {
        if (_users.Any(u => u.Email == email))
            return false;

        CreatePasswordHash(password, out byte[] hash, out byte[] salt);

        var user = new User
        {
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _users.Add(user);
        return true;
    }

    public User? Authenticate(string email, string password)
    {
        var user = _users.FirstOrDefault(u => u.Email == email);
        if (user == null) return null;

        return VerifyPassword(password, user.PasswordHash, user.PasswordSalt) ? user : null;
    }

    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(hash);
    }
}