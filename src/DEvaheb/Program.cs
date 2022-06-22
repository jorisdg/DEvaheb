using System.Diagnostics;
using System.Text;
using DEvahebLib.Nodes;
using DEvahebLib.Parser;
using DEvahebLib.Visitors;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("************** THIS IS AN ALPHA VERSION **************");
                Console.WriteLine("***                                                ***");
                Console.WriteLine("***  contact interface@codecrib.com for questions  ***");
                Console.WriteLine("***                                                ***");
                Console.WriteLine("******************************************************");
                Console.WriteLine();
                Console.WriteLine("DEvaheb ALPHAv2.0");
                Console.WriteLine();

                Console.WriteLine("DEvaheb.exe \"path_to_compiled_icarus\"");
                Console.WriteLine();
                Console.WriteLine("Optional arguments:");
                Console.WriteLine("   -output \"filepath\"     path and filename to save decompiled source to");
                Console.WriteLine("   -extension \"icarus\"    file extension for new file");
                Console.WriteLine("   -open \"filepath\"       path and filename to automatically the new file with");
                Console.WriteLine();
                Console.WriteLine("if output file path is ommitted, IBI file name and path are reused, but with extension .icarus or the extension specified in a -extension argument");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("    DEvaheb.exe \"C:\\Temp\\real_scripts\\intro.IBI\" -open \"C:\\Tools\\JEDI_Academy_SDK\\BehavEd.exe\"");
                Console.WriteLine("    DEvaheb.exe -open \"notepad\" -extension txt \"C:\\Temp\\real_scripts\\intro.IBI\"");
                Console.WriteLine();
            }
            else
            {
                string extension = "icarus";
                string sourceFile = string.Empty;
                string targetFile = string.Empty;
                string editor = string.Empty;

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-output", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            targetFile = args[i];
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file path after \"-output\" parameter");
                            return;
                        }
                    }
                    else if (args[i].Equals("-open", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            editor = args[i];
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file path after \"-open\" parameter");
                            return;
                        }
                    }
                    else if (args[i].Equals("-extension", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            extension = args[i].StartsWith(".") ? args[i].Substring(1) : args[i];

                            if (!string.IsNullOrEmpty(targetFile))
                            {
                                targetFile = Path.ChangeExtension(targetFile, $".{extension}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected extension after \"-extension\" parameter");
                            return;
                        }
                    }
                    else if (!args[i].StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sourceFile = args[i];
                        if (string.IsNullOrEmpty(targetFile))
                        {
                            targetFile = Path.ChangeExtension(sourceFile, extension);
                        }

                        if (!File.Exists(sourceFile))
                        {
                            Console.WriteLine($"ERROR! File \"{sourceFile}\" doesn't exist");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown argument {args[i]}");
                        return;
                    }
                }

                if (string.IsNullOrEmpty(sourceFile))
                {
                    Console.WriteLine("No source file specified");
                    return;
                }

                // "D:\temp\real_scripts\_brig\poormunro.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\test.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\JAscripts\scripts\academy1\intro.IBI"
                // "D:\temp\barrel_costa_loopbck.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\EFScripts\PAK3\_holodeck_garden\boothby_workloop.IBI"
                var output = Read(sourceFile);

                var vars = DEvahebLib.Variables.FromCsv("variable_types.csv");

                File.WriteAllText(targetFile, GenerateSource(vars, output));

                if (!string.IsNullOrWhiteSpace(editor))
                {
                    Process.Start(editor, $"\"{targetFile}\"");
                }
            }
        }

        public static string GenerateSource(DEvahebLib.Variables variables, List<Node> nodes, SourceCodeParity parity = SourceCodeParity.BehavED)
        {
            var icarusText = new GenerateIcarusWithAliases(variables) { Parity = parity };
            icarusText.Visit(nodes);

            StringBuilder sb = new StringBuilder();

            if (parity == SourceCodeParity.BehavED)
            {
                sb.AppendLine("//Generated by BehavEd");
                sb.AppendLine();
            }

            sb.Append(icarusText.SourceCode.ToString());

            return sb.ToString();
        }

        public static List<Node> Read(string filename)
        {
            List<Node> nodes = new List<Node>();

            try
            {
                using (var file = new FileStream(filename, FileMode.Open))
                {
                    using (var reader = new BinaryReader(file))
                    {
                        var header = reader.ReadChars(4);

                        if (new string(header) != "IBI\0") // IBI string terminating
                            throw new Exception($"File {filename} is not a valid IBI file");

                        Console.WriteLine($"IBI File Version: {reader.ReadSingle()}");

                        var parser = new IBIParser();
                        while (reader != null && reader.BaseStream.Position < reader.BaseStream.Length)
                        {
                            nodes.Add(parser.ReadIBIBlock(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.ToString());
            }

            return nodes;
        }
    }
}