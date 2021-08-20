using System;
using xayrga;
using System.IO;
using Be.IO;
using Newtonsoft.Json;


namespace bastool
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red ;
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("!BASTOOL build in debug mode, do not push into release!");
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.ForegroundColor = ConsoleColor.Gray;
            args = ("pack biribiri.json").Split(" ");
#endif
            cmdarg.cmdargs = args;
            var operation = cmdarg.assertArg(0, "Operation");

            switch(operation)
            {
                case "pack":
                    var packFile = cmdarg.assertArg(1, "Input File");
                    var packOut = cmdarg.tryArg(2, null);
                    if (packOut == null)
                        packOut = $"{Path.GetFileNameWithoutExtension(packFile)}.bas";
                    packBAS(packFile, packOut);
                    break;
                case "unpack":
                    var unpackFile = cmdarg.assertArg(1, "Input File");
                    var unpackOut = cmdarg.tryArg(2, null);
                    if (unpackOut == null)
                        unpackOut = $"{Path.GetFileNameWithoutExtension(unpackFile)}.json";
                    unpackBAS(unpackFile, unpackOut);                       
                    break;
                case "help":
                default:
                    if (operation.Contains(".bas"))
                        Main(new string[] { "unpack", operation });
                    else if (operation.Contains(".json"))
                        Main(new string[] { "pack", operation });
                    else                  
                        cmdarg.assert(true, $"Unrecognized task {operation}");
                    break;
            }            
        }

        public static void unpackBAS(string file, string output)
        {
            cmdarg.assert(!File.Exists(file), $"File {file} doesn't exist or cannot be accessed.");
            var stream = File.OpenRead(file);
            var reader = new BeBinaryReader(stream);
            var basData = new BinaryAudioSequence();
            basData.read(reader); // Try to load BAS.
            var jsondata = JsonConvert.SerializeObject(basData, Formatting.Indented);
            File.WriteAllText(output, jsondata);
            Console.WriteLine($"Success! Output {output}");
        }

        private static void packBAS(string file, string output)
        {
            cmdarg.assert(!File.Exists(file), $"File {file} doesn't exist or cannot be accessed.");
            var outFile = File.OpenWrite(output);
            var outWriter = new BeBinaryWriter(outFile);
            var jData = File.ReadAllText(file);
            BinaryAudioSequence bas = null;
            try
            {
                bas = JsonConvert.DeserializeObject<BinaryAudioSequence>(jData);
            } catch (Exception E)
            {
                cmdarg.assert($"JSON Parse error: {E.Message}");
#if DEBUG 
                Console.WriteLine(E.ToString());
#endif
                Environment.Exit(0);
            }

            bas.EntryCount = (ushort)bas.entries.Length; // update before write.
            bas.write(outWriter);
            outFile.Flush();
            outFile.Close();
            Console.WriteLine($"Success! Output {output}");
        }
    }
}
