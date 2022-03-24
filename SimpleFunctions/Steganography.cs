using System;

namespace P1_1
{
    class Program
    {
        static void Main(string[] args)
        {

            byte[] bmpBytes = new byte[] {0x42, 0x4D, 0x4C ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x00 ,0x1A ,0x00 ,0x00 ,0x00, 0x0C, 0x00,
                                            0x00, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
                                            0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00,
                                            0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF ,0x00 ,0x00 ,0xFF ,0xFF ,0xFF ,0xFF ,0xFF,
                                            0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00};
            
            // secret message argument stored in array
            byte[] byte_msg_arr = Array.ConvertAll(args[0].Split(' '), x => Convert.ToByte(x, 16));

            //loop through each byte to change
            int msg_position = -1;
            
            for (int i = 26; i < bmpBytes.Length; i++) {
                int index = i - 26;
                byte bit_position = (byte) (4-(index % 4));
                if (bit_position == 4) {
                    msg_position += 1;
                }
                
                byte hide_two_bits = (byte) ((byte_msg_arr[msg_position] >> (byte)((bit_position-1)*2)) & 0x03);
                
                bmpBytes[i] = (byte) (bmpBytes[i] ^ hide_two_bits); 
            }
            
            Console.WriteLine(BitConverter.ToString(bmpBytes).Replace( "-", " " ));
        }
    }
}
