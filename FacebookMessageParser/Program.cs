using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FacebookMessageParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseFacebookFormat("message.htm", "message.json");

            Console.WriteLine("Done! (press any key to exit)");
            Console.ReadKey();
        }

        private static void ParseFacebookFormat(string inputFile, string outputFile)
        {
            Console.WriteLine("Loading input html...");
            var inputText = File.ReadAllText(inputFile);
            Console.WriteLine("Loaded input html!");

            Console.WriteLine("Parsing....");
            var parser = new MessageParser();
            var messages = parser.Parse(inputText);

            var json = JsonConvert.SerializeObject(messages, Formatting.Indented);
            if (outputFile != null)
                File.WriteAllText(json, outputFile);
            else Console.WriteLine(json);
        }
    }
}
