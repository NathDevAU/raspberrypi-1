using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;
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
                        ProcessToggle(tokens);
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

        private static void ProcessToggle(string[] tokens)
        {
            var config = GetBreadBoardConfig();

            var connections = config.Where(c => c.Key.StartsWith("led"))
                .Select(c => new Tuple<ConnectorPin, GpioConnection>(
                                            c.Value,
                                            new GpioConnection(c.Value.Output()))).ToList();

            bool exit = false;
            Thread t = new Thread(o =>
            {
                while (!exit)
                {
                    foreach (var connection in connections)
                    {
                        Console.WriteLine("Blinking: {0}", connection.Item1.ToString());
                        connection.Item2.Blink(connection.Item1);
                    }
                }
            });
            t.Start(connections);
            Console.WriteLine("Press any key to exit");
            var key = Console.ReadKey(true);
            exit = true;
            foreach (var connection in connections)
            {
                connection.Item2.Close();
            }
            Console.WriteLine("Toggle stopped");
        }

        private static Dictionary<string, ConnectorPin> GetBreadBoardConfig()
        {
            //this is just my home hardware current setup
            var config = new Dictionary<string, ConnectorPin>();
            config.Add("ledRed", ConnectorPin.P1Pin29);
            config.Add("ledYellow", ConnectorPin.P1Pin16);
            config.Add("ledGreen", ConnectorPin.P1Pin11);
            config.Add("button", ConnectorPin.P1Pin22);
            return config;
        }
    }

}
