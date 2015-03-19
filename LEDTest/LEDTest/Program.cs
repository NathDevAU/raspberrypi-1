using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Raspberry.IO.GeneralPurpose;

namespace LEDTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Specify command: ");
                string command = Console.ReadLine();

                string[] tokens = command.Split(' ');
                if (tokens.Length == 0)
                    break;

                switch (tokens[0])
                {
                    case "toggle":
                    {
                        ProcessCommand("toggle");
                        break;
                    }
                    case "quit":
                    case "q":
                    case "exit":
                    {
                        Console.WriteLine("Bye!");
                        exit = true;
                        break;
                    }
                }
            }
            Console.WriteLine("Exited..");            
        }
    }
}
