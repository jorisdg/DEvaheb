﻿using System.Diagnostics;
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
                Console.WriteLine("DEvaheb v2.0 *ALPHA*");
                Console.WriteLine();

                Console.WriteLine("DEvaheb.exe \"path_to_compiled_icarus\" [-output \"path_for_new_source_file\"] [-open \"path_to_editor_to_open\"]");
                Console.WriteLine();
                Console.WriteLine("if output file path is ommitted, IBI file name and path are reused, but with extension .icarus");
                Console.WriteLine("if option third parameter is specified, the generated source file is opened");

                Console.WriteLine();
                Console.WriteLine("Example: DEvaheb.exe \"C:\\Temp\\real_scripts\\intro.IBI\" -open \"notepad\"");
                Console.WriteLine();
            }
            else
            {
                string sourceFile = args[0];
                string targetFile = Path.ChangeExtension(sourceFile, "icarus");
                string editor = string.Empty;

                if (!File.Exists(sourceFile))
                {
                    Console.WriteLine($"ERROR! File \"{sourceFile}\" doesn't exist");
                    return;
                }

                if (args.Length > 1)
                {
                    for (int i = 1; i < args.Length; i++)
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
                                Console.WriteLine("ERROR! Expected file path after \"output\" parameter");
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
                                Console.WriteLine("ERROR! Expected file path after \"open\" parameter");
                                return;
                            }
                        }
                    }
                }

                // "D:\temp\real_scripts\_brig\poormunro.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\test.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\JAscripts\scripts\academy1\intro.IBI"
                // "D:\temp\barrel_costa_loopbck.IBI"
                // "D:\Repos\DEvaheb JEDI_Academy_SDK\Tools\EFScripts\PAK3\_holodeck_garden\boothby_workloop.IBI"

                var output = Read(sourceFile);

                File.WriteAllText(targetFile, GenerateSource(output));

                if (!string.IsNullOrWhiteSpace(editor))
                {
                    Process.Start(editor, $"\"{targetFile}\"");
                }
            }
        }

        public static string GenerateSource(List<Node> nodes, SourceCodeParity parity = SourceCodeParity.BehavED)
        {
            var icarusText = new GenerateIcarus() { Parity = parity };
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