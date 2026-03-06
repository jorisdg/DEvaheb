using DEvahebLib;
using DEvahebLib.Parser;
using DEvahebLib.Visitors;

namespace suraci
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine($"Suraci v{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");
            Console.WriteLine();

            if (args.Length == 0)
            {
                Console.WriteLine($"** Documentation, Contact, Source Code and reporting issues: https://github.com/jorisdg/DEvaheb");
                Console.WriteLine();

                Console.WriteLine("suraci.exe \"path_to_icarus_sourcefile\"");
                Console.WriteLine();
                Console.WriteLine("Optional arguments:");
                Console.WriteLine("   -output \"filepath\"      path and filename to save compiled IBI to");
                Console.WriteLine("   -v132                   create IBI files compatible with v1.32 of Icarus (Elite Force, SoF2)");
                Console.WriteLine("   -v133   (default)       create IBI files compatible with v1.33 of Icarus (Jedi Knight, Jedi Academy)");
                Console.WriteLine("   -a                      compile all files recursively");
                Console.WriteLine();
                Console.WriteLine("   The original -e flag of IBIze.exe is ignored");
                Console.WriteLine();
                Console.WriteLine("if output file path is ommitted, IBI file will be placed next to the original source file");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("    suraci.exe \"C:\\Temp\\real_scripts\\intro.txt\"");
                Console.WriteLine();
            }
            else
            {
                List<string> sourceFiles = new List<string>();
                string targetPath = string.Empty;
                bool v133 = true;
                bool allFilesRecursively = false;

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
                    else if (args[i].Equals("-v132", StringComparison.InvariantCultureIgnoreCase))
                    {
                        v133 = false;
                    }
                    else if (args[i].Equals("-a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        allFilesRecursively = true;
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
                    else if (!args[i].Equals("-e", StringComparison.InvariantCultureIgnoreCase)) // error on anything other than -e which is an IBIze.exe parameter that BehavED uses
                    {
                        Console.WriteLine($"Unknown argument {args[i]}");
                        return;
                    }
                }

                if (allFilesRecursively)
                {
                    var txtFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.txt", SearchOption.AllDirectories);
                    sourceFiles.AddRange(txtFiles);
                }

                if (!sourceFiles.Any())
                {
                    Console.WriteLine("No source file specified");
                    return;
                }

                bool anyErrors = false;

                string targetFile = targetPath;
                foreach (var sourceFile in sourceFiles)
                {
                    // if no target specified
                    if (string.IsNullOrEmpty(targetPath))
                    {
                        targetFile = Path.ChangeExtension(sourceFile, "IBI");
                    }
                    // If a directory was specified
                    else if (Directory.Exists(targetPath))
                    {
                        targetFile = Path.Join(targetPath, Path.ChangeExtension(Path.GetFileName(sourceFile), "IBI"));
                    }
                    // assume anything else was a file, so we just keep the directory and swap filenames
                    else
                    {
                        targetFile = Path.Join(Path.GetDirectoryName(targetPath), Path.ChangeExtension(Path.GetFileName(sourceFile), ".IBI"));
                    }

                    var parser = new IcarusParser();

                    Console.WriteLine($"Parsing '{sourceFile}'");

                    var nodes = parser.ParseSourceFile(sourceFile, convertComments: false, includeRem: false);

                    foreach(var diagnostic in parser.Diagnostics)
                    {
                        if (diagnostic.Level == DiagnosticLevel.Error)
                        {
                            anyErrors = true;
                        }

                        Console.WriteLine($"{diagnostic.Level} {diagnostic.DiagnosticCode} : {diagnostic.Message}, line {diagnostic.Node.Metadata[DEvahebLib.Nodes.Metadata.SourceLine]} column {diagnostic.Node.Metadata[DEvahebLib.Nodes.Metadata.SourceColumn]}");
                    }
                    Console.WriteLine();

                    TransformNodes.Transform(nodes);

                    using (var ms = File.Open(targetFile, File.Exists(targetFile) ? FileMode.Truncate : FileMode.CreateNew))
                    {
                        using (var writer = new BinaryWriter(ms, IbiEncoding.Windows1252))
                        {
                            var generator = new GenerateIBI(writer) { v133 = v133 };
                            generator.Visit(nodes);
                        }
                    }
                }

                Console.WriteLine("Done");

                if (anyErrors)
                {
                    Environment.ExitCode = -1;
                }
            }
        }
    }
}
