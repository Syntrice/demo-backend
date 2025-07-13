namespace DemoBackend.Settings;

public class PasswordHashingSettings
{
    public int SaltSize { get; set; } = 16; // recommended size 128 bits, so 16 bytes
    public int HashSize { get; set; } = 32; // recommended size 256 bits, so 32 bytes
    public int Iterations { get; set; } = 100000; // Number of iterations to run the hash function
    public string Algorithm { get; set; } = "SHA512";
}