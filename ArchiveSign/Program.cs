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
                    Console.WriteLine(args[i] + ": " + signatureManager.GetSignature());
                }
            }

            return 0;
        }
    }
}