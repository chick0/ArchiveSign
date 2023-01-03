using System.Security.Cryptography;

namespace ArchiveSign
{
    class RSAManager
    {
        private readonly KeyPair keyPair;

        public RSAManager()
        {
            keyPair = KeyManager.GetKeyPair();
        }

        public byte[] SignAndGetSignature(byte[] data)
        {
            var RSA = new RSACryptoServiceProvider();
            RSA.ImportFromPem(keyPair.Private);

            return RSA.SignData(data, SHA512.Create());
        }

        public bool GetVerifyResult(byte[] data, byte[] signature)
        {
            var RSA = new RSACryptoServiceProvider();
            RSA.ImportFromPem(keyPair.Public);

            return RSA.VerifyData(data, SHA512.Create(), signature);
        }
    }
}
