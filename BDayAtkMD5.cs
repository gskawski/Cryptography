using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace P2
{
    class Program
    {
        
        static string create_md5hash(string hashed_str, byte[] salt) {
            // Creates an instance of the default implementation of the MD5 hash algorithm.
            using (var md5Hash = MD5.Create())
            {
                // Byte array representation of source string
                var sourceBytes = Encoding.UTF8.GetBytes(hashed_str);
                var saltedBytes = sourceBytes.Concat(salt).ToArray();
                // Generate hash value(Byte Array) for input data
                var hashBytes = md5Hash.ComputeHash(saltedBytes);
                // Convert hash byte array to string
                var hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                // Output the MD5 hash
                return hash.Substring(0,10);
            }
        }
        
        static void Main(string[] args)
        {
            //Find collisions using a birthday attack
            var str_hash_dict = new Dictionary<string, string>{};
            string chars = "00000000";
            Random random = new Random();
            string alpha_num = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int length = 8;
            byte[] salt = Array.ConvertAll(args[0].Split(' '), x => Convert.ToByte(x, 16));

            str_hash_dict.Add(create_md5hash(chars,salt), chars);

            //generate random alphanumeric strings of length 8 and their MD5 hash and compare to existing hashes 
            while(true) {
                string temp_str = new string(Enumerable.Repeat(alpha_num, length).Select(s => s[random.Next(s.Length)]).ToArray());
                string temp_hash = create_md5hash(temp_str,salt);
                
                if (str_hash_dict.ContainsKey(temp_hash)) {
                    Console.WriteLine($"{temp_str},{str_hash_dict[temp_hash]}");
                    break;
                }
                else {
                    str_hash_dict.Add(temp_hash,temp_str);
                }
            }
        }
    }
}
