namespace ShadyNagy.Utilities.Crypto.RSA
{
    internal interface IAlgorithm
    {
        byte[] Encrypt(byte[] b, bool isPadding = true);
    }
}
