using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApp
{
    internal class Program
    {
        public static Stopwatch Swatch1 = new Stopwatch();
        public static Stopwatch Swatch2 = new Stopwatch();


        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
            {
                //Swatch1.Reset();
                Swatch2.Reset();

                //Swatch1.Start();
                //Thread.Sleep(TimeSpan.FromMilliseconds(1000d / 60));
                //Swatch1.Stop();
                //Console.WriteLine($"Normal Sleep: {Swatch1.Elapsed.TotalMilliseconds}");

                Swatch2.Start();
                Sleep(1000f / 1000);
                Swatch2.Stop();
                Console.WriteLine($"Custom Sleep: {Swatch2.Elapsed.TotalMilliseconds}");
            }

            Console.ReadLine();
        }

        public static void Sleep(float milliseconds)
        {
            var watch = new Stopwatch();
            watch.Start();
            while(watch.Elapsed.TotalMilliseconds < milliseconds) { }
            watch.Stop();
        }
    }
}
