using System.Security.Cryptography;

namespace ArchiveSign
{
    class KeyPair
    {
        private readonly string? PrivateKey;
        private readonly string? PublicKey;

        public string Private
        {
            get
            {
                if (string.IsNullOrEmpty(PrivateKey))
                {
                    throw new Exception("Private Key is empty!");
                }

                return PrivateKey;
            }
        }

        public string Public
        {
            get
            {
                if (string.IsNullOrEmpty(PublicKey))
                {
                    throw new Exception("Public Key is empty!");
                }

                return PublicKey;
            }
        }

        public KeyPair(string? Private, string? Public)
        {
            PrivateKey = Private;
            PublicKey = Public;
        }
    }

    class KeyManager
    {
        readonly static string CurrentDirectory = Directory.GetCurrentDirectory();
        readonly static string PrivatePath = Path.Join(CurrentDirectory, "private.pem");
        readonly static string PublicPath = Path.Join(CurrentDirectory, "public.pem");

        private static void WriteKeyPair(string PrivateKey, string PublicKey)
        {
            File.WriteAllText(PrivatePath, PrivateKey);
            File.WriteAllText(PublicPath, PublicKey);
        }

        private static KeyPair CreateKeyPair()
        {
            RSA key = RSA.Create(2048);

            string PrivateKey = key.ExportRSAPrivateKeyPem();
            string PublicKey = key.ExportRSAPublicKeyPem();

            WriteKeyPair(PrivateKey, PublicKey);
            return new KeyPair(PrivateKey, PublicKey);
        }

        private static KeyPair ReadKeyPair()
        {
            string? PrivateKey;
            string? PublicKey;

            try
            {
                PrivateKey = File.OpenText(PrivatePath).ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                PrivateKey = null;
            }

            try
            {
                PublicKey = File.OpenText(PublicPath).ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                PublicKey = null;
            }

            if ((PrivateKey == null) && (PublicKey == null))
            {
                return CreateKeyPair();
            }

            return new KeyPair(PrivateKey, PublicKey);
        }

        public static KeyPair GetKeyPair()
        {
            KeyPair pair = ReadKeyPair();
            return pair;
        }
    }
}
