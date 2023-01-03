using System.Text;

namespace ArchiveSign
{
    class Utils
    {
        public static string ByteToHex(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }

        public static string HexFromByte(byte[] hexData)
        {
            return Encoding.UTF8.GetString(hexData);
        }

        public static string ReadStreamToString(Stream stream)
        {
            string Result = "";

            while (true)
            {
                int raw = stream.ReadByte();

                if (raw == -1)
                {
                    break;
                }
                else
                {
                    Result += Convert.ToChar(raw);
                }
            }

            return Result;
        }
    }
}
