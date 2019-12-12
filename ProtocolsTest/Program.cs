using System;
using System.IO.Ports;

namespace ProtocolsTest
{
    class Program
    {
        public static string ReceiveMsg = "";
        public static string ErrorMsg = "";
        public static string SendMsg = "";
        public static string portName = "COM4";
        public static int baudRate = 115200;
        private static Parity parity = Parity.None;
        private static int dataBits = 8;
        private static StopBits stopBits = StopBits.One;
        private static SerialPort sp = null;
        public static byte[] dataSend = new byte[32];

        static void Main(string[] args)
        {
            //float floatValue = 100;
            //Console.WriteLine(conv(floatValue, 0));
            float x, y, z, alpha, beta, gamma;
            x = 0;
            y = 0;
            z = 0;
            alpha = 0;
            beta = 0;
            gamma = 0;
            StartPortOpen();
            set_protocal(x, y, z, alpha, beta, gamma);
            display();
            WriteData(dataSend);
        }

        static void StartPortOpen()
        {
            if (sp != null && sp.IsOpen)
            {
                ClosePort();
            }
            else
            {
                OpenPort();
                Console.WriteLine("Open successful");
            }
            
        }

        static void ClosePort()
        {
            try
            {
                if (sp != null)
                    sp.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void OpenPort()
        {
            sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            sp.ReadTimeout = 400;
            try
            {
                sp.Open();
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                Console.WriteLine(ex.Message);
            }
        }

        static void display()
        {
            var hex = BitConverter.ToString(dataSend, 0).Replace("-", string.Empty).ToUpper();
            Console.WriteLine(hex);
        }

        static void set_protocal(float fx, float fy, float fz, float falpha, float fbeta, float fgamma)
        {
            byte[] x = BitConverter.GetBytes(fx);
            byte[] y = BitConverter.GetBytes(fy);
            byte[] z = BitConverter.GetBytes(fz);
            byte[] alpha = BitConverter.GetBytes(falpha);
            byte[] beta = BitConverter.GetBytes(fbeta);
            byte[] gamma = BitConverter.GetBytes(fgamma);
            dataSend[0] = 0xFB;
            dataSend[1] = 0xFD;
            for (int i = 0; i < 4; i++)
            {
                dataSend[2 + i] = x[i];
            }
            for (int i = 0; i < 4; i++)
            {
                dataSend[6 + i] = y[i];
            }
            for (int i = 0; i < 4; i++)
            {
                dataSend[10 + i] = z[i];
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

        static void WriteData(byte[] databyte)
        {
            if (sp != null && sp.IsOpen)
            {
                sp.Write(databyte, 0, 32);
                //sp.Write(byte[],offset,count);
            }
        }

    }
}
