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
            Console.WriteLine();
            Console.WriteLine($"DEvaheb v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine($"** Documentation, Contact, Source Code and reporting issues: https://github.com/jorisdg/DEvaheb");
                Console.WriteLine();

                Console.WriteLine("DEvaheb.exe \"path_to_compiled_icarus\"");
                Console.WriteLine();
                Console.WriteLine("Optional arguments:");
                Console.WriteLine("   -output \"filepath\"      path and filename to save decompiled source to");
                Console.WriteLine("   -extension \"icarus\"     file extension for new file");
                Console.WriteLine("   -open \"filepath\"        path and filename to an executable to open the new file with");
                Console.WriteLine("   -nocompat               don't make files compatible with BehavED");
                Console.WriteLine("   -variables \"csv path\"   path to alternate variables CSV file");
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
                List<string> sourceFiles = new List<string>();
                string targetPath = string.Empty;
                string editor = string.Empty;
                bool behavedCompatibility = true;
                string variablesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "variable_types.csv");

                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-output", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            targetPath = args[i];

                            if (targetPath.StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Console.WriteLine("ERROR! Expected file path after \"-output\" parameter");
                                return;
                            }
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

                            if (editor.StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Console.WriteLine("ERROR! Expected file path after \"-open\" parameter");
                                return;
                            }
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
                            extension = args[i];

                            if (extension.Contains("\\", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Console.WriteLine("ERROR! Expected file extension after \"-extension\" parameter, for example \"icarus\" or \"txt\"");
                                return;
                            }

                            if (extension.StartsWith(".", StringComparison.InvariantCultureIgnoreCase))
                            {
                                extension = extension.Substring(1);
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file extension after \"-extension\" parameter, for example \"icarus\" or \"txt\"");
                            return;
                        }
                    }
                    else if (args[i].Equals("-nocompat", StringComparison.InvariantCultureIgnoreCase))
                    {
                        behavedCompatibility = false;
                    }
                    else if (args[i].Equals("-variables", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            variablesPath = args[i];

                            if (variablesPath.StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                            {
                                Console.WriteLine("ERROR! Expected file path after \"-variables\" parameter");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file path after \"-variables\" parameter");
                            return;
                        }
                    }
                    else if (!args[i].StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var sourceFile = args[i];

                        if (!File.Exists(sourceFile))
                        {
                            Console.WriteLine($"ERROR! File \"{sourceFile}\" doesn't exist");
                            return;
                        }

                        sourceFiles.Add(sourceFile);
                    }
                    else
                    {
                        Console.WriteLine($"Unknown argument {args[i]}");
                        return;
                    }
                }

                if (!sourceFiles.Any())
                {
                    Console.WriteLine("No source file specified");
                    return;
                }

                string targetFile = targetPath;
                foreach (var sourceFile in sourceFiles)
                {
                    var output = Read(sourceFile);

                    var vars = DEvahebLib.Variables.FromCsv(variablesPath);

                    // if no target specified
                    if (string.IsNullOrEmpty(targetPath)) 
                    {
                        targetFile = Path.ChangeExtension(sourceFile, extension);
                    }
                    // If a directory was specified
                    else if (Directory.Exists(targetPath))
                    {
                        targetFile = Path.Join(targetPath, Path.ChangeExtension(Path.GetFileName(sourceFile), extension));
                    }
                    // assume anything else was a file, so we just keep the directory and swap filenames
                    else
                    {
                        targetFile = Path.Join(Path.GetDirectoryName(targetPath), Path.ChangeExtension(Path.GetFileName(sourceFile), extension));
                    }

                    File.WriteAllText(targetFile, GenerateSource(vars, output, parity: behavedCompatibility ? SourceCodeParity.BehavED : SourceCodeParity.BareExpressions));

                }

                // Only open editor if doing exactly 1 file
                if (sourceFiles.Count == 1 && !string.IsNullOrWhiteSpace(editor))
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