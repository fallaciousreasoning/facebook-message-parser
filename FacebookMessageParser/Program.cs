using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;
using Newtonsoft.Json;

namespace FacebookMessageParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "message-pretty.htm";
            string outputFile = null;
            var parseOnly = true;
            List<string> from = null;

            var parser = new FluentCommandLineParser();
            parser.Setup<bool>('p', "parse")
                .Callback(arg => parseOnly = arg)
                .SetDefault(true);
            parser.Setup<string>('i', "input")
                .Callback(arg => inputFile = arg);
            parser.Setup<string>('o', "ouput")
                .Callback(arg => outputFile = arg);
            parser.Setup<List<string>>('f', "from")
                .Callback(arg =>
                {
                    from = arg;
                    parseOnly = false;
                });

            parser.Parse(args);

            if (parseOnly)
                ParseFacebookFormat(inputFile, outputFile);
            else FilterMessages(inputFile, from, outputFile);

            Console.WriteLine("Done! (press any key to exit)");
            Console.ReadKey();
        }

        private static List<Message> FilterMessages(string inputFile, List<string> from, string outputFile=null)
        {
            var json = inputFile.EndsWith(".htm") ? ParseFacebookFormat(inputFile) : File.ReadAllText(inputFile);

            Console.WriteLine("Loading json...");
            var messages = JsonConvert.DeserializeObject<List<Message>>(json);
            Console.WriteLine("Done!");

            Console.WriteLine("Filtering..");
            var result = messages.Where(message => from.Any(f => f == message.Sender)).ToList();
            Console.WriteLine("Done!");

            if (outputFile != null)
            {
                var outputJson = JsonConvert.SerializeObject(result, Formatting.Indented);
                File.WriteAllText(outputFile, outputJson);
            }

            return result;
        }

        private static string ParseFacebookFormat(string inputFile, string outputFile=null)
        {
            Console.WriteLine("Loading input html...");
            var inputText = File.ReadAllText(inputFile);
            Console.WriteLine("Loaded input html!");

            Console.WriteLine("Parsing....");
            var parser = new MessageParser();
            var messages = parser.Parse(inputText);

            var json = JsonConvert.SerializeObject(messages, Formatting.Indented);
            if (outputFile != null)
                File.WriteAllText(outputFile, json);

            return json;
        }
    }
}
