using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace ArchiveSign
{
    class SignatureManager
    {
        RSAManager rsa;
        readonly string archivePath;

        readonly string Fingerprint;
        readonly byte[] Signature;

        readonly string CreatedAt;
        readonly byte[] CreatedAtSignature;

        public SignatureManager(string archivePath)
        {
            rsa = new RSAManager();
            this.archivePath = archivePath;

            // Get Archive Signature
            byte[] file = File.ReadAllBytes(archivePath);
            byte[] Hash = SHA512.Create().ComputeHash(file);

            Fingerprint = Utils.ByteToHex(Hash);
            Signature = rsa.SignAndGetSignature(Encoding.UTF8.GetBytes(Fingerprint));

            // Set Archive Metadata
            CreatedAt = DateTime.UtcNow.ToString("u").Replace(" ", "T");
            CreatedAtSignature = rsa.SignAndGetSignature(Encoding.UTF8.GetBytes(CreatedAt));
        }

        public void UpdateArchive()
        {
            string outputArchivePath = archivePath.Substring(0, archivePath.Length - 4) + ".pl-archive";

            FileStream SignedArchive = File.Open(outputArchivePath, FileMode.Create);

            using (ZipArchive zipArchive = new ZipArchive(SignedArchive, ZipArchiveMode.Create))
            {
                zipArchive.CreateEntryFromFile(archivePath, "src.zip", CompressionLevel.Fastest);

                ZipArchiveEntry header = zipArchive.CreateEntry("header");

                using (StreamWriter writer = new StreamWriter(header.Open()))
                {
                    writer.WriteLine("fingerprint=" + Fingerprint);
                    writer.WriteLine("signature=" + Convert.ToBase64String(Signature));
                    writer.WriteLine("created_at=" + CreatedAt);
                    writer.WriteLine("created_at_signature=" + Convert.ToBase64String(CreatedAtSignature));
                }
            }

            SignedArchive.Close();
        }
    }
}
