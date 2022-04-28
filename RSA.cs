using System;
using System.Security.Cryptography;
using System.Numerics;
using System.IO;

namespace P4
{
    class Program
    {
        
        
        static BigInteger ext_GCD(BigInteger e_key, BigInteger phi_N) {
            BigInteger d_key,b_y,old_dkey,old_by,quotient,remainder,inter_dkey,inter_by;

            d_key = 0;
            b_y = 1;
            old_dkey = 1;
            old_by = 0;
            while (e_key != 0) {
                quotient = (phi_N-(phi_N % e_key)) / e_key;
                remainder = phi_N % e_key;
                inter_dkey = d_key - old_dkey * quotient;
                inter_by = b_y - old_by * quotient;
                phi_N = e_key;
                e_key = remainder;
                d_key = old_dkey;
                b_y = old_by;
                old_dkey = inter_dkey;
                old_by = inter_by;
            }
            return d_key;
            
        }
        
        
        // Alice computes (gy) x mod N. These two values are equivalent and are the key.
        static BigInteger[] CalcKey(int p_e, BigInteger p_c, int q_e, BigInteger q_c, BigInteger e_key) {
            
            BigInteger p =  BigInteger.Pow(2, p_e) - p_c; 
            BigInteger q =  BigInteger.Pow(2, q_e) - q_c;
            BigInteger N = p * q;
            BigInteger phi_N = (p-1)*(q-1);

            BigInteger d_key = ext_GCD(e_key,phi_N);

            //BigInteger test = (e_key*d_key) % phi_N;
            //Console.WriteLine(test);
            
            BigInteger[] RSA_values = {N, d_key};

            return RSA_values;
        }

        static BigInteger DecryptMsg(BigInteger cipher, BigInteger N, BigInteger d_key) {
            BigInteger plain = BigInteger.ModPow(cipher, d_key, N);
            return plain;
        }

        static BigInteger EncryptMsg(BigInteger plain, BigInteger N, BigInteger e_key) {
            BigInteger cipher = BigInteger.ModPow(plain, e_key, N);
            return cipher;
        }
        
        static void Main(string[] args)
        {
            BigInteger e_key = 65537;
            
            // Arguments
            int p_e = Convert.ToInt32(args[0]);                                                     // g_e in base 10                    = 251
            BigInteger p_c = BigInteger.Parse(args[1]);                                                     // g_c in base 10                    = 456
            int q_e = Convert.ToInt32(args[2]);                                                     // N_e in base 10                    = 255
            BigInteger q_c = BigInteger.Parse(args[3]);                                                     // N_c in base 10                    = 1311
            BigInteger msg_to_decrypt = BigInteger.Parse(args[4]);
            BigInteger msg_to_encrypt = BigInteger.Parse(args[5]);
            
            BigInteger[] RSA_values = CalcKey(p_e, p_c, q_e, q_c, e_key);
            
            BigInteger decrypted_msg = DecryptMsg(msg_to_decrypt, RSA_values[0], RSA_values[1]);

            BigInteger encrypted_msg = EncryptMsg(msg_to_encrypt, RSA_values[0], e_key);
            
            Console.WriteLine(decrypted_msg + "," + encrypted_msg);
            
        }
    }
}
