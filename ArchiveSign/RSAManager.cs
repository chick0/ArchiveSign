using System.Security.Cryptography;

namespace ArchiveSign
{
    class RSAManager
    {
        KeyPair keyPair;
        RSACryptoServiceProvider RSA;

        public RSAManager()
        {
            keyPair = KeyManager.GetKeyPair();

            RSA = new RSACryptoServiceProvider();
            RSA.ImportFromPem(keyPair.Private);
            RSA.ImportFromPem(keyPair.Public);
        }

        public byte[] Encrypt(byte[] data)
        {
            return RSA.Encrypt(data, false);
        }

        public byte[] Decrypt(byte[] data)
        {
            return RSA.Decrypt(data, false);
        }
    }
}
