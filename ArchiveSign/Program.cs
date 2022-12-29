using System.Security.Cryptography;

namespace ArchiveSign
{
    internal class Program
    {
        readonly static string CurrentDirectory = Directory.GetCurrentDirectory();
        readonly static string PrivatePath = Path.Join(CurrentDirectory, "private.pem");
        readonly static string PublicPath = Path.Join(CurrentDirectory, "public.pem");

        static void Main(string[] args)
        {
            string? PrivateKey;
            string? PublicKey;

            try
            {
                PrivateKey = File.OpenText(PrivatePath).ReadToEnd();
                PublicKey = File.OpenText(PublicPath).ReadToEnd();
            }
            catch (FileNotFoundException)
            {
                RSA key = RSA.Create(2048);

                PrivateKey = key.ExportRSAPrivateKeyPem();
                PublicKey = key.ExportRSAPublicKeyPem();

                File.WriteAllText(PrivatePath, PrivateKey);
                File.WriteAllText(PublicPath, PublicKey);
            }

            Console.WriteLine(PrivateKey);
            Console.WriteLine(PublicKey);
        }
    }
}