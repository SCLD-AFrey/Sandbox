using System.Security.Cryptography;
using System.Text;

namespace VikingEnterprise.Server.Services.Host;

public class EncryptionService
{

    private readonly ILogger<EncryptionService> m_logger;
    public EncryptionService(ILogger<EncryptionService> p_logger)
    {
        m_logger = p_logger;
    }
    private const int KeySize = 64;
    private const int Iterations = 350000;
    private readonly HashAlgorithmName m_hashAlgorithm = HashAlgorithmName.SHA512;
    public string GeneratePasswordHash(string p_password, out byte[] p_salt)
    {
        p_salt = RandomNumberGenerator.GetBytes(KeySize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(p_password),
            p_salt,
            Iterations,
            m_hashAlgorithm,
            KeySize);
        return Convert.ToHexString(hash);
    }

    public bool VerifyPassword(string p_password, string p_hash, byte[] p_salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(p_password, p_salt, Iterations, m_hashAlgorithm, KeySize);
        return hashToCompare.SequenceEqual(Convert.FromHexString(p_hash));
    }

}