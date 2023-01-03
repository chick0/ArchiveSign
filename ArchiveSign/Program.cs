namespace ArchiveSign
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new Exception("Error: Target is empty.");
            }

            for (int i = 0; i < args.Length; i++)
            {
                string fileName = args[i];

                if (fileName.EndsWith(".zip"))
                {
                    SignatureManager signatureManager = new(fileName);
                    signatureManager.UpdateArchive();

                    Console.WriteLine($"{args[i]}: Archive Signed");
                }
                else if (fileName.EndsWith(".pl-archive"))
                {
                    VerifyManager verifyManager = new(fileName);
                    verifyManager.Verify();

                    string result = verifyManager.Result == true ? "Success" : "Fail";
                    Console.WriteLine($"{args[i]}: Verify {result}");
                }
            }
        }
    }
}
