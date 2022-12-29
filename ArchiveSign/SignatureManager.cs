using System.Security.Cryptography;

namespace ArchiveSign
{
    class SignatureManager
    {
        RSAManager rsa;
        string archivePath;

        public SignatureManager(string archivePath)
        {
            rsa = new RSAManager();
            this.archivePath = archivePath;
        }

        byte[] GetDigest()
        {
            byte[] file = File.ReadAllBytes(archivePath);
            byte[] hash = SHA512.Create().ComputeHash(file);

            return hash;
        }

        public string GetSignature()
        {
            byte[] signature = rsa.Encrypt(GetDigest());
            return Convert.ToBase64String(signature);
        }
    }
}
