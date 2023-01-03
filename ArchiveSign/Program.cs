namespace ArchiveSign
{
    internal class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error: Target is empty.");
                return -1;
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

            return 0;
        }
    }
}
