using System;
using System.Security.Cryptography;
using System.Numerics;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace P3
{
    class Program
    {
        
        // Alice computes (gy) x mod N. These two values are equivalent and are the key.
        static byte[] CalcKey(int N_e, BigInteger N_c, int x, BigInteger gy_modN) {
            
            // calculate g or N values for using the formula 2^e - c.
            BigInteger N =  BigInteger.Pow(2, N_e) - N_c; 
            BigInteger key = BigInteger.ModPow(gy_modN, x, N);
            byte[] key_bytes = key.ToByteArray();
            
            return key_bytes;
        }
        
        static string DecryptMsg(byte[] cipherText, byte[] Key, byte[] IV) {
            string plaintext = null;

            //AES class, 256 bit key mode
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider()) {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        
        
        static byte[] EncryptMsg(string msg, byte[] Key, byte[] IV) {
            byte[] encrypted;

            //AES class, 256 bit key mode
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider()) {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(msg);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }


        static void Main(string[] args)
        {
            /*
            ---To replace spaces in a string
            string IV_str = "0" + Regex.Replace(args[0], @"\s+", "");
            //Console.WriteLine(IV_str);
            BigInteger IV = BigInteger.Parse(IV_str, NumberStyles.AllowHexSpecifier);                    // 128-bit IV in hex                = "A2 2D 93 61 7F DC 0D 8E C6 3E A7 74 51 1B 24 B2"
            //Console.WriteLine(IV);
            */
            
            // Arguments
            byte[] IV = Array.ConvertAll(args[0].Split(' '), x => Convert.ToByte(x, 16));
            int g_e = Convert.ToInt32(args[1]);                                                     // g_e in base 10                    = 251
            BigInteger g_c = BigInteger.Parse(args[2]);                                                     // g_c in base 10                    = 456
            int N_e = Convert.ToInt32(args[3]);                                                     // N_e in base 10                    = 255
            BigInteger N_c = BigInteger.Parse(args[4]);                                                     // N_c in base 10                    = 1311
            int alice_x = Convert.ToInt32(args[5]);                                                   // x in base 10                      = 2101864342
            BigInteger gy_modN = BigInteger.Parse(args[6]);                                             // g y mod N in base 10              = 8995936589171851885163650660432521853327227178155593274584417851704581358902
            byte[] msg_to_decrypt = Array.ConvertAll(args[7].Split(' '), x => Convert.ToByte(x, 16));    // encrypted message C in hex        = "F2 2C 95 FC 6B 98 BE 40 AE AD 9C 07 20 3B B3 9F F8 2F 6D 2D 69 D6 5D 40 0A 75 45 80 45 F2 DE C8 6E C0 FF 33 A4 97 8A AF 4A CD 6E 50 86 AA 3E DF"
            string msg_to_encrypt = args[8] ;                                                                // plaintext message P as a string   = AfYw7Z6RzU9ZaGUloPhH3QpfA1AXWxnCGAXAwk3f6MoTx
    
            //Calculate symmetric key via diffie hellman
            byte[] dh_key = CalcKey(N_e, N_c, alice_x, gy_modN);
            
            string decrypted_msg = DecryptMsg(msg_to_decrypt, dh_key, IV);

            byte[] encrypted_msg = EncryptMsg(msg_to_encrypt, dh_key, IV);
            Console.WriteLine(decrypted_msg + "," + BitConverter.ToString(encrypted_msg).Replace("-", " "));
        }
    }
}
