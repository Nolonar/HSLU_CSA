using System;
using System.Collections.Generic;
using System.Linq;

namespace Hello_World
{
    class Program
    {
        private static readonly Dictionary<string, Func<string>> map = new Dictionary<string, Func<string>>()
        {
            ["random"] = GetRandom
        };

        static void Main(string[] args)
        {
            string arg = args.FirstOrDefault();
            string message = map.ContainsKey(arg) ? map[arg]() : "Hello World";
            Console.WriteLine(message);
        }

        static string GetRandom()
        {
            return new Random().Next(100).ToString();
        }
    }
}
