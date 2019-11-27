using System;

namespace ProtocolsTest
{
    class Program
    {

        static string conv(float x, int c)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            Array.Reverse(bytes);
            if (c == 0)
                return (BitConverter.ToInt32(bytes, 0).ToString("X8"));
            else
                return (BitConverter.ToSingle(bytes, 0).ToString());
        }

        static void Main(string[] args)
        {
            float floatValue = 30;
            Console.WriteLine(conv(floatValue, 0));
        }

    }
}
