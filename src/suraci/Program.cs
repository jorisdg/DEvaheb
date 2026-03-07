using System.Text;
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
                Console.WriteLine("   -output \"filepath\"      path or filename to save compiled IBI to");
                Console.WriteLine("   -extension \"icarus\"     file extension to look for when searching folders for source files, default is \"txt\"");
                Console.WriteLine("   -v132                   create IBI files compatible with v1.32 of Icarus (Elite Force, SoF2)");
                Console.WriteLine("   -v133   (default)       create IBI files compatible with v1.33 of Icarus (Jedi Knight, Jedi Academy)");
                Console.WriteLine("   -a                      compile all files recursively");
                Console.WriteLine("   -e                      output errors and warnings to a log file and open it when done");
                Console.WriteLine("   -logpath                path to save log file when using -e option (default is in /logs/ subdirectory of location of suraci.exe)");
                Console.WriteLine("   -strict                 report warnings as errors (and fail compile on warnings)");
                Console.WriteLine();
                Console.WriteLine("   Note: The original -e flag of IBIze.exe was \"pause on error\" which was used by the BehavED editor to show output.");
                Console.WriteLine("         Newer versions of Windows don't show the cmd window, essentially hanging BehavED. Using a file is the alternative.");
                Console.WriteLine();
                Console.WriteLine("if output file path is ommitted, IBI file will be placed next to the original source file");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("    suraci.exe \"C:\\Temp\\real_scripts\\intro.txt\" [compile intro.txt to intro.IBI]");
                Console.WriteLine("    suraci.exe \"C:\\Temp\\real_scripts\" -a [compile all scripts in real_scripts and all subdirectories");
                Console.WriteLine("    suraci.exe \"C:\\Temp\\real_scripts\" -extension icarus [compile all scripts in real_scripts that have extension .icarus");
                Console.WriteLine();
            }
            else
            {
                string extension = "txt";
                string logDirectory = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "logs");
                List<string> sourceFiles = new();
                string targetPath = string.Empty;
                List<string> sourceDirectories = new();
                bool v133 = true;
                bool allFilesRecursively = false;
                bool strict = false;
                bool openLog = false;

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
                                Environment.ExitCode = -1;
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file path after \"-output\" parameter");
                            Environment.ExitCode = -1;
                            return;
                        }
                    }
                    else if (args[i].Equals("-extension", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            extension = args[i];

                            if (extension.StartsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                            {
                                extension = extension.Substring(1);

                                if (extension.EndsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    extension = extension.Substring(0, extension.Length - 1);
                                }
                            }

                            if (extension.StartsWith(".", StringComparison.InvariantCultureIgnoreCase))
                            {
                                extension = extension.Substring(1);
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Expected file extension after \"-extension\" parameter, for example \"icarus\" or \"txt\"");
                            Environment.ExitCode = -1;
                            return;
                        }
                    }
                    else if (args[i].Equals("-v132", StringComparison.InvariantCultureIgnoreCase))
                    {
                        v133 = false;
                    }
                    else if (args[i].Equals("-strict", StringComparison.InvariantCultureIgnoreCase))
                    {
                        strict = true;
                    }
                    else if (args[i].Equals("-a", StringComparison.InvariantCultureIgnoreCase))
                    {
                        allFilesRecursively = true;
                    }
                    else if (!args[i].StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var sourceFile = args[i];

                        if (Directory.Exists(sourceFile))
                        {
                            sourceDirectories.Add(sourceFile);
                        }
                        else if (!File.Exists(sourceFile))
                        {
                            Console.WriteLine($"ERROR! File or directory \"{sourceFile}\" doesn't exist");
                            Environment.ExitCode = -1;
                            return;
                        }
                        else
                        {
                            sourceFiles.Add(sourceFile);
                        }
                    }
                    else if (args[i].Equals("-e", StringComparison.InvariantCultureIgnoreCase)) // error on anything other than -e which is an IBIze.exe parameter that BehavED uses
                    {
                        openLog = true;
                    }
                    else if (args[i].Equals("-logpath", StringComparison.InvariantCultureIgnoreCase)) // error on anything other than -e which is an IBIze.exe parameter that BehavED uses
                    {
                        if (i + 1 < args.Length)
                        {
                            i++;
                            logDirectory = args[i];

                            if (logDirectory.StartsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                            {
                                logDirectory = logDirectory.Substring(1);

                                if (logDirectory.EndsWith("\\", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    logDirectory = logDirectory.Substring(0, logDirectory.Length - 1);
                                }
                            }

                            if (!Directory.Exists (logDirectory))
                            {
                                Console.WriteLine($"ERROR! Log directory \"{logDirectory}\" doesn't exist");
                                Environment.ExitCode = -1;
                                return;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Unknown argument {args[i]}");
                        Environment.ExitCode = -1;
                        return;
                    }
                }

                if (sourceDirectories.Any())
                {
                    foreach (var sourceDirectory in sourceDirectories)
                    {
                        var txtFiles = Directory.GetFiles(sourceDirectory, $"*.{extension}", allFilesRecursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                        sourceFiles.AddRange(txtFiles);
                    }
                }
                else if (allFilesRecursively)
                {
                    var txtFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), $"*.{extension}", SearchOption.AllDirectories);
                    sourceFiles.AddRange(txtFiles);
                }

                sourceFiles = sourceFiles.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

                if (!sourceFiles.Any())
                {
                    Console.WriteLine("No source file specified or found in directories");
                    Environment.ExitCode = -1;
                    return;
                }

                bool anyErrors = false;

                StringBuilder log = new();

                string targetFile = targetPath;
                foreach (var sourceFile in sourceFiles)
                {
                    bool thisFileErrors = false;

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
                        targetFile = Path.Join(Path.GetDirectoryName(targetPath), Path.ChangeExtension(Path.GetFileName(sourceFile), "IBI"));
                    }

                    var parser = new IcarusParser();

                    Console.WriteLine($"Parsing '{sourceFile}'");
                    if (openLog)
                        log.AppendLine($"Parsing '{sourceFile}'");

                    var nodes = parser.ParseSourceFile(sourceFile, convertComments: false, includeRem: false);
                    TransformNodes.Transform(nodes);

                    foreach (var diagnostic in parser.Diagnostics)
                    {
                        DiagnosticLevel level = (diagnostic.Level == DiagnosticLevel.Warning && strict) ? DiagnosticLevel.Error : diagnostic.Level;

                        if (level == DiagnosticLevel.Error)
                        {
                            anyErrors = true;
                            thisFileErrors = true;
                        }

                        string logEntry = $"{level} {diagnostic.Level.ToString().Substring(0, 3).ToUpper()}{diagnostic.DiagnosticCode.ToString("D4")} : {diagnostic.Message}, line {diagnostic.Node.Metadata[DEvahebLib.Nodes.Metadata.SourceLine]} column {diagnostic.Node.Metadata[DEvahebLib.Nodes.Metadata.SourceColumn]}";
                        Console.WriteLine(logEntry);
                        if (openLog)
                            log.AppendLine(logEntry);
                    }

                    Console.WriteLine();
                    if (openLog)
                        log.AppendLine();


                    if (!thisFileErrors)
                    {
                        using (var ms = File.Open(targetFile, File.Exists(targetFile) ? FileMode.Truncate : FileMode.CreateNew))
                        {
                            using (var writer = new BinaryWriter(ms, IbiEncoding.Windows1252))
                            {
                                var generator = new GenerateIBI(writer) { v133 = v133 };
                                generator.Visit(nodes);
                            }
                        }
                    }
                }

                if (openLog)
                {
                    if (!Directory.Exists(logDirectory))
                    {
                        Directory.CreateDirectory(logDirectory);
                    }

                    string filename = Path.Join(logDirectory, $"{DateTime.Now:yyyyMMddHHmmss}.txt");
                    File.WriteAllText(filename, log.ToString());

                    try
                    {
                        System.Diagnostics.Process process = new();
                        process.StartInfo.FileName = filename;
                        process.StartInfo.UseShellExecute = true;
                        process.Start();
                    }
                    catch
                    {
                        Console.WriteLine($"Failed to open log file {filename}");
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
