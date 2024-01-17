using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TunnelDweller.NetCore.Randomness
{
    public static class SecureRandom
    {
        public static char[] DEFAULT_CHAR_SET = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower() + "1234567890").ToCharArray();

        private static Random rand;
        static SecureRandom()
        {
            // Generate a cryptographically secure random seed
            byte[] seedBytes = new byte[4];
            using (RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider())
            {
                cryptoProvider.GetBytes(seedBytes);
            }
            int seed = BitConverter.ToInt32(seedBytes, 0);

            // Create a new instance of the Random class using the secure seed
            rand = new Random(seed);
        }

        public static int NextInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static double NextDouble(double min, double max)
        {
            return rand.NextDouble() * (max - min) + min;
        }

        public static float NextFloat(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
        public static string NextString(int length, char[] characters)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = characters[rand.Next(0, characters.Length)];
            }
            return new string(result);
        }
    }
}
