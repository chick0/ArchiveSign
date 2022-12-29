using System.Security.Cryptography;

namespace ArchiveSign
{
    class KeyPair
    {
        public string Private;
        public string Public;

        public KeyPair(string Private, string Public)
        {
            this.Private = Private;
            this.Public = Public;
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

        private static KeyPair? ReadKeyPair()
        {
            try
            {
                string PrivateKey = File.OpenText(PrivatePath).ReadToEnd();
                string PublicKey = File.OpenText(PublicPath).ReadToEnd();

                return new KeyPair(PrivateKey, PublicKey);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public static KeyPair GetKeyPair()
        {
            KeyPair? pair = ReadKeyPair();
            pair ??= CreateKeyPair();

            return pair;
        }
    }
}
