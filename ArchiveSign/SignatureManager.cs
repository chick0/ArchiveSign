using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace ArchiveSign
{
    class SignatureManager
    {
        RSAManager rsa;
        string archivePath;
        string? Signature;

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
            byte[] EncryptedDigest = rsa.Encrypt(GetDigest());
            Signature = Convert.ToBase64String(EncryptedDigest);
            return Signature;
        }

        public void UpdateArchive()
        {
            if (Signature == null)
            {
                throw new Exception("You must call GetSignature function first");
            }

            string outputArchivePath = archivePath.Substring(0, archivePath.Length - 4) + ".pl-archive";

            FileStream SignedArchive = File.Open(outputArchivePath, FileMode.Create);

            using (ZipArchive zipArchive = new ZipArchive(SignedArchive, ZipArchiveMode.Create))
            {
                zipArchive.CreateEntryFromFile(archivePath, "src.zip", CompressionLevel.Fastest);

                ZipArchiveEntry header = zipArchive.CreateEntry("header");

                using (StreamWriter writer = new StreamWriter(header.Open()))
                {
                    string CreatetAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:s");

                    writer.WriteLine("signature=" + Signature);
                    writer.WriteLine("created_at=" + CreatetAt);
                    writer.WriteLine("created_at_sig=" + Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(CreatetAt))));
                }
            }

            SignedArchive.Close();
        }
    }
}
