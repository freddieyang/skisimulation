using System;
using System.IO.Ports;

namespace ProtocolsTest
{
    class Program
    {
        public static byte[] dataSend = new byte[32];
        static Int32 conv(float x, int c)
        {
            byte[] bytes = BitConverter.GetBytes(x);
            Array.Reverse(bytes);
            if (c == 0)
                return BitConverter.ToInt32(bytes, 0);
            else
                return 0;
            //return (BitConverter.ToSingle(bytes, 0).ToString());
        }

        static void Main(string[] args)
        {
            //float floatValue = 100;
            //Console.WriteLine(conv(floatValue, 0));
            set_protocal();
            display();
        }


        static void display()
        {
            var hex = BitConverter.ToString(dataSend, 0).Replace("-", string.Empty).ToUpper();
            Console.WriteLine(hex);
        }

        static void set_protocal()
        {
            byte[] x = BitConverter.GetBytes(0);
            byte[] y = BitConverter.GetBytes(0);
            byte[] z = BitConverter.GetBytes(0);
            byte[] alpha = BitConverter.GetBytes(0);
            byte[] beta = BitConverter.GetBytes(0);
            byte[] gamma = BitConverter.GetBytes(30);

            dataSend[0] = 0xFB;
            dataSend[1] = 0xFD;
            for (int i = 2; i < 31; i++)
            {
                dataSend[i] = 0x0;
            }
            for (int i = 0; i < 4; i++)
            {
                dataSend[14 + i] = alpha[i];
            }
            for (int i = 0; i < 4; i++)
            {
                dataSend[18 + i] = beta[i];
            }
            for (int i = 0; i < 4; i++)
            {
                dataSend[22 + i] = gamma[i];
            }
            for (int i = 26; i < 31; i++)
            {
                dataSend[i] = 0x0;
            }
            int crc = 0;
            for (int i = 1; i < 31; i++)
            {
                crc += dataSend[i];
            }
            crc = crc % 256;
            dataSend[31] = (byte)crc;
        }

    }
}
