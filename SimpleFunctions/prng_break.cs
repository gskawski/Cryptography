using System;
using System.Security.Cryptography;
using System.IO;


namespace P1_2
{
    class Program
    {
        
        private static string Encrypt(byte[] key, string secretString) {
            DESCryptoServiceProvider csp = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream (ms, csp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            StreamWriter sw = new StreamWriter(cs);
            sw.Write(secretString);
            sw.Flush();
            cs.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int) ms.Length);
        }
        
        static void Main(string[] args)
        {
            string plain = args[0];
            string cipher = args[1];
            
            DateTime dt_start = DateTime.Parse("7/3/2020 11:00:00.000 AM",System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt_end = DateTime.Parse("7/4/2020 11:00:00.000 PM",System.Globalization.CultureInfo.InvariantCulture);

            TimeSpan start = dt_start.Subtract(new DateTime(1970, 1, 1));
            TimeSpan end = dt_end.Subtract(new DateTime(1970, 1, 1));

            int min_start = (int) start.TotalMinutes;
            int min_end = (int) end.TotalMinutes;

            for (int i = min_start; i <= min_end; i++) {
                Random rng = new Random(i);
                byte[] key = BitConverter.GetBytes(rng.NextDouble());
                string encrypt_str = Encrypt(key, plain);
                if (cipher.Equals(encrypt_str)) {
                    Console.WriteLine($"{i}");
                    break;
                }
            }

        }
    }
}
