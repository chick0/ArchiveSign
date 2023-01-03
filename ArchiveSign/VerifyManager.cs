using System.IO.Compression;
using System.Text;

namespace ArchiveSign
{
    class ArchiveHead
    {

        public byte[] Fingerprint { get; }
        public byte[] Signature { get; }
        public DateTime CreatedAt { get; }
        public byte[] RawCreatedAt { get; }
        public byte[] CreatedAtSignature { get; }

        public ArchiveHead(string fingerprint, string signature, string CreatedAt, string CreatedAtSignature)
        {
            try
            {
                Fingerprint = Encoding.UTF8.GetBytes(fingerprint);
                Signature = Convert.FromBase64String(signature);

                this.CreatedAt = DateTime.Parse(CreatedAt);

                if (this.CreatedAt >= DateTime.Now)
                {
                    throw new Exception("Archive created at is future or present.");
                }

                RawCreatedAt = Encoding.UTF8.GetBytes(CreatedAt);
                this.CreatedAtSignature = Convert.FromBase64String(CreatedAtSignature);
            }
            catch (EncoderFallbackException)
            {
                throw new Exception("This Archive has invaild header.");
            }
        }
    }

    class VerifyManager
    {
        private readonly string archivePath;
        private bool? verifyResult;

        private readonly RSAManager rsaManager;
        private readonly TempfileManager tmp;
        private ArchiveHead? Head;

        public VerifyManager(string archivePath)
        {
            rsaManager = new RSAManager();

            this.archivePath = archivePath;
            tmp = new TempfileManager();
        }

        public bool Result
        {
            get
            {
                if (verifyResult == null)
                {
                    throw new Exception("This archive is not verified.");
                }

                return (bool)verifyResult;
            }
        }

        void ParseHeader(string RawHeader)
        {
            Dictionary<string, string> RawHeaderStore = new();

            foreach (string Line in RawHeader.Split("\n"))
            {
                if (Line.Trim().Length == 0)
                {
                    continue;
                }

                int index = Line.IndexOf("=");

                if (index == -1)
                {
                    throw new Exception("Archive Header is broken.");
                }

                string key = Line.Substring(0, index);
                string value = Line.Substring(index + 1).Trim();

                RawHeaderStore[key] = value;
            }

            Head = new ArchiveHead(
                RawHeaderStore["fingerprint"],
                RawHeaderStore["signature"],
                RawHeaderStore["created_at"],
                RawHeaderStore["created_at_signature"]
            );
        }

        void UnzipToTemp()
        {
            using (ZipArchive zip = ZipFile.Open(archivePath, ZipArchiveMode.Read))
            {
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    if (entry.Name == "header")
                    {
                        string RawHeader = "";

                        using (Stream HeaderStream = entry.Open())
                        {
                            while (true)
                            {
                                int raw = HeaderStream.ReadByte();

                                if (raw == -1)
                                {
                                    break;
                                }
                                else
                                {
                                    RawHeader += Convert.ToChar(raw);
                                }
                            }
                        }

                        ParseHeader(RawHeader);
                    }
                    else if (entry.Name == "src.zip")
                    {
                        tmp.Clear();
                        entry.ExtractToFile(tmp.Path);
                    }
                }
            }
        }

        public void Verify()
        {
            UnzipToTemp();

            if (Head == null)
            {
                throw new Exception("Archive Header is empty");
            }

            bool FingerprintVerify = rsaManager.GetVerifyResult(Head.Fingerprint, Head.Signature);
            bool CreatedVerify = rsaManager.GetVerifyResult(Head.RawCreatedAt, Head.CreatedAtSignature);

            // Verify Result
            verifyResult = (FingerprintVerify == true) && (CreatedVerify == true);
        }
    }
}
