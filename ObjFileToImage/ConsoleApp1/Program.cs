
using System;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");
            var c = Console.ReadLine();
            if (c == "write")
            {
                var m = new Maker();
                m.Input();
            }
            else if (c == "read")
            {
                var m = new Maker();
                m.Read();
            }
            Console.ReadLine();
        }
    }

}
